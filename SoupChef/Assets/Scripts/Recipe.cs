using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour {

	public GameObject[] ingredients = new GameObject[3];
	Ingredient ingScript;  
	public int onionNum = 0; 
	public int tomatoNum = 0; 
	public float maxTime; 

	public bool isExpired; 
	public Image recipeProgressBar; 
	public float progress; 

	// Use this for initialization
	void Start () {
		setIngredients (); 
		//each recipe has 10 seconds to complete before it expires 
		maxTime = 10f;
		isExpired = false; 

		//INSTANTIATE DEFAULTS FOR PROGRESS BAR 
		recipeProgressBar.type = Image.Type.Filled;
		recipeProgressBar.fillMethod = Image.FillMethod.Horizontal;
	}
	
	// Update is called once per frame
	void Update () {
		updateRecipe ();
		
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

	public GameObject[] getIngredients()
	{
		return ingredients; 
	}

	public bool checkIfExpired()
	{
		return isExpired; 
	}

	public void updateRecipe()
	{
		if(progress < maxTime)
		{
			progress += Time.deltaTime;
			recipeProgressBar.fillAmount -= progress/maxTime;
		}
		else{
			isExpired = true;
		}
	}
}


