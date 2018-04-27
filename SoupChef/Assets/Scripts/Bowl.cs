using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour {

	public bool hasSoup; 
	public bool isDirty; 

	public MeshRenderer meshRenderer;
	public Material cleanMaterial; 
	public Material hasSoupMaterial;
	public Material isDirtyMaterial; 

	public GameObject pot; 
	public Pot potScript; 

	public GameObject recipe; 

	// Use this for initialization
	void Start () {
		meshRenderer = gameObject.GetComponent<MeshRenderer> ();
		potScript = pot.GetComponent<Pot> ();
	}
	
	// Update is called once per frame
	void Update () {
		changeMaterial;
	}

	public void changeMaterial()
	{
		if (hasSoup) {
			meshRenderer.material = hasSoupMaterial; 
		} else if (isDirty) {
			meshRenderer.material = isDirtyMaterial;
		} else {
			meshRenderer.material = cleanMaterial;
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Pot") {
			if (potScript.isCooked && potScript.isValidRecipe && !isDirty && !hasSoup) {
				recipe = potScript.recipe; 
				hasSoup = true; 
				//reset pot script values 
				potScript.isCooked = false;
				potScript.isValidRecipe = false; 
			}
		} 
	}
		

}
