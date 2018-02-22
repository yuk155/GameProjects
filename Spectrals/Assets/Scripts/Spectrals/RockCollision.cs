using UnityEngine;
using System.Collections;

public class RockCollision : MonoBehaviour {

	double existTimer;

	// Use this for initialization
	void Start () 
	{


	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnCollisionEnter(Collision other)
	{
		//existTimerMethod ();
		if(other.gameObject.tag != "Spectral")
		{
			Object.Destroy(this.gameObject);
		}

	}
	//so that the rock only exists for a certain amount of time - 10 seconds
	void existTimerMethod()
	{
		existTimer += Time.deltaTime;
		if(existTimer > 5)
		{
			Object.Destroy(this.gameObject);
		}
	}
}
