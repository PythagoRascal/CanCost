using UnityEngine;
using System.Collections;

public class ChangeSceneOnReturn : MonoBehaviour {

	public string Scene;

	void Update () {
		if (Input.GetKey (KeyCode.Return)) {
			Application.LoadLevel(Scene);
		}
	}
}
