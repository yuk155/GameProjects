using UnityEngine;
using System.Collections;

public class Female_button : MonoBehaviour {

	public GameObject CodeHolder;
	Buttons buttons;
	

	// Use this for initialization
	void Start () 
	{
		buttons = CodeHolder.GetComponent<Buttons> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Debug.Log ("Name = " + hit.collider.name);
				Debug.Log ("Tag = " + hit.collider.tag);
				Debug.Log ("Hit Point = " + hit.point);
				Debug.Log ("Object position = " + hit.collider.gameObject.transform.position);
				Debug.Log ("--------------");
			}
		}
*/
	}

	void onMouseDown()
	{
		Debug.Log ("FEMALE BUTTON");
		buttons.isFemale = true;
		buttons.isMale = false;
	}
}
