using UnityEngine;
using System.Collections;
using System.IO;

public class toLevels : MonoBehaviour {

	//public charBubble charbubble;
	int level = 2;
	public int levelBeat = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	void CheckLevel()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			level += 1;
		}
		
		if (Input.GetKey(KeyCode.DownArrow))
		{
			level -= 1;
		}

		if (Input.GetKey(KeyCode.Return))
		{
			if (levelBeat >= (level-1))
			{
				//print("fuck this shit");
				Application.LoadLevel(level);
			
			}
			//else
				//print("Level not yet available");
		}
	}
}
