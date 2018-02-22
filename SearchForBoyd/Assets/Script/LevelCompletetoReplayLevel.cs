using UnityEngine;
using System.Collections;

public class LevelCompletetoReplayLevel : MonoBehaviour {

	public int levelNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
	
		Application.LoadLevel (levelNumber+1);
	}
}
