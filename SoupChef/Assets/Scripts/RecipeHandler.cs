using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeHandler : MonoBehaviour {

	public float spawnTime; 
	public float startTime; 
	public GameObject[] possibleRecipes = new GameObject[3];
	public List<GameObject> recipes = new List<GameObject>(); 
	public GameObject currRecipe; 
	public int recipeIndex; 

	public Vector3 lastPosition; 
	public Vector3 currPosition; 
	public Vector3 spawnPos; 
	public Quaternion spawnRot; 
	public Recipe recipeScript; 
	public int recipeSpawnTime;
	public float lastSpawn; 
	public float currTime; 


	public float adjust; 
	public int moveRecipe; 
	// Use this for initialization
	void Start () {
		//HARD CODEDED FOR THE OFFSET BETWEEN RECIPE OBJECTS 
		adjust = 0.5f;
		recipeIndex = 0;
		spawnPos = transform.position;
		spawnRot = Quaternion.identity;
		//SPAWN INITIAL RECIPE -> Onion only recipe 
		currRecipe = Instantiate(possibleRecipes[recipeIndex], spawnPos, spawnRot); 
		recipes.Add (currRecipe);

	}
	
	// Update is called once per frame
	void Update () {
		currTime += Time.deltaTime;
		//SPAWN A RECIPE BASED ON A RANDOM VALUE DEFINED IN GETSPAWNTIME()
		if (currTime - (lastSpawn + recipeSpawnTime) < 0.2) {
			spawnRecipe ();
			getSpawnTime ();
		}

		//always check recipes for if they are expired or not 
		checkRecipes ();

	}

	public void spawnRecipe()
	{
		recipeIndex = Random.Range (0, possibleRecipes.Length-1); 
		lastPosition = recipes [recipes.Count - 1].transform.position;
		spawnPos.z += adjust;
		currRecipe = Instantiate (possibleRecipes [recipeIndex], spawnPos, spawnRot);
		recipes.Add (currRecipe);
		lastSpawn = Time.deltaTime; 
		currTime = 0; 
	}

	public void getSpawnTime()
	{
		//HARDCODED TO SPAWN RECIPES EVERY 4 to 8 SECONDS 
		recipeSpawnTime = Random.Range (4, 8);
	}

	public void checkRecipes()
	{
		for(int i = 0; i < recipes.Count; i++)
		{
			recipeScript = recipes[i].GetComponent<Recipe> ();
			if (recipeScript.checkIfExpired ()) {
				//remove the recipe from the list
				recipes.Remove (recipes[i]); 
				//move all of the other recipes over 
				for (int j = i; j < recipes.Count; j++) {
					currPosition = recipes [j].transform.position;
					currPosition.z -= adjust; 
					recipes [j].transform.position = currPosition; 
				}
			}
		}
	}
	//FINDS A VALID RECIPE THAT MATCHES A GIVEN SET OF INGREDIENTS 
	public GameObject findValidRecipe(GameObject[] ingredients)
	{
		int onionCount = 0; 
		int tomatoCount = 0; 
		for (int i = 0; i < ingredients.Length; i++) {
			if (ingredients [i].name == "Tomato") {
				tomatoCount++;
			} else if (ingredients [i].name == "Onion") {
				onionCount++;
			}
		}
		foreach (GameObject recipe in recipes) {
			recipeScript = recipe.GetComponent<Recipe> ();
			if(onionCount==recipeScript.onionNum && tomatoCount == recipeScript.tomatoNum)
			{
				return recipe;
			}
		}
		return null; 
	}
	//REMOVES A RECIPE IF IT HAS BEEN COMPLETED 
	public void removeRecipe(GameObject recipe)
	{
		for (int i = 0; i < recipes.Count; i++) {
			recipes.Remove (recipe); 
			for (int j = i; j < recipes.Count; j++) {
				currPosition = recipes [j].transform.position;
				currPosition.z -= adjust; 
				recipes [j].transform.position = currPosition; 
			}
		}
	}



}
