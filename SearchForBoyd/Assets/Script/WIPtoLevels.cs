using UnityEngine;
using System.Collections;

public class WIPtoLevels : MonoBehaviour {

	int level = 2;
	public int levelBeat = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey(KeyCode.UpArrow))
		{
			if (level != 4)
			{
				level += 1;
			}
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			if (level != 2)
			{
				level -= 1;
			}
		}

		if (Input.GetKey(KeyCode.Return))
		{
			if (levelBeat <= (level-1))
			{
				Application.LoadLevel(level);
			}
		}
	}
}
