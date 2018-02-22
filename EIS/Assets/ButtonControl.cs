using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	public GameObject[] gameObjectArray = new GameObject[4];
	public GameObject[] buttonArray = new GameObject[4];

	public int glowArrayIndex = 0;
	public int previousIndex = 3;

	public GameObject NodeTree;
	public chsngeScript changeScript;

	// Use this for initialization
	void Start () {
		//makes the button dissapear 
		buttonArray [0].renderer.enabled = false;

		gameObjectArray [1].renderer.enabled = false;
		gameObjectArray [2].renderer.enabled = false;
		gameObjectArray [3].renderer.enabled = false;

		changeScript = NodeTree.GetComponent<chsngeScript> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(changeScript.gameEnded() == false)
		{
			changeIndex ();
			highlightButton ();
		}
		else
		{
			gameObjectArray [glowArrayIndex].renderer.enabled = false;
			buttonArray [glowArrayIndex].renderer.enabled = true;
		}
	}

	void changeIndex()
	{
		if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			previousIndex = glowArrayIndex;
			if(glowArrayIndex == 0)
			{
				glowArrayIndex = 3;
			}
			else if(glowArrayIndex < 4)
			{
				glowArrayIndex --;
			}
		}
		if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			previousIndex = glowArrayIndex;
			if(glowArrayIndex == 3)
			{
				glowArrayIndex = 0;
			}
			else if (glowArrayIndex > -1)
			{
				glowArrayIndex ++;
			}
		}
	}
	void highlightButton ()
	{
		gameObjectArray [glowArrayIndex].renderer.enabled = true;
		gameObjectArray [previousIndex].renderer.enabled = false;

		buttonArray [glowArrayIndex].renderer.enabled = false;
		buttonArray [previousIndex].renderer.enabled = true;
	}


}
