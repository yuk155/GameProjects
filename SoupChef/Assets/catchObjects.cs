using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catchObjects : MonoBehaviour {

	public GameObject knifeSpawn;
	public GameObject lidSpawn; 
	public GameObject bowlSpawn; 

	public Rigidbody temp; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Knife") {
			temp = other.gameObject.GetComponent<Rigidbody> ();
			temp.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
			other.transform.position = knifeSpawn.transform.position;
		} else if (other.gameObject.tag == "Lid") {
			temp = other.gameObject.GetComponent<Rigidbody> ();
			temp.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
			other.transform.position = lidSpawn.transform.position;
		} else if (other.gameObject.tag == "Bowl") {
			Debug.Log ("BOWL OVERBOARD");
			temp = other.gameObject.GetComponent<Rigidbody> ();
			temp.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
			other.transform.position = bowlSpawn.transform.position;
		}
	}


}
