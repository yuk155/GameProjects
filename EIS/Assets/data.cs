using UnityEngine;
using System.Collections;

public class data : MonoBehaviour {

	public bool isFemale;
	public bool isMale;

	public string path;

	// Use this for initialization
	void Start () 
	{
		isFemale = false;
		isMale = true;

		path  = ("C:/EISData.txt");

		DontDestroyOnLoad (this.gameObject);

		Application.LoadLevel ("TitleScreen");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
