using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

	//Referencess
	public GameObject[] Digits;
	public Sprite[] Sprites;

	//Fields
	private SpriteRenderer[] _spriteRenderers;

	//Unity Methods
	void Awake () {
		_spriteRenderers = new SpriteRenderer[7];
		for (int i = 0; i < 7; i++) {
			_spriteRenderers[i] = Digits[i].gameObject.GetComponent<SpriteRenderer>();
		}
	}

	//Custom Methods
	public void SetScore(int score) {
		int n = score;
		for (int i = 0; i < 7; i++) {
			int digit = n % 10;
			_spriteRenderers[i].sprite = Sprites[digit];
			n /= 10;
		}
	}
}
