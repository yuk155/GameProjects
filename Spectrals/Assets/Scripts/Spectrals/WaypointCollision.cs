using UnityEngine;
using System.Collections;

public class WaypointCollision : MonoBehaviour {

	//Code for tracking when the spectral is in the waypoint or not
	//can also be adapted to do a simplified learning algorithm with the player 

	public bool isColliding = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collision)
	{
		Debug.Log ("COLLISION SCRIPT");
		if (collision.gameObject.tag == "Spectral")
		{
			isColliding = true;
		}

	}

	void OnTriggerExit(Collider collision)
	{
		if(collision.gameObject.tag == "Spectral")
		{
			isColliding = false;
		}
	}


}
