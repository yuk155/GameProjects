using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour {

	public GameObject ingredient; 
	public GameObject[] ingredientArray; 
	public GameObject newObject;
	public int size; 
	public int ptr;
	public Vector3 spawnLoc;
	public Quaternion spawnRot;
	// Use this for initialization
	void Start () {
		//HARDCODED TO ONLY ALLOW 5 INGREDIENTS ON THE MAP AT A TIME
		size = 5; 
		ptr = 0; 
		ingredientArray = new GameObject[size];
		newObject = Instantiate (ingredient, spawnLoc, spawnRot); 
		ingredientArray [ptr] = newObject;
		ptr++;

		spawnRot = Quaternion.identity;
		spawnLoc = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//THIS WILL NOT WORK IN ALL CASES YET 
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Ingredient") {
			//IF THERE IS SPACE IN THE ARRAY 
			if (checkArray()) {
				newObject = Instantiate (ingredient, spawnLoc, spawnRot);
				ingredientArray [ptr] = newObject;
				ptr++;
			} else {
				if (ptr == 0) {
					ptr = 5;
				}
				ptr--; 
				newObject = ingredientArray [ptr];
				Debug.Log ("Object at index: " + ptr + " deleted");
				Destroy (newObject);

				newObject = Instantiate (ingredient, spawnLoc, spawnRot);
				ingredientArray [ptr] = newObject; 
				if (ptr == 4) {
					ptr = 0;
				} else {
					ptr++;
				}

			}
		}			
	}

	public bool checkArray()
	{
		for (int i = 0; i < ingredientArray.Length; i++) {
			if (ingredientArray [i] == null) {
				return true; 
			}
		}
		return false; 
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
}

