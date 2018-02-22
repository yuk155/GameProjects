using UnityEngine;
using System.Collections;

public class LeveltoLevelSelect2 : MonoBehaviour {

	public LevelBeat beat;
	public int levelID;

	// Use this for initialization
	void Start () {
		beat = GameObject.Find ("LevelBeatObject").GetComponent<LevelBeat> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.Return))
		{
			if (beat.levelBeat == levelID)
			{
				beat.levelBeat++;
			}
			Application.LoadLevel (1);
		}
	}

	/*void OnTriggerEnter2D (Collider2D other)
	{
		if (charBubble.levelBeat < 1)
		{
			charBubble.levelBeat++;
		}

		Application.LoadLevel (1); //Take back to Level Select screen
	}*/
}
