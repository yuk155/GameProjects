using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	public Bowl bowlScript; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Bowl") {
			bowlScript = other.gameObject.GetComponent<Bowl> ();
			bowlScript.isDirty = false; 
		}
	}

}
