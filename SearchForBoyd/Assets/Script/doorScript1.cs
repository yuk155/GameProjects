using UnityEngine;
using System.Collections;

public class doorScript1 : MonoBehaviour {

	public GameObject myTrigger;
	public Trigger triggerScript;
	public Vector2 position;
	public Vector2 targetPosition;
	public bool horizontal;
	public int roomOne;
	public int roomTwo;
	
	// Use this for initialization
	void Start () {
		triggerScript = myTrigger.GetComponent<Trigger> ();
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (horizontal) 
		{
			openDoorLeft ();
		}
		else
		{
			openDoorUp ();
		}
	}
	
	// run this code if you want the door to move right/left - currently updating to do so 
	public void openDoorRight()
	{
		switch(triggerScript.on)
		{
		case true: 
			targetPosition = new Vector2(transform.position.x + 4.5f, transform.position.y);
			transform.position = Vector2.Lerp(position,targetPosition, .4f);
			break;
			
		case false:
			//targetPosition = new Vector2(transform.position.x + -3f, transform.position.y);
			transform.position = Vector2.Lerp(transform.position, position, .4f);
			break; 
		}
	}
	public void openDoorLeft()
	{
		switch (triggerScript.on) 
		{
			case true:
			targetPosition = new Vector2(transform.position.x + -4.5f, transform.position.y);
			transform.position = Vector2.Lerp(position, targetPosition , .4f);
			break; 

			case false:
			//targetPosition = new Vector2(transform.position.x + 3f, transform.position.y);
			transform.position = Vector2.Lerp(transform.position, position, .4f);
			break;

		}
	}
	
	//run this code in update if you want the door to move up and down instead
	public void openDoorUp()
	{
		switch (triggerScript.on)
		{
		case true:
			targetPosition = new Vector2(transform.position.x, transform.position.y + 4.5f);
			transform.position = Vector2.Lerp(position,targetPosition, .4f);
			break;
		case false:
			//targetPosition = new Vector2(transform.position.x, transform.position.y - 3f);
			transform.position = Vector2.Lerp(transform.position, position, .4f);
			break;
		}
	}
}
