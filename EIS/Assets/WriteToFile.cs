using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class WriteToFile : MonoBehaviour {

	int[,] saveArray;

	public GameObject ChangeScriptObject;
	chsngeScript changeScript;

	string path;
	string fileName;

	GameObject persistantData;
	data data;

	public bool canWrite = true;

	//string[] stringArray = new string[14];
	string stringArray = "";

	// Use this for initialization
	void Start () 
	{
		changeScript = ChangeScriptObject.GetComponent<chsngeScript> ();

		persistantData = GameObject.FindGameObjectWithTag ("data");
		data = persistantData.GetComponent<data> ();


		path = "C:/Users/Kristen/Desktop/FRIEND user testing/";
		//fileName = "UserTesting_11_15_15.txt";
		//fileName = "test.txt";

		//includes path and filename
		path = data.path;
	}	
	
	// Update is called once per frame
	void Update () 
	{
		//if(Input.GetKeyDown(KeyCode.W))
		if(changeScript.currentNode == 11 || changeScript.currentNode == 12 || changeScript.currentNode == 13)
		{
			if(canWrite)
			{
				writeToFile ();
			}
		}
	}


	void writeToFile()
	{

		saveArray = changeScript.getSaveArray ();
		//Debug.Log (saveArray.Length);

		//HARDCODED FOR LENGTH OF THE SCRIPT-> LENGTH RETURNS THE TOTAL NUMBER OF VALUES
		for(int i = 0; i<6; i++)
		{
			//System.IO.File.WriteAllText(path, changeScript.getNodeTree()[saveArray[i,0],0] + Environment.NewLine);
			//System.IO.File.WriteAllText(path, changeScript.getNodeTree()[saveArray[i,0],saveArray[i,1]] + Environment.NewLine);

			stringArray = stringArray + changeScript.getNodeTree()[saveArray[i,0],0] + Environment.NewLine;
			stringArray = stringArray + changeScript.getNodeTree()[saveArray[i,0],saveArray[i,1]] + Environment.NewLine;

		}
		stringArray = stringArray + "Number of Good Answers: " + changeScript.getGoodAnswerNum () + Environment.NewLine;
		stringArray = stringArray + "Number of Okay Answers: " + changeScript.getOkayAnswerNum () + Environment.NewLine;
		stringArray = stringArray + "Number of Bad Answers: " + changeScript.getBadAnswerNum () + Environment.NewLine;
	

	//	System.IO.File.WriteAllText (path, "Number of Good Answers: " + changeScript.getGoodAnswerNum () + Environment.NewLine);
	//	System.IO.File.WriteAllText (path, "Number of Okay Answers: " + changeScript.getOkayAnswerNum () + Environment.NewLine);
	//	System.IO.File.WriteAllText (path, "Number of Bad Answers: " + changeScript.getBadAnswerNum () + Environment.NewLine);

		//Debug.Log (stringArray);

		System.IO.File.WriteAllText (path, stringArray);

		canWrite = false;

	}


}
