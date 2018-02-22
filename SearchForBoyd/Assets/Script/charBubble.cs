using UnityEngine;
using System.Collections;

public class charBubble : MonoBehaviour {

	private float moveDistance = 2.768f;
	public float speed = 1f;
	Vector3 targetPosition = new Vector3(-6.7255f,13.4169f,0);
	public int level = 1;
	private float timer = 0;
	//public int levelBeat = 1;
	
	public LevelBeat beat;
	
	// Use this for initialization
	void Start () {
		beat = GameObject.Find ("LevelBeatObject").GetComponent<LevelBeat> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		timer += Time.deltaTime;

		if (level < 8) 
		{
			if (Input.GetKeyUp (KeyCode.UpArrow) && timer > 0.5 && level < beat.levelBeat) 
			{
				targetPosition = new Vector3 (transform.position.x, transform.position.y + moveDistance, transform.position.z);
				
				level++;
				timer = 0;
			}
		transform.position = Vector3.Lerp (transform.position, targetPosition, .1f);
		}

		if (level > 1) 
		{
			if (Input.GetKeyUp (KeyCode.DownArrow) && timer > 0.5)
			{
				targetPosition = new Vector3 (transform.position.x, transform.position.y - moveDistance, transform.position.z);
				level --;
				timer = 0;
			}
			transform.position = Vector3.Lerp (transform.position, targetPosition, .1f);
		}

		if (Input.GetKey (KeyCode.Return)) {
						Application.LoadLevel (level+1);
				}
	}	
}
