using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour {

	public float cookingTime; 
	public GameObject[] ingredients; 


	// Use this for initialization
	void Start () {
		//ALl RECIPES ONLY HAVE 3 INGREDIENTS 
		ingredients = new GameObject[3];
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
