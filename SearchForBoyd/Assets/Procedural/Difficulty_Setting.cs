using UnityEngine;
using System.Collections;

public class Difficulty_Setting : MonoBehaviour {

	public int difficultySetting = 0;

	// Use this for initialization
	void Awake () {

		DontDestroyOnLoad (this.gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
