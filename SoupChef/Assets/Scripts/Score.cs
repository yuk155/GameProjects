using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	public int score = 0; 
	public int value; 
	public float maxTime; 
	public float currTime;
	public int time; 

	public bool gameOver; 

	public Text scoreText; 
	public Text TimerText; 
	public Bowl bowlScript; 

	public GameObject recipeHandler;
	public RecipeHandler recipeHandlerScript;

	public GameObject tempRecipe;

	// Use this for initialization
	void Start () {
		value = 50; 	
		gameOver = false;
		maxTime = 300f; 
		currTime = 0; 
		scoreText.text = score.ToString();
		recipeHandlerScript = recipeHandler.GetComponent<RecipeHandler> ();

	}

	
	// Update is called once per frame
	void Update () {
		updateTimer (); 
		if (currTime > maxTime) {
			Debug.Log ("end gameplay");
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Bowl") {
			bowlScript = other.gameObject.GetComponent<Bowl> ();
			if (bowlScript.hasSoup) {
				if (bowlScript.isGoodSoup) {
					Debug.Log ("is good soup");
					tempRecipe = recipeHandlerScript.checkMatching (bowlScript.onionNum, bowlScript.tomatoNum);
					if (tempRecipe != null) {
						Debug.Log ("found matching recipe");
						score += value;
						scoreText.text = score.ToString ();
						bowlScript.hasSoup = false; 
						bowlScript.isDirty = true; 
						bowlScript.onionNum = 0;
						bowlScript.tomatoNum = 0;
						recipeHandlerScript.removeRecipe (tempRecipe);
					} else {
						Debug.Log ("No available recipe");
						scoreText.text = score.ToString ();
						bowlScript.hasSoup = false; 
						bowlScript.isDirty = true; 
						bowlScript.onionNum = 0;
						bowlScript.tomatoNum = 0;
					}

				} else if (!bowlScript.isGoodSoup) {
					score -= value; 
					scoreText.text = score.ToString ();
					bowlScript.hasSoup = false;
					bowlScript.isDirty = true;
					bowlScript.onionNum = 0;
					bowlScript.tomatoNum = 0;
				}



			}
		}
	}

	void updateTimer()
	{
		currTime += Time.deltaTime;	
		time = Mathf.RoundToInt (maxTime - currTime);
		TimerText.text = time.ToString(); 
	}
}
