using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manageGame : MonoBehaviour {
	public GameObject Score;
	public Score scoreScript; 

	// Use this for initialization
	void Start () {
		scoreScript = Score.GetComponent<Score> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (scoreScript.time < 0) {
			//Debug.log pause game
		}
	}
}
