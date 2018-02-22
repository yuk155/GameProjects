using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class Buttons : MonoBehaviour {

	public bool isFemale = false;
	public bool isMale = true;

	public GameObject femaleCheck;
	public GameObject maleCheck;

	public GameObject PersistantData;
	data data; 

	Renderer femaleRenderer;
	Renderer maleRenderer;

	public Text pathText;

	// Use this for initialization
	void Start () 
	{

		femaleRenderer = femaleCheck.GetComponent<Renderer> ();
		maleRenderer  = maleCheck.GetComponent<Renderer> ();

		maleRenderer.enabled = true;
		femaleRenderer.enabled = false;

		PersistantData = GameObject.Find ("Persistant Data");
		data = PersistantData.GetComponent<data> ();

		isFemale = data.isFemale;
		isMale = data.isMale;
	
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if(hit.collider.tag == "Female Button")
				{
					Debug.Log ("Female Button");
					isFemale = true;
					data.isFemale = isFemale;
					femaleRenderer.enabled = true;
					isMale = false;
					data.isMale = isMale;
					maleRenderer.enabled = false;
				}
				if(hit.collider.tag == "Male Button")
				{
					Debug.Log ("Male Button");
					isMale = true;
					data.isMale = isMale;
					maleRenderer.enabled = true;
					isFemale = false;
					data.isFemale = isFemale;
					femaleRenderer.enabled = false;
				}
				if(hit.collider.tag == "Start Button")
				{
					Debug.Log ("Start Button");
					Application.LoadLevel("Classroom");
				}
				if(hit.collider.tag == "Exit Button")
				{
					Debug.Log ("Exit Button");
					Application.Quit();
				}
				if(hit.collider.tag == "Browse Button")
				{
					Debug.Log("Browse Button");

					string path = EditorUtility.SaveFilePanel("Choose a Place to Save", "C:/","EISdata.txt","txt");
					pathText.text = path;
					data.path = path;
				}
			}
		}
#endif
	}
	/*
	void onMouseDown(Collider other)
	{
		if (other.gameObject.tag == "Start Button") 
		{
			Debug.Log ("Start Button");
			//Application.LoadLevel("Classroom");
		}
		if(other.gameObject.tag == "Male Button")
		{
			Debug.Log ("Male Button");
			isMale = true;
			isFemale = false;
		}
		if (other.gameObject.tag == "Female Button")
		{
			Debug.Log ("Female Button)
			isMale = false;
			isFemale = true;
		}
	}
	*/

}
