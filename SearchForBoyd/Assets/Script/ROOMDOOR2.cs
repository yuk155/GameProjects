using UnityEngine;
using System.Collections;

public class ROOMDOOR2 : MonoBehaviour {

	public GameObject myTrigger;
	public Trigger triggerScript;
	public Vector2 position;
	public Vector2 targetPosition;

	// Use this for initialization
	void Start () {
		triggerScript = myTrigger.GetComponent<Trigger> ();
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		openDoorUp ();
	}

	public void openDoorUp()
	{
		switch (triggerScript.on)
		{
		case true: 
			targetPosition = new Vector2(transform.position.x, transform.position.y + 2f);
			transform.position = Vector2.Lerp(position,targetPosition, .4f);
			break;
			
		case false:
			targetPosition = new Vector2(transform.position.x, transform.position.y - 2f);
			transform.position = Vector2.Lerp(position, targetPosition , .4f);
			break; 
		}
	}
}
