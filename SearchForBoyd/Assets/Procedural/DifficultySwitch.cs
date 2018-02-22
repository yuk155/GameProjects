using UnityEngine;
using System.Collections;

public class DifficultySwitch : MonoBehaviour {

	public int difficulty;
	public Difficulty_Setting difficultySetter;

	// Use this for initialization
	void Start () 
	{
		difficultySetter = GameObject.Find ("DifficultySetting").GetComponent<Difficulty_Setting>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		difficultySetter.difficultySetting = difficulty;
		Application.LoadLevel ("Procedural_Tester");
	}
}
