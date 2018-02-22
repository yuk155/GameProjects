using UnityEngine;
using System.Collections;

public class LeveltoLevelComplete : MonoBehaviour {

	public LevelBeat beat;
	public int levelID;
	public int levelCompleteID;

	// Use this for initialization
	void Start () {
		beat = GameObject.Find ("LevelBeatObject").GetComponent<LevelBeat> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (beat.levelBeat == levelID)
		{
			beat.levelBeat++;
		}

		Application.LoadLevel (levelCompleteID); //Make sure this goes to LevelComplete
	}
}
