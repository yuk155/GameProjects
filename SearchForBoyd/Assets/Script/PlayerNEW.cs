using UnityEngine;
using System.Collections;

public class PlayerNEW : MonoBehaviour {

	//order for arrays is always still, left, right
	public Sprite[] up;
	public Sprite[] down;
	public Sprite[] left;
	public Sprite[] right;
	
	public SpriteRenderer spriteRender;
	int spriteCounter = 0;
	bool facingRight = true;

	//Move Dist changed so that it will line up nicely with grid (each square = 3 units)
	int moveDistance = 3;
	public RecordingInfo recorder; //RecordingInfo script
	public GameObject recorderObject; //RecordingManager game object
	public Vector3 nextPosition;
	public Vector3 currentPosition;
	public Vector3 previousPosition;
	public string currentlySelectedArray;
	public int movementCounter = 0;
	public string[] currentArray;
	public Vector3 recordingStartPosition;
	public bool isRecording = true;
	public float moveDelay = 1.0f;
	public float moveTimer = 0.0f;
	public bool hitObject = false;
	
	// Use this for initialization
	void Start () {
		spriteRender = gameObject.GetComponent<SpriteRenderer> ();
		recorder = recorderObject.GetComponent<RecordingInfo> ();
		recordingStartPosition = transform.position;
		nextPosition = transform.position; //Set the target position equal to the initial position
		currentPosition = transform.position;
		currentlySelectedArray = "recording 1";
	}
	
	// Update is called once per frame
	void Update () {
		currentArray = setArray (currentlySelectedArray); //Decide which array in RecordingInfo to use
		
		
		if (isRecording)
		{
			//Left
			if (Input.GetKeyDown (KeyCode.A))
			{

				Debug.Log ("A Key");
				//if(facingRight){
				//	Flip();
			//	}
				//if statements for determining the proper sprite
				if(movementCounter == 0)
				{
					spriteCounter = 0;
				}
				else if (currentArray[movementCounter-1] != "Left")
				{
					spriteCounter = 0;
				}
				else if(currentArray[movementCounter-1] == "Left")
				{
					if(spriteCounter == 1)
					{
						spriteCounter = 2;

					}
					else
					{
						spriteCounter = 1;
					}
				}

				//switches the sprite to walk
				switch (spriteCounter)
				{
					case 0:
						spriteRender.sprite = left[0];
						break;
					case 1:
						spriteRender.sprite = left[1];
						break;
					case 2:
						spriteRender.sprite = left[2];
						break;
					default:
						break;
				}
					
				previousPosition = currentPosition;
				nextPosition = new Vector3 (currentPosition.x - moveDistance, currentPosition.y, currentPosition.z);
				AddMovement (currentArray, movementCounter, "Left");
				movementCounter++;
			}
			
			if (Input.GetKeyDown (KeyCode.D)) {
				//if(!facingRight){
				//	Flip();
				//}
				if (movementCounter == 0)
				{
					spriteCounter = 0;
				}
				else if (currentArray[movementCounter-1] != "Right" )
				{
					spriteCounter = 0;
				}
				else if(currentArray[movementCounter-1] == "Right")
				{
					if(spriteCounter == 1)
					{
						spriteCounter = 2;
						
					}
					else
					{
						spriteCounter = 1;
					}
				}
				
				switch (spriteCounter)
				{
				case 0:
					spriteRender.sprite = right[0];
					break;
				case 1:
					spriteRender.sprite = right[1];
					break;
				case 2:
					spriteRender.sprite = right[2];
					break;
				default:
					break;
				}
				
				previousPosition = currentPosition;
				nextPosition = new Vector3 (currentPosition.x + moveDistance, currentPosition.y, currentPosition.z);
				AddMovement (currentArray, movementCounter, "Right");
				movementCounter++;
			}
			
			if (Input.GetKeyDown (KeyCode.W)) {
				if (movementCounter == 0)
				{
					spriteCounter = 0;
				}
				else if (currentArray[movementCounter-1] != "Up" )
				{
					spriteCounter = 0;
				}
				else if(currentArray[movementCounter-1] == "Up")
				{
					if(spriteCounter == 1)
					{
						spriteCounter = 2;
						
					}
					else
					{
						spriteCounter = 1;
					}
				}
				
				switch (spriteCounter)
				{
				case 0:
					spriteRender.sprite = up[0];
					break;
				case 1:
					spriteRender.sprite = up[1];
					break;
				case 2:
					spriteRender.sprite = up[2];
					break;
				default:
					break;
				}
				previousPosition = currentPosition;
				nextPosition = new Vector3 (currentPosition.x, currentPosition.y + moveDistance, currentPosition.z);
				AddMovement (currentArray, movementCounter, "Up");
				movementCounter++;
			}

			if (Input.GetKeyDown (KeyCode.S)) {
				if (movementCounter == 0)
				{
					spriteCounter = 0;
				}
				else if (currentArray[movementCounter-1] != "Down" )
				{
					spriteCounter = 0;
				}
				else if(currentArray[movementCounter-1] == "Down")
				{
					if(spriteCounter == 1)
					{
						spriteCounter = 2;
						
					}
					else
					{
						spriteCounter = 1;
					}
				}
				
				switch (spriteCounter)
				{
				case 0:
					spriteRender.sprite = down[0];
					break;
				case 1:
					spriteRender.sprite = down[1];
					break;
				case 2:
					spriteRender.sprite = down[2];
					break;
				default:
					break;
				}
				previousPosition = currentPosition;
				nextPosition = new Vector3 (currentPosition.x, currentPosition.y - moveDistance, currentPosition.z);
				AddMovement (currentArray, movementCounter, "Down");
				movementCounter++;
			}
			
			if (Input.GetKeyDown (KeyCode.Space))
			{
				AddMovement (currentArray,movementCounter, "Wait");
				movementCounter++;
			}
			
			transform.position = Vector3.Lerp (transform.position, nextPosition, 0.3f);
			currentPosition = nextPosition;
			
			
		}
		
	}
	
	void AddMovement(string[] array, int index, string movement)
	{
		array [index] = movement;
	}
	
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	public string[] setArray(string array)//Decides what array within RecordingInfo to use based on the string "currentlySelectedArray"
	{
		switch (array) 
		{
		case "recording 1":
			return recorder.recording1;
		case "recording 2":
			return recorder.recording2;
		case "recording 3":
			return recorder.recording3;
		case "recording 4":
			return recorder.recording4;
		case "recording 5":
			return recorder.recording5;
		default:
			return null;
		}
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Wall")
		{
			currentPosition = previousPosition;
			nextPosition = previousPosition;
		}
	}
}
