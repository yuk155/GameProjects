using UnityEngine;
using System.Collections;

public class SCRIPTTHING : MonoBehaviour {

	public GameObject myInventory;
	public Inventory inventoryScript;
	public GameObject myDoor;
	public DoorScript2 doorScript;

	// Use this for initialization
	void Start () {
		inventoryScript = myInventory.GetComponent<Inventory> ();
		doorScript = myDoor.GetComponent<DoorScript2> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void on2DEnter(Collider other){
		Debug.Log ("ENTERED");
		if (inventoryScript.itemArray [0] != null) 
		{

			doorScript.targetPosition = new Vector2(transform.position.x + 1f, transform.position.y);
			transform.position = Vector2.Lerp(doorScript.position, doorScript.targetPosition, .4f);

		}
		for (int i = 0; i<10; i++) {
			if (inventoryScript.itemArray[i].gameObject.name == "ID CARD"){
				//doorScript.openDoorRight();
				doorScript.targetPosition = new Vector2(transform.position.x + 1f, transform.position.y);
				transform.position = Vector2.Lerp(doorScript.position, doorScript.targetPosition, .4f);
			}

				}


	}
}
