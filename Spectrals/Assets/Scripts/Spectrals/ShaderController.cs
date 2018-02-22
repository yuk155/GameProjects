//Spectrals - Shader Controller Script v1
//created by Shevis Johnson

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShaderController : MonoBehaviour {

	private List<Material> mats = new List<Material> ();

	// Use this for initialization
	void Start () {
		Renderer[] rend_array = transform.parent.gameObject.GetComponentsInChildren<Renderer> ();
		foreach (Renderer rend in rend_array) {
			mats.Add (rend.material);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void HitByDust () {
		StopCoroutine ("SwitchBack");
		foreach (Material mat in mats) {
			mat.SetFloat ("_Covered", 1.0f);
			mat.SetFloat ("_Opacity", 0.7f);
			mat.SetFloat ("_State", 0.0F);
			StartCoroutine ("SwitchBack", mat);
		}
	}

	IEnumerator SwitchBack(Material mat) {
		yield return new WaitForSeconds (5.0f);
		for (float f = 0.7f; f >= 0.0f; f -= 0.01f) {
			mat.SetFloat ("_Opacity", f);
			yield return new WaitForSeconds (0.01f);
		}
		mat.SetFloat ("_Covered", 0.0f);
	}
}
