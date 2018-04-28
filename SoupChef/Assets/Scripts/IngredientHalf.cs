using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientHalf : MonoBehaviour {

	// Use this for initialization
	public string name;
	public bool alreadyScaled;


	void Start () {
		alreadyScaled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setIngredientName(string n)
	{
		this.name = n;
	}
}
