using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour {

	public bool isGoodSoup; 
	public bool hasSoup; 
	public bool isDirty; 

	public MeshRenderer meshRenderer;
	public Material cleanMaterial; 
	public Material isDirtyMaterial; 
	public int soupType; 
	public Material onionSoup; 
	public Material tomatoSoup;
	public Material comboSoup;
	public Material badSoup;

	public GameObject pot; 
	public Pot potScript;

	public List<GameObject> ingredients; 

	public int onionNum; 
	public int tomatoNum;



	public GameObject soup;
	public MeshRenderer soupMesh; 

	public GameObject recipe; 

	// Use this for initialization
	void Start () {
		meshRenderer = gameObject.GetComponent<MeshRenderer> ();
		potScript = pot.GetComponent<Pot> ();
		soupMesh = soup.GetComponent<MeshRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		changeMaterial();
		setSoup ();
	}

	public void changeMaterial()
	{
		if (hasSoup) {
			soup.SetActive (true);
		} else if (isDirty) {
			soup.SetActive (false);
			meshRenderer.material = isDirtyMaterial;
		} else {
			soup.SetActive (false);
			meshRenderer.material = cleanMaterial;
		}
	}
		
	public void setIngredients(List<GameObject> ing)
	{
		ingredients.Clear ();
		for (int i = 0; i < ing.Count; i++) {
			ingredients.Add (ing [i]);
		}
	}

	public void setSoup()
	{
		if (soupType == 0) {
			soupMesh.material = onionSoup;
		} else if (soupType == 1) {
			soupMesh.material = tomatoSoup;
		} else if (soupType == 2) {
			soupMesh.material = comboSoup;
		} else if (soupType == 3) {
			soupMesh.material = badSoup;
		}
	}


	/*
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Pot") {
			if (potScript.isCooked && potScript.isBadFood && !isDirty && !hasSoup) {
				recipe = potScript.recipe; 
				hasSoup = true; 
				//reset pot script values 
				potScript.isCooked = false;
				potScript.isValidRecipe = false; 
				potScript.isBurned = false;
				potScript.isBadFood = false;
			}
		} 
	}
	*/

}
