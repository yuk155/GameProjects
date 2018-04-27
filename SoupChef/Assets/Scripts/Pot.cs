﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pot : MonoBehaviour {

	public float cookingTime; 
	public GameObject[] ingredients; 
	int ingredientsNum = 0; 
	public float startTime; 
	public bool isCooking; 
	public bool isCooked; 
	public Image cookingProgessBar;

	//Access recipe handler
	public GameObject recipeHandler; 
	public RecipeHandler recipeHandlerScript; 

	public bool isValidRecipe; 
	public GameObject recipe; 

	//ingredients script 
	Ingredient ingredientScript; 
	public GameObject onionIngredientSpawner; 
	public GameObject tomatoIngredientSpawner;
	public IngredientSpawner ingredientSpawnerScript; 




	// Use this for initialization
	void Start () {
		//ALl RECIPES ONLY HAVE 3 INGREDIENTS 
		ingredients = new GameObject[3];
		//DEFAULT COOKING TIME, HARD CODED 
		cookingTime = 5.0f;
		isCooking = false; 
		isCooked = false;

		//INSTANTIATE DEFAULTS FOR COOKING PROGRESS BAR 
		cookingProgessBar.type = Image.Type.Filled;
		cookingProgessBar.fillMethod = Image.FillMethod.Horizontal;
		cookingProgessBar.fillAmount = 0;

		recipeHandlerScript = recipeHandler.GetComponent<RecipeHandler> ();
		isValidRecipe = false;

	}
	
	// Update is called once per frame
	void Update () {
		cookFood ();
		if (isCooking) {
			startProgressBar (cookingProgessBar, isCooking, isCooked, cookingTime);
		}
		if (isCooked) {
			recipe = recipeHandlerScript.findValidRecipe (ingredients);
			if (recipe != null) {
				isValidRecipe = true;
			}
		}
		
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ingredient") {
			if (ingredientsNum < 4) {
				//if there is less than the max ingredients available, add to the ingredients list
				ingredientScript = other.gameObject.GetComponent<Ingredient>();
				if (ingredientScript.numberOfCuts > 3) {
					ingredients [ingredientsNum] = other.gameObject;
				} else {
					if (ingredientScript.name == "Tomato") {
						ingredientSpawnerScript = tomatoIngredientSpawner.GetComponent<IngredientSpawner> ();
					} else if (ingredientScript.name == "Onion") {
						ingredientSpawnerScript = onionIngredientSpawner.GetComponent<IngredientSpawner> ();
					}
					ingredientSpawnerScript.removeObject (other.gameObject);
				}

			} else {
				Debug.Log ("Max ingredients reached");
			}
		}

	}

	public void cookFood()
	{
		if (ingredientsNum == 3) {
			//START COOKING 
			isCooking = true; 
		}
	}

	//STARTS A GIVEN PROGRESS BAR 
	public void startProgressBar(Image bar, bool check, bool done, float time)
	{
		float progress = 0;
		if (progress < time) {
			progress += Time.deltaTime;
			bar.fillAmount = progress / time;
		}
		else if (progress > time) {
			check = false;
			done = true; 
		}
	}




		

}
