using System.Collections;
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
	public Image burningProgressBar; 
	public bool isBurned; 
	public bool isBurning; 
	public float burnTime; 

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

	public bool isBadFood;



	//rewrite for half ingredients
	public List<GameObject> halfIng;
	public IngredientHalf ingredientHalfScript;

	public GameObject soup; 
	public MeshRenderer soupMesh; 


	public float cookingProgress = 0; 
	public float burningProgress = 0;

	public Bowl bowlScript; 

	List<GameObject> bowlIngredients; 

	public Material onionSoup;
	public Material tomatoSoup;
	public Material comboSoup;
	public Material badSoup; 

	public int onionNum;
	public int tomatoNum; 

	// Use this for initialization
	void Start () {
		//ALl RECIPES ONLY HAVE 3 INGREDIENTS 
		ingredients = new GameObject[3];
		//DEFAULT COOKING TIME, HARD CODED 
		cookingTime = 5.0f;
		isCooking = false; 
		isCooked = false;

		burnTime = 10.0f;
		isBurned = false; 
		isBurning = false; 

		isBadFood = false; 

		//INSTANTIATE DEFAULTS FOR COOKING PROGRESS BAR 
		cookingProgessBar.type = Image.Type.Filled;
		cookingProgessBar.fillMethod = Image.FillMethod.Horizontal;
		cookingProgessBar.fillOrigin = 1;
		cookingProgessBar.fillAmount = 0;

		burningProgressBar.type = Image.Type.Filled;
		burningProgressBar.fillMethod = Image.FillMethod.Horizontal;
		burningProgressBar.fillOrigin = 1; 
		burningProgressBar.fillAmount = 0;

		recipeHandlerScript = recipeHandler.GetComponent<RecipeHandler> ();
		isValidRecipe = false;

		soupMesh = soup.GetComponent<MeshRenderer> ();
		soup.SetActive(false);

		bowlIngredients = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (isCooking) {
			cookingProgress += Time.deltaTime;
			cookingProgessBar.fillAmount = cookingProgress / cookingTime;
			if (cookingProgress > cookingTime) {
				isCooking = false;
				isCooked = true;
				burningProgress = 0;
				countIngredients ();
				if (onionNum == 6) {
					soupMesh.material = onionSoup;
				} else if (onionNum == 3 && tomatoNum == 3) {
					soupMesh.material = comboSoup;
				} else if (tomatoNum == 6) {
					soupMesh.material = tomatoSoup;
				} else {
					soupMesh.material = badSoup;	
				}

				soup.SetActive(true); 
			}
		}
		if (isCooked) {
			isBurning = true;
			cookingProgessBar.fillAmount = 0;
		}
		if (isBurning) {
			burningProgress += Time.deltaTime;
			burningProgressBar.fillAmount = burningProgress / burnTime;
			if (burningProgress > burnTime) {
				isBurning = false; 
				isBurned = true;
				burningProgressBar.fillAmount = 0;
			}
		}
		if (isBurned) {
			soupMesh.material = badSoup;
		}
			
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ingredient") {
			ingredientHalfScript = other.gameObject.GetComponent<IngredientHalf> ();
			halfIng.Add (other.gameObject);
		} else if (other.gameObject.tag == "Lid") {
			if (halfIng.Count > 0) {
				isCooking = true;
				countIngredients ();
				checkIngredientsList ();
				Debug.Log (isValidRecipe);
				startCooking ();
			}
		} else if (other.gameObject.tag == "Bowl") {
			
			bowlScript = other.gameObject.GetComponent<Bowl> ();
			if (isBurned) {
				bowlScript.isGoodSoup = false;
				bowlScript.soupType = 3;
				Debug.Log ("Soup is burned");
			} else if (!isValidRecipe) {
				bowlScript.isGoodSoup = false; 
				bowlScript.soupType = 3;
				Debug.Log ("Not a valid Soup recipe");
			}
			else {
				bowlScript.isGoodSoup = true; 
				bowlScript.setIngredients (bowlIngredients);
				if (onionNum == 6) {
					bowlScript.onionNum = onionNum;
					bowlScript.soupType = 0;
				} else if (onionNum == 3 && tomatoNum == 3) {
					bowlScript.onionNum = onionNum;
					bowlScript.tomatoNum = tomatoNum;
					bowlScript.soupType = 2;
				} else if (tomatoNum == 6) {
					bowlScript.tomatoNum = tomatoNum;
					bowlScript.soupType = 1;
				}
			}
			isBurning = false;
			burningProgressBar.fillAmount = 0; 
			bowlScript.hasSoup = true;
			isCooked = false; 
			isBurned = false; 
			isValidRecipe = false; 
			soup.SetActive (false);
			onionNum = 0;
			tomatoNum = 0;
		}
	}
	public void countIngredients()
	{
		for (int i = 0; i < halfIng.Count; i++) {
			IngredientHalf ing = halfIng [i].GetComponent<IngredientHalf>();
			if (ing.name == "Onion") {
				onionNum++;
			} else if (ing.name == "Tomato") {
				tomatoNum++;
			}
		}
	}

	public void checkIngredientsList()
	{
		//recipe = recipeHandlerScript.findValidRecipe (halfIng);	
		//REPLACED WITH HARDCODED VALUES FOR TIME
		if (onionNum == 6 && tomatoNum == 0) {
			isValidRecipe = true;
		} else if (onionNum == 3 && tomatoNum == 3) {
			isValidRecipe = true;
		} else if (onionNum == 0 && tomatoNum == 6) {
			isValidRecipe = true;
		} else {
			isValidRecipe = false;
		}

	}

	public void removeIngredients()
	{
		for (int i = 0; i < halfIng.Count; i++) {
			Destroy (halfIng [i]);
		}
		halfIng.Clear ();
	}
		

	public void startCooking()
	{
		removeIngredients ();
		cookingProgress = 0;
	}
		
	public void copyToBowl()
	{
		for (int i = 0; i < halfIng.Count; i++) {
			bowlIngredients.Add(halfIng [i]);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Ingredient") {
			halfIng.Remove (other.gameObject);
		}
	}

	/*
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ingredient") {
			if (ingredientsNum < 4) {
				//if there is less than the max ingredients available, add to the ingredients list
				ingredientScript = other.gameObject.GetComponent<Ingredient>();
				if (ingredientScript.numberOfCuts > 3) {
					ingredients [ingredientsNum] = other.gameObject;
				} 
				if (ingredientScript.name == "Tomato") {
					ingredientSpawnerScript = tomatoIngredientSpawner.GetComponent<IngredientSpawner> ();
				} else if (ingredientScript.name == "Onion") {
					ingredientSpawnerScript = onionIngredientSpawner.GetComponent<IngredientSpawner> ();
				}
				//ingredientSpawnerScript.removeObject (other.gameObject);


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


	public void badFoodCheck()
	{
		if (isBurned) {
			isBadFood = true; 
		}
		if (!isValidRecipe) {
			isBadFood = true; 
		}
	}

	public void checkValidFood()
	{
		if (isCooked) {
			recipe = recipeHandlerScript.findValidRecipe (ingredients);
			if (recipe != null) {
				isValidRecipe = true;
			} else {
				isValidRecipe = false; 
			}
		}
	}
	*/



}
