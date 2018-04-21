using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

	public string name;
	public int numberOfCuts; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//IF IT INTERACTS WITH THE OTHER OBJECTS AROUND IT
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Knife") {
			numberOfCuts++;
		}
	}
}
