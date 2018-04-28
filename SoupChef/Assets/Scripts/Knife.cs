using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour {

	public GameObject ingredient;
	public Ingredient ingredientScript;

	public Collider[] currCol; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void checkCollisions()
	{
		currCol = Physics.OverlapBox (transform.position, new Vector3 (0.2f, 0.05f, 0.05f));
		for (int i = 0; i < currCol.Length; i++) {
			if (currCol [i].gameObject.tag == "Ingredient") {
				ingredient = currCol [i].gameObject;
				ingredientScript = ingredient.GetComponent<Ingredient> (); 
				ingredientScript.numberOfCuts++;
			}
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ingredient") {
			ingredient = other.gameObject; 
			ingredientScript = ingredient.GetComponent<Ingredient> ();
			ingredientScript.numberOfCuts++;
		}
	}
}
