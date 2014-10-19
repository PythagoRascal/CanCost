using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	//Properties
	public float MaxSpeed = 10f;
	public float JumpPower = 2f;
	public float ProjectileSpeed = 5f;

	public float PoopInterval = 2.5f;
	public float FeeInterval = 0.3f;

	public int PooledPoopsAmount = 4;
	public int PooledFeesAmount = 16;

	//References
	public GameObject GameManager;
	public LayerMask WhatIsGround;
	public Transform GroundCheck;

	public GameObject Poop;
	public GameObject Fee;

	public GameObject PoopBar;
	public GameObject FeeBar;

	//Fields
	private GameManager _gameManager;

	private GameObject _gameObject;
	private Animator _animator;

	private bool _grounded;
	private float _groundRadius = 0.2f;
	private float _lastPoop = 0;
	private float _lastFee = 0;
	private int _lastDir = 1;

	private List<GameObject> _pooledPoops;
	private List<GameObject> _pooledFees;


	//Unity Methods
	void Awake() {
		_gameObject = this.gameObject;
		_gameManager = GameManager.GetComponent<GameManager> ();
		_animator = _gameObject.GetComponent<Animator> ();
		_grounded = false;
		_lastPoop = PoopInterval;
		_lastFee = FeeInterval;
	}
	void Start() {
		_pooledPoops = new List<GameObject> ();
		_pooledFees = new List<GameObject> ();

		for (int i = 0; i < PooledPoopsAmount; i++) {
			GameObject obj = (GameObject)Instantiate(Poop);
			obj.SetActive(false);
			_pooledPoops.Add(obj);
		}
		for (int i = 0; i < PooledFeesAmount; i++) {
			GameObject obj = (GameObject)Instantiate(Fee);
			obj.SetActive(false);
			_pooledFees.Add(obj);
		}
	}
	void Update() {
		PoopBar.transform.localScale = new Vector3 (((_lastPoop > PoopInterval) ? PoopInterval : _lastPoop) / PoopInterval, 1, 1);
		FeeBar.transform.localScale = new Vector3 (((_lastFee > FeeInterval) ? FeeInterval : _lastFee) / FeeInterval, 1, 1);

		if (_grounded &&  Input.GetKey(KeyCode.W)) {
			_animator.SetBool ("Ground", false);
			rigidbody2D.AddForce (new Vector2(0, JumpPower), ForceMode2D.Impulse);
		}
		else if (Input.GetKey(KeyCode.Space) && (_lastPoop >= PoopInterval )) {
			for (int i = 0; i < _pooledPoops.Count; i++) {
				if (!_pooledPoops[i].activeInHierarchy) {
					_pooledPoops[i].transform.position = _gameObject.transform.position;
					_pooledPoops[i].SetActive(true);
					_lastPoop = 0;
					break;
				}
			}
		}

		if (Input.GetMouseButton (0) && (_lastFee >= FeeInterval)) {
			for (int i = 0; i < _pooledFees.Count; i++) {
				if (!_pooledFees[i].activeInHierarchy) {
					_pooledFees[i].transform.position = _gameObject.transform.position;

					Vector3 sp = Camera.main.WorldToScreenPoint(_gameObject.transform.position);
					Vector3 dir = (Input.mousePosition - sp).normalized;					
					_pooledFees[i].transform.Rotate(dir, Space.Self);
					_pooledFees[i].SetActive(true);
					_pooledFees[i].rigidbody2D.AddForce(dir * ProjectileSpeed); // = (Input.mousePosition - _gameObject.transform.position).normalized * -;
					_lastFee = 0;
					break;
				}
			}
		}

		_lastPoop += Time.deltaTime;
		_lastFee += Time.deltaTime;
	}
	void FixedUpdate () {
		_grounded = Physics2D.OverlapCircle (GroundCheck.position, _groundRadius, WhatIsGround);
		_animator.SetBool ("Ground", _grounded);

		//Horizontal
		float inputH = Input.GetAxis ("Horizontal");
		float movement = inputH * MaxSpeed;
		_animator.SetFloat ("Movement", Mathf.Abs(movement));

		int dir = (inputH == 0) ? 0 : (inputH > 0) ? 1 : -1;
		if (dir != 0 && dir != _lastDir) {
			_gameObject.transform.localScale = new Vector3 (dir, 1, 1);
			((PoopBar.transform.parent).transform.parent).transform.localScale = new Vector3 (dir, 1, 1);
			_lastDir = dir;
		}
		rigidbody2D.velocity = new Vector2 (movement, rigidbody2D.velocity.y);
	}
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag.Equals ("Money")) {
			GameObject coin = collision.gameObject;

			int money = coin.GetComponent<Money>().Amount;
			_gameManager.AddScore(money);

			if (coin.GetComponent<Money>().Destroy) {
				Destroy(coin);
			}
			else {
				coin.SetActive(false);
			}
		}
	}
}