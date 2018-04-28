using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeHandler : MonoBehaviour {

	public float spawnTime; 
	public float startTime; 
	public GameObject[] possibleRecipes;
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

	public GameObject tempRecipe; 


	public float adjust; 
	public int moveRecipe; 
	// Use this for initialization
	void Start () {
		//HARD CODEDED FOR THE OFFSET BETWEEN RECIPE OBJECTS 
		adjust = 0.55f;
		recipeIndex = 0;
		spawnPos = transform.position;
		spawnRot = transform.rotation;
		//SPAWN INITIAL RECIPE -> Onion only recipe 
		currRecipe = Instantiate(possibleRecipes[0], spawnPos, spawnRot); 
		recipes.Add (currRecipe);
		getSpawnTime ();



	}
	
	// Update is called once per frame
	void Update () {
		currTime += Time.deltaTime;
		//SPAWN A RECIPE BASED ON A RANDOM VALUE DEFINED IN GETSPAWNTIME()
		if ((int)currTime % recipeSpawnTime == 0 && (int)currTime > 0) {
			spawnRecipe ();
			getSpawnTime ();
		}

		//always check recipes for if they are expired or not 
		checkRecipes ();

	}

	public void spawnRecipe()
	{
		recipeIndex = Random.Range (0, possibleRecipes.Length); 
		lastPosition = recipes [recipes.Count-1].transform.position;
		spawnPos = lastPosition;
		spawnPos.z += adjust;
		currRecipe = Instantiate (possibleRecipes [recipeIndex], spawnPos, spawnRot);
		recipes.Add (currRecipe);
		lastSpawn = Time.deltaTime; 
		currTime = 0; 
	}

	public void getSpawnTime()
	{
		//HARDCODED TO SPAWN RECIPES EVERY 4 to 8 SECONDS 
		recipeSpawnTime = Random.Range (7, 10);
		//Debug.Log (recipeSpawnTime);
	}

	public void checkRecipes()
	{
		for(int i = 0; i < recipes.Count; i++)
		{
			recipeScript = recipes[i].GetComponent<Recipe> ();
			if (recipeScript.checkIfExpired ()) {
				//remove the recipe from the list
				tempRecipe = recipes [i];
				recipes.Remove (recipes[i]); 
				Destroy (tempRecipe);

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
	public GameObject findValidRecipe(List<GameObject> ingredients)
	{
		int onionCount = 0; 
		int tomatoCount = 0; 
		IngredientHalf temp; 
		for (int i = 0; i < ingredients.Count; i++) {
			temp = ingredients [i].GetComponent<IngredientHalf> ();
			if (temp.name == "Tomato") {
				tomatoCount++;
			} else if (temp.name == "Onion") { 
				onionCount++;
			}
		}
		Debug.Log ("Ing Tomato count:" + tomatoCount);
		Debug.Log ("Ing Onion count: " + onionCount);
		foreach (GameObject recipe in possibleRecipes) {
			recipeScript = recipe.GetComponent<Recipe> ();
			if(onionCount==recipeScript.onionNum && tomatoCount == recipeScript.tomatoNum)
			{
				return recipe;
			}
		}
		return null; 
	}

	public GameObject checkMatching(int onion, int tomato)
	{
		Debug.Log ("enter checkMatching");
		foreach(GameObject recipe in recipes)
		{
			recipeScript = recipe.GetComponent<Recipe>();
			if(onion==recipeScript.onionNum && tomato == recipeScript.tomatoNum)
			{
				return recipe;
			}
		}
		return null;
	}

	//REMOVES A RECIPE IF IT HAS BEEN COMPLETED 
	public void removeRecipe(GameObject recipe)
	{
		int temp = recipes.IndexOf (recipe);
		recipes.Remove (recipe);
		Destroy (recipe);
		for (int i = temp; i < recipes.Count; i++) {
			currPosition = recipes [i].transform.position;
			currPosition.z -= adjust; 
			recipes [i].transform.position = currPosition; 
		}
	}



}
