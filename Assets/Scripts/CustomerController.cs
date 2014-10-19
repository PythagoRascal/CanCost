using UnityEngine;
using System.Collections;

public class CustomerController : MonoBehaviour {

	public enum CustomerType {
		Man,
		Woman
	}

	//Properties
	public CustomerType Type;
	public float Speed = 4f;
	public float Attack = 5f;
	public float AttackInterval = 1f;
	public float AttackRadius = 0.6f;
	public LayerMask WhatToAttack;

	//Fields
	private GameObject _gameObject;
	private Transform _attackTransform;

	private int _lastDir;
	private float _lastHit;

	//Unity Methods
	void Awake () {
		_gameObject = this.gameObject;
		_attackTransform = GameObject.Find ("Player").transform;
		_lastHit = 0;
	}
	void FixedUpdate () {
		int dir = (_gameObject.transform.position.x > _attackTransform.position.x) ? -1 : 1;
		float movement = dir * Speed;
		rigidbody2D.velocity = new Vector2 (movement, rigidbody2D.velocity.y);

		if (_lastHit >= AttackInterval) {
			RaycastHit2D hit = Physics2D.CircleCast(_gameObject.transform.position, 0.2f, Vector2.right * dir, AttackRadius, WhatToAttack);

			if (hit.collider != null && hit.collider.gameObject.tag.Equals ("Player")) {
				hit.collider.gameObject.GetComponent<PlayerHealth>().Hit(Attack);
			}

			_lastHit = 0;
		}
		_lastHit += Time.deltaTime;

		if (dir != _lastDir) {
			_lastDir = dir;
			Flip();
		}
	}

	//Custom Methods
	void Flip() {
		_gameObject.transform.localScale = new Vector3(_lastDir * -1, 1, 1);
	}
}
