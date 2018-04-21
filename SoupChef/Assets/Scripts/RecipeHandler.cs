using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeHandler : MonoBehaviour {

	public float spawnTime; 
	public float startTime; 
	public GameObject[] recipes;
	public int recipeIndex; 

	// Use this for initialization
	void Start () {
		//INITIALIE WITH 3 DIFFERENT RECIPE PREFABS
		recipes = new GameObject[3];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void spawnRecipe()
	{
		recipeIndex = Random.Range (0, recipes.Length); 
		
	}
}
