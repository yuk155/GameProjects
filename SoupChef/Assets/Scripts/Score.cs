using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	public int score = 0; 
	public int value; 

	public Text scoreText; 
	public Bowl bowlScript; 

	// Use this for initialization
	void Start () {
		value = 50; 	
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Bowl") {
			bowlScript = other.gameObject.GetComponent<Bowl> ();
			if (bowlScript.hasSoup) {
				score += value;
				bowlScript.hasSoup = false; 
				bowlScript.isDirty = true; 
			}
		}
	}
}
