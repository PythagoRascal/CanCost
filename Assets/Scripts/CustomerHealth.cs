using UnityEngine;
using System.Collections;

public class CustomerHealth : MonoBehaviour {

	//Properties
	public float MaxHealth = 40f;
	public GameObject HealthBar;
	public GameObject Loot;
	
	//Fields
	private GameObject _gameObject;
	private float _health;
	
	
	//Unity Methods
	void Awake() {
		_health = MaxHealth;
		_gameObject = this.gameObject;
	}
	
	//Custom Methods
	public void Hit(float attack) {
		_health -= attack;
		float percent = ((_health < 0) ? 0 : _health) / MaxHealth;
		HealthBar.transform.localScale = new Vector3 (percent, 1, 1);
		
		if (_health <= 0) {
			((GameObject)Instantiate(Loot)).transform.position = _gameObject.transform.position;

			_health = MaxHealth;
			HealthBar.transform.localScale = new Vector3 (1, 1, 1);
			_gameObject.SetActive(false);
		}
	}
}
