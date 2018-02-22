using UnityEngine;
using System.Collections;

public class LevelCompletetoNextLevel : MonoBehaviour {

	public int levelNext;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown (){
	
		Application.LoadLevel (levelNext+1);
	}
}
