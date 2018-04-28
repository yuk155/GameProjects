using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

	public string name;
	public int numberOfCuts; 

	public GameObject child;
	public GameObject ingredientSpawner;
	public IngredientSpawner ingredientSpawnerScript;  

	public bool hasSpawned = false;

	GameObject half; 
	GameObject halfManager; 
	HalfManager halfManagerScript; 

	// Use this for initialization
	void Start () {
		numberOfCuts = 0; 
		if (name == "Onion") {
			ingredientSpawner = GameObject.Find ("Onion Spawner");
		} else if (name == "Tomato") {
			ingredientSpawner = GameObject.Find ("Tomato Spawner");
		}
		ingredientSpawnerScript = ingredientSpawner.GetComponent<IngredientSpawner> ();

		halfManager = GameObject.Find ("Half Manager");
		halfManagerScript = halfManager.GetComponent<HalfManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		//detectCollision ();

		if (numberOfCuts > 0) {
			createChildren ();
		}
	}  
	public void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Knife") {
			numberOfCuts++;
		}
	}


	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Knife") {
			//Debug.Log ("Collision detected with knife using trigger");
			numberOfCuts++;
		}
	}

	public void createChildren()
	{
		if (!hasSpawned) {
			halfManagerScript.checkHalves ();
			half = Instantiate (child, transform.position, transform.rotation);
			halfManagerScript.halfIng.Add (half);
			Vector3 secondSpawn = transform.position; 
			secondSpawn.x += 0.25f;
			Instantiate (child, secondSpawn, Quaternion.Inverse(transform.rotation));
			halfManagerScript.halfIng.Add (half);
			hasSpawned = true; 
		}
		ingredientSpawnerScript.removeObject (this.gameObject);
	}
}
