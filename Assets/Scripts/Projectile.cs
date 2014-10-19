using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	//Properties
	public int Damage = 10;
	public float Lifespan = 1f;
	
	//Fields
	private GameObject _gameObject;
	private float _lifeDuration;
	
	void Awake () {
		_gameObject = this.gameObject;
	}
	
	void Update() {
		if (_lifeDuration >= Lifespan) {
			_lifeDuration = 0;
			_gameObject.SetActive(false);
		}
		_lifeDuration += Time.deltaTime;
	}
	
	// Update is called once per frame
	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag.Equals ("Enemy")) {
			CustomerHealth enemy = collision.gameObject.GetComponent<CustomerHealth>();
			enemy.Hit(Damage);
			_lifeDuration = 0;
			_gameObject.SetActive(false);
		}
	}
}
