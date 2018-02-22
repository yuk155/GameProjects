using UnityEngine;
using System.Collections;

public class GhostDetermine : MonoBehaviour {

	public Graph_Builder numberOfButtonRooms;
	int numberOfGhosts;
	public GameObject[] ghostTriggers = new GameObject[5];

	// Use this for initialization
	void Start () 
	{
		for(int ghostTrigger = 0; ghostTrigger < ghostTriggers.Length; ghostTrigger ++)
		{
			ghostTriggers[ghostTrigger].SetActive(false);
		}
		numberOfButtonRooms = GameObject.Find ("Grapher").GetComponent<Graph_Builder> ();
		numberOfGhosts = numberOfButtonRooms.buttonRoomsList.Count + 1;
		Debug.Log ("Number of Ghosts: " + numberOfGhosts);

		if(numberOfGhosts < 5)
		{
			for(int i =0; i < numberOfGhosts; i ++)
			{
				ghostTriggers[i].SetActive(true);
			}
		}

		else
		{
			for(int j = 0; j < ghostTriggers.Length; j ++)
			{
				ghostTriggers[j].SetActive(true);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
