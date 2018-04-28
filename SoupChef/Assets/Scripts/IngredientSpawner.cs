using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour {

	public GameObject ingredient; 
	public GameObject[] ingredientArray; 
	public GameObject newObject;
	public int size; 
	public int ptr;
	public int lastAddedIndex; 
	public Vector3 spawnLoc;
	public Quaternion spawnRot;

	public bool canSpawn; 
	public Vector3 checkTransform; 

	public Collider[] currentCol;

	// Use this for initialization
	void Start () {
		//HARDCODED TO ONLY ALLOW 5 INGREDIENTS ON THE MAP AT A TIME
		size = 5; 
		ptr = 0; 
		ingredientArray = new GameObject[size];
		spawnRot = Quaternion.identity;
		spawnLoc = transform.position;
		newObject = Instantiate (ingredient, spawnLoc, spawnRot); 
		ingredientArray [ptr] = newObject;
		ptr++;

		canSpawn = true; 


	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Ingredient") {
			//Debug.Log ("spawn ingredient");
			//IF THERE IS SPACE IN THE ARRAY 
			canSpawn = checkForSpawn();
			if (canSpawn) {
				ptr = checkArray (); 
				if (checkArray () != -1) {
					newObject = Instantiate (ingredient, spawnLoc, spawnRot);
					ingredientArray [ptr] = newObject;
					lastAddedIndex = ptr;
				} else {
					if (lastAddedIndex > 0) {
						ptr = lastAddedIndex - 1;
					} else {
						ptr = 4; 
					}
					newObject = ingredientArray [ptr];
					//Debug.Log ("Object at index: " + ptr + " deleted");
					Destroy (newObject);

					newObject = Instantiate (ingredient, spawnLoc, spawnRot);
					ingredientArray [ptr] = newObject; 
					lastAddedIndex = ptr; 
				}
			} else {
				//Debug.Log ("Can't spawn");
			}

		}	
	}

	public int checkArray()
	{
		for (int i = 0; i < ingredientArray.Length; i++) {
			if (ingredientArray [i] == null) {
				return i; 
			}
		}
		return -1; 
	}

	public void removeObject(GameObject g)
	{
		for (int i = 0; i < ingredientArray.Length; i++) {
			if (ingredientArray [i] = g) {
				newObject = ingredientArray [i];
				Destroy (newObject);
				ptr = i; 
			}
		}
	}
	//function to check if there is an ingredient in the current space so that it won't spawn an overlaping one 
	public bool checkForSpawn()
	{
		currentCol = Physics.OverlapBox (transform.position, new Vector3(0.1f, 0.1f, 0.1f));
		if (currentCol.Length == 0) {
			return true;
		} else {
			for (int i = 0; i < currentCol.Length; i++) {
				if (currentCol [i].gameObject.tag == "Ingredient") {
					return false;
				}
			}
			return true;
		}
	}
}

