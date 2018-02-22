using UnityEngine;
using System.Collections;

public class Camera_Mover : MonoBehaviour {

	public FloorPlan floorPlanner;
	Camera camera;

	// Use this for initialization
	void Start () 
	{
		floorPlanner = GameObject.Find ("FloorPlanner").GetComponent<FloorPlan> ();
		camera = gameObject.GetComponent<Camera> ();
		if(floorPlanner.numberOfWalls <= 3)
		{
			camera.orthographicSize = 17;
			camera.transform.position = new Vector3(14.5f,12.0f,-10.0f);
		}
		else
		{
			camera.orthographicSize = 23;
			camera.transform.position = new Vector3(24.5f,18.0f,-10.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
