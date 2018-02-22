using UnityEngine;
using System.Collections;

public class toLevel : MonoBehaviour {

	public charBubble levelNum;
	public GameObject level;
	public float b;


	// Use this for initialization
	void Start () {
	
		levelNum = level.GetComponent<charBubble> ();

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey(KeyCode.Return))
		{
			
			Application.LoadLevel (levelNum.level + 1);
			
		}

	}
}
