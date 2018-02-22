using UnityEngine;
using System.Collections;

public class MeshController : MonoBehaviour {

	//array to hold the different poses for the friend

	//0 is the start pose; 1 is the good pose; 2 is the sad pose; 
	//3 is okay pose; 4 is the good ending; 5 is bad ending; 6 is okay ending 	
	public GameObject[] malePoseArray = new GameObject[1];
	//has same arrangement as male pose array 
	public GameObject[] femalePoseArray = new GameObject[1];

	//hardcoded for the number of poses 
	public GameObject [] poseArray = new GameObject[7];

	//public int size;

	public bool isMale;
	public bool isFemale;

	public GameObject NodeTree;
	chsngeScript changeScript;

	GameObject persistantData;
	data data;

	// Use this for initialization
	void Start () 
	{

		//	poseArray = new GameObject[size];
		persistantData = GameObject.FindGameObjectWithTag ("data");
		data = persistantData.GetComponent<data> ();

		changeScript = NodeTree.GetComponent<chsngeScript> ();

		isMale = data.isMale;
		isFemale = data.isFemale;

		for(int i = 0; i<7; i++)
		{
			malePoseArray[i].renderer.enabled = false;
			femalePoseArray[i].renderer.enabled = false;
		}

		//sets the male/female arrays
		if(isMale)
		{
			poseArray = malePoseArray;
		}
		if(isFemale)
		{
			poseArray = femalePoseArray;
		}

	}
	
	// Update is called once per	 frame
	void Update () 
	{
		//changePose ();

	}

	public void changePose()
	{
		poseArray [changeScript.previousPose].renderer.enabled = false;
		poseArray [changeScript.setPose].renderer.enabled = true;
	

		/*
		//renders a new pose 
		//Debug.Log ("Change Pose Fired");
		GameObject temp = poseArray [changeScript.setPose].transform.GetChild (1).gameObject;
		Debug.Log("TEMP:" + temp);

		//The number of meshes is 11 for each pose 
		for(int i =0; i<11; i++)
		{
			MeshRenderer childRenderer = (MeshRenderer) temp.transform.GetChild(i).renderer;
			//Renderer childRenderer = temp.transform.GetChild(i).renderer;
			Debug.Log ("Pose: " + childRenderer);
			Debug.Log (temp.transform.GetChild(i));
			childRenderer.renderer.enabled = true;
			Debug.Log (" IS RENDERED" + childRenderer.isVisible);
			Debug.Log(i);
		}

		//Gets rid of old pose 
		GameObject temp2 = poseArray [changeScript.previousPose].transform.GetChild (1).gameObject;

		for(int j = 0; j<11; j++)
		{
			Renderer childRenderer = temp2.transform.GetChild(j).renderer;
			childRenderer.renderer.enabled = false;
		}
		*/
		/*
		Debug.Log (temp);
		Renderer tempRender = temp.transform.GetChild (1).renderer;
		Debug.Log (tempRender);
		tempRender.enabled = true;
		Renderer[] poseRenderer = temp.GetComponentsInChildren<Renderer> ();
		Debug.Log (poseRenderer);
		Debug.Log (poseRenderer.Length);
		foreach(Renderer pose in poseRenderer)
		{
			Debug.Log(changeScript.setPose);
			Debug.Log ("Pose Loop: " + pose);
			pose.enabled = true;
		}
		*/
	}

	public void clearPoses()
	{
		poseArray [0].renderer.enabled = true;
		poseArray [1].renderer.enabled = false;
		poseArray [2].renderer.enabled = false;
		poseArray [3].renderer.enabled = false;
		poseArray [4].renderer.enabled = false;
		poseArray [5].renderer.enabled = false;
		poseArray [6].renderer.enabled = false;


		/*
		//only allows starting pose
		GameObject temp1 = poseArray [1].transform.GetChild (1).gameObject;
		for(int j = 0; j<11; j++)
		{
			Renderer childRenderer = temp1.transform.GetChild(j).renderer;
			childRenderer.renderer.enabled = false;
		}

		GameObject temp2 = poseArray [2].transform.GetChild (1).gameObject;
		for(int j = 0; j<11; j++)
		{
			Renderer childRenderer = temp2.transform.GetChild(j).renderer;
			childRenderer.renderer.enabled = false;
		}

		GameObject temp3 = poseArray [3].transform.GetChild (1).gameObject;
		for(int j = 0; j<11; j++)
		{
			Renderer childRenderer = temp3.transform.GetChild(j).renderer;
			childRenderer.renderer.enabled = false;
		}

		GameObject temp4 = poseArray [4].transform.GetChild (1).gameObject;
		for(int j = 0; j<11; j++)
		{
			Renderer childRenderer = temp4.transform.GetChild(j).renderer;
			childRenderer.renderer.enabled = false;
		}
		*/

	}
}
