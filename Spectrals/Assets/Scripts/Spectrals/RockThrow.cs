using UnityEngine;
using System.Collections;

public class RockThrow : MonoBehaviour {

	//PLEASE MAKE SURE EVERYTHING IS SPELLED THE SAME IN ORDER FOR THE CODE TO WORK
	
	public GameObject bossSpectral;
	//HAVE TO CHANGE EVERYTIME THERE IS A NEW ITERATION
	//Boss_AI_V2 bossAI;
	
	GameObject rightHand;
	GameObject leftHand;
	
	GameObject rightRock;
	GameObject leftRock;

	bool isLeft;
	bool isRight;

	public int throwForce;

	// Use this for initialization
	void Start () 
	{
		isLeft = false;
		isRight = false;

		bossSpectral = GameObject.Find ("Boss Spectral").gameObject;
		//bossAI = bossSpectral.GetComponent<Boss_AI_V1> ();
		
		rightHand = bossSpectral.transform.FindChild ("Boss Right Hand").gameObject;
		leftHand = bossSpectral.transform.FindChild ("Boss Left Hand").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
		if(isLeft)
		{
			Debug.Log ("Throwing LeftRock");
			throwLeftRock();
		}
		if(isRight)
		{
			Debug.Log("Throwing Right Rock");
			throwRightRock();
		}
	}

	void OnCollisionExit(Collision other)
	{
		isLeft = true;
		isRight = true;
	}

	void throwLeftRock()
	{
		
		leftRock = leftHand.transform.FindChild ("Spectral Rock(Clone)").gameObject;
		Rigidbody leftRockRigidBody = leftRock.GetComponent<Rigidbody> ();
		//leftRockRigidBody.useGravity = true;
		//leftRockRigidBody.constraints = RigidbodyConstraints.None;
		Vector3 throwVector = transform.forward;
		leftRockRigidBody.AddForce (throwVector * 25);
		//leftRockRigidBody.velocity ();
		
	}
	void throwRightRock()
	{
		rightRock = rightHand.transform.FindChild ("Spectral Rock(Clone)").gameObject;
		Rigidbody rightRockRigidBody = rightRock.GetComponent<Rigidbody> ();
		//rightRockRigidBody.useGravity = true;
	//	rightRockRigidBody.constraints = RigidbodyConstraints.None;
		Vector3 throwVector = transform.forward;
		rightRockRigidBody.AddForce (throwVector * 25);
	}
}
