using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	//Properties
	public float MaxHealth = 100f;
	public GameObject HealthBar;
	public GameObject GameManager;

	//Fields
	private float _health;
	private GameManager _gameManager;


	//Unity Methods
	void Awake() {
		_health = MaxHealth;
		_gameManager = GameManager.gameObject.GetComponent<GameManager>();
	}

	//Custom Methods
	public void Hit(float attack) {
		_health -= attack;
		float percent = ((_health < 0) ? 0 : _health) / MaxHealth;
		HealthBar.transform.localScale = new Vector3 (percent, 1, 1);

		if (_health <= 0) {
			_gameManager.GameOver(false);
		}
	}
}
