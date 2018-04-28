using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfManager : MonoBehaviour {

	public List<GameObject> halfIng; 
	GameObject half; 

	// Use this for initialization
	void Start () {
		halfIng = new List<GameObject> (); 
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void checkHalves()
	{
		/*
		Debug.Log (halfIng.Count);
		if (halfIng.Count > 10) {
			for (int i = 0; i < 2; i++) {
				half = halfIng [0];
				Debug.Log (half.name);
				halfIng.RemoveAt(0);
				Destroy (half);
			}

		}
		*/
	}
}
