using UnityEngine;
using System.Collections;

public class WinBoxPlacement : MonoBehaviour {

	public FloorPlan coordinateMatrix;
	public GameObject floorPlanner;

	// Use this for initialization
	void Start () {
		coordinateMatrix = floorPlanner.GetComponent<FloorPlan> ();
		int yCoordinate = (coordinateMatrix.coordinateMatrixList.Count - 1) * 3;
		int xCoordinate = (coordinateMatrix.coordinateMatrixList [0].Count - 1) * 3;

		transform.position = new Vector2(xCoordinate, yCoordinate);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			Application.LoadLevel("DifficultyChooser");
		}
	}

}
