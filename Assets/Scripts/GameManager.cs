using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour {

	//References
	public GameObject ScoreBoard;
	public GameObject SmallCoin;
	public GameObject BigCoin;
	public GameObject Man;
	public GameObject Woman;

	//Properties
	public int Money = 0;

	public int PooledSmallCoinsAmount = 10;
	public int PooledBigCoinsAmount = 4;
	public int PooledCustomersAmount = 10;

	public float Difficulty = 1f;
	public float CoinSpawnRate = 0.8f;
	public float CoinScatter = 2f;
	public float CustomerSpawnRate = 3.5f;

	//Fields
	private Scoreboard _scoreBoard;
	private float _lastCoinSpawned = 0;
	private float _lastCustomerSpawned = 0;

	private List<GameObject> _smallCoins;
	private List<GameObject> _bigCoins;
	private List<GameObject> _customers;

	//Unity Methods
	void Awake() {
		_scoreBoard = ScoreBoard.GetComponent<Scoreboard> ();
	}
	void Start () {
		_smallCoins = new List<GameObject> ();
		_bigCoins = new List<GameObject> ();
		_customers = new List<GameObject> ();

		for (int i = 0; i < PooledSmallCoinsAmount; i++) {
			GameObject obj = (GameObject)Instantiate(SmallCoin);
			obj.SetActive(false);
			_smallCoins.Add(obj);
		}
		for (int i = 0; i < PooledBigCoinsAmount; i++) {
			GameObject obj = (GameObject)Instantiate(BigCoin);
			obj.SetActive(false);
			_bigCoins.Add(obj);
		}
		for (int i = 0; i < PooledCustomersAmount; i++) {
			GameObject obj = (i % 2 == 0) ? (GameObject)Instantiate(Man) : (GameObject)Instantiate(Woman);
			obj.SetActive(false);
			_customers.Add(obj);
		}
	}
	void Update () {
		//Difficulty
		int progress = (int)Money / 50000;
		float difficulty = 1 + (0.05f * progress);

		//Spawning of Coins
		if (_lastCoinSpawned * difficulty >= CoinSpawnRate) {
			int rand = Random.Range(1, (PooledBigCoinsAmount + PooledSmallCoinsAmount));
			if (rand <= PooledBigCoinsAmount) {
				for (int i = 0; i < _bigCoins.Count; i++) {
					if (!_bigCoins[i].activeInHierarchy) {
						float randPosX = Random.Range(-7.5f, 7.5f);
						float randVelX = Random.Range(-CoinScatter, CoinScatter);

						_bigCoins[i].SetActive(true);
						_bigCoins[i].transform.position = new Vector3(randPosX, 7f, _bigCoins[i].transform.position.z);
						_bigCoins[i].rigidbody2D.velocity = new Vector2(randVelX, _bigCoins[i].rigidbody2D.velocity.y);
						break;
					}
				}
			}
			else if (rand > PooledBigCoinsAmount || (rand <= PooledBigCoinsAmount &&_lastCoinSpawned != 0)) {
				for (int i = 0; i < _smallCoins.Count; i++) {
					if (!_smallCoins[i].activeInHierarchy) {
						float randPosX = Random.Range(-7.5f, 7.5f);
						float randVelX = Random.Range(-CoinScatter, CoinScatter);

						_smallCoins[i].SetActive(true);
						_smallCoins[i].transform.position = new Vector3(randPosX, 7f, _smallCoins[i].transform.position.z);
						_smallCoins[i].rigidbody2D.velocity = new Vector2(randVelX, _smallCoins[i].rigidbody2D.velocity.y);
						break;
					}
				}
			}
			_lastCoinSpawned = 0;
		}
		_lastCoinSpawned += Time.deltaTime;

		//Spawning of Customers
		if (_lastCustomerSpawned * difficulty >= CustomerSpawnRate) {
			for (int i = 0; i < _customers.Count; i++) {
				if (!_customers[i].activeInHierarchy) {
					int side = (Random.Range(0, 100) % 2 == 0) ? 1 : -1;

					_customers[i].transform.position = new Vector3(9.5f * side, -3f, _customers[i].transform.position.z);
					_customers[i].SetActive(true);
					break;
				}
			}
			_lastCustomerSpawned = 0;
		}
		_lastCustomerSpawned += Time.deltaTime;
	}

	//Custom Methods
	public void GameOver(bool win) {
		Application.LoadLevel ("GameOver");
	}
	public void AddScore(int score) {
		Money += score;
		_scoreBoard.SetScore (Money);

		if (Money >= 1000000) {
			GameOver(true);
		}
	}
}