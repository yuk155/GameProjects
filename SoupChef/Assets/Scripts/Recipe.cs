using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour {

	public GameObject[] ingredients = new GameObject[3];
	Ingredient ingScript;  
	public int onionNum = 0; 
	public int tomatoNum = 0; 

	// Use this for initialization
	void Start () {
		setIngredients (); 

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setIngredients()
	{
		for (int i = 0; i < ingredients.Length; i++) {
			ingScript = ingredients [i].GetComponent<Ingredient> ();
			if (ingScript.name == "Tomato") {
				tomatoNum++;
			} else if (ingScript.name == "Onion") {
				onionNum++;
			}
		}
	}

}


