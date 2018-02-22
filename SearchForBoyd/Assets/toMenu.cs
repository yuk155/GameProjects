using UnityEngine;
using System.Collections;

public class toMenu : MonoBehaviour {

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.M))
		{
			Application.LoadLevel("levelComplete_level1");
		}
	}
	void OnMouseDown()
	{
		Application.LoadLevel("levelComplete_level1");
	}

}
