using UnityEngine;
using System.Collections;

public class NavMeshAI_V8 : MonoBehaviour {

	
	//If the AI cannot index the last element in the array. it breaks it. Therefore, you should always place one more waypoint than needed
	
	NavMeshAgent AIAgent;
	//creates an array with 4 different waypoints
	//for the wander
	public GameObject[] waypointArray = new GameObject[1];
	private int nextWaypoint = 0;
	private int currentWaypoint = 0;
	public float turnRadius;	//needs to be set in unity (how close it gets to the waypoints)
	// default = .05
	
	//for the idle 
	private bool startIdleTimer;
	private float idleTimer;	
	public float idleTime;		//needs to be set in unity: default = 2
	public float turnSpeed; 	// needs to be set in unity: default = .05 
	
	//to check if it should idle
	private float angleBetweenCurrAndNext;
	public float angleThreshold; //needs to be set in unity (what angle you want to allow to not pause at);
	public bool shouldTurn = false;
	//default = 80
	//to control speed of AI
	public float slowSpeed;		//needs to be set in unity: default = 2
	public float fastSpeed;		//needs to be set in unity: default = 3
	
	//for the escapeAI
	float timeDif = 0;
	public bool escapeByDistance = true; //default = true
	public int escapeDistance;		//needs to be set in unity: default = 5, needs to be greater than distance to player
	public bool escapeByTime = true; //defaut = true;
	public int escapeTime;			//needs to be set in unity: default = 3
	
	//for find player
	public float distanceToPlayer;		//needs to be set in unity (how far it can see): default = 4
	
	//for wander
	Vector3 target;
	Vector3 lastTarget;
	int num = 0;
	
	//find player stuff
	//public bool seenPlayer = false;
	public Transform playerPosition;
	
	//for kill player 
	public float killAnimationTime;		//needs to be set in unity : default = 3
	private float timeDifference = 0;
	private bool startKillTimer = false;
	
	//enum to store state changes
	//enum gameState{wander = 0, follow =1};
	//0 = set destination, 1 = go to destination, 2 = follow, 3 = attack , 4  = go to sound, 5 = wait ;
	public int state;
	
	//get the player script so that you can get the distance & is running variables 
	public GameObject Player1;
	private PlayerScript playerScript;
	//public variable for the running method
	public int playerDistance; //default set to 3;
	private Vector3 oldPosition;
	//public float soundTimer = 0;
	
	//break timer for the second and fourth state
	float breakTimer = 0;
	private Vector3 previousPosition;
	float movingTimer = 0;

	//for the wait 
	bool isWaitTimer = false;
	float waitTimer;
	public float waitTime;
	
	
	void Start () 
	{
		AIAgent = GetComponent<NavMeshAgent>();
		//playerPosition = GameObject.Find ("Player").transform;
		//actual character controller
		playerPosition = GameObject.Find ("Player").transform;
		
		playerScript = Player1.GetComponent<PlayerScript> ();
		
		nextWaypoint = Random.Range (0, waypointArray.Length - 1);
		target = waypointArray[nextWaypoint].transform.position;
		AIAgent.SetDestination (target);
		
		state = 0;
		
		AIAgent.updateRotation = true;
		
		angleBetweenCurrAndNext = 0;
		
		previousPosition = waypointArray [0].transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{	
		
		switch (state) {
		case(0):
			//randomly walking animation
			setDestination ();
			searchPlayer ();
			runningPlayer();
			break;
		case (1):
			//no animation here
			goToDestination ();
			break;
		case(2):
			//chasing the player animation
			foundPlayer ();
			escapeAI ();
			breakLoop();
			break;
		case (3):
			//player kill animation
			killPlayer ();
			break;
		case(4):
			//searching for sound animation
			goToSound (oldPosition);
			searchPlayer();
			breakLoop();
			break;
		case(5):
			//waiting animation
			wait ();
			searchPlayer();
			break;
		default:
			setDestination ();
			searchPlayer ();
			break;
		}
	}
	//state 0
	public void searchPlayer()
	{
		//CODE TO GET THE HUMAN
		Ray lineOfSite = new Ray(transform.localPosition, transform.forward);
		Ray lineToPlayer = new Ray (transform.position, (playerPosition.position-transform.position));
		RaycastHit hit;
		Debug.DrawRay (transform.localPosition, transform.forward, Color.blue, 20f);
		Debug.DrawRay (transform.position, Vector3.ClampMagnitude((playerPosition.position - transform.position), distanceToPlayer), Color.green, 10f);
		
		if(Physics.Raycast(lineToPlayer, out hit, distanceToPlayer))
		{
			//	Debug.Log (hit.collider.tag);
			if(hit.collider.tag == "Player")
			{
				
				Quaternion fromAngle = Quaternion.LookRotation(transform.forward);
				Quaternion toAngle = Quaternion.LookRotation(playerPosition.position - transform.position);
				float angle =  Quaternion.Angle(fromAngle, toAngle);
				//Debug.Log(angle);
				if(angle < 60 )
				{
					Debug.Log ("following Player");
					state = 2;
				}
			}
		}
		
	}
	public void setDestination()
	{
		//Debug.Log (state);
		AIAgent.speed = slowSpeed;
		//HAS AI PATH ALONG RANDOMLY WITH WAYPOINTS
		
		float distanceX = transform.position.x - target.x;
		float distanceZ = transform.position.z - target.z;
		
		if (distanceX < turnRadius && distanceZ < turnRadius) 
		{
			
			//Debug.Log("REACH TIMER");
			startIdleTimer = true;
			
			
			//sets it to next way point
			currentWaypoint = nextWaypoint;
			nextWaypoint = Random.Range (0, waypointArray.Length-1);
			target = waypointArray [currentWaypoint].transform.position;
			
			
			
			//	Ray behindRay = new Ray(transform.localPosition, transform.forward);
			//	angleBetweenCurrAndNext= Vector3.Angle ( behindRay.GetPoint (-5), AIAgent.path.corners[0]);
			
			Quaternion fromRotation = Quaternion.LookRotation(transform.forward);
			Quaternion toRotation = Quaternion.LookRotation(target- transform.position);
			angleBetweenCurrAndNext = Quaternion.Angle (fromRotation, toRotation);

			if(angleBetweenCurrAndNext> angleThreshold)
			{
				shouldTurn = true;
			}

			}
		if (shouldTurn) 
		{
			//Debug.Log ("TRYING TO STOP AND TURN");
			AIAgent.updateRotation = false;
			AIAgent.Stop ();
			idle ();
			AIAgent.updateRotation = true;
			AIAgent.Resume();
			
			if (idleTimer > idleTime) 
			{
				//Debug.Log("Should Turn = State1");
				state = 1;
				shouldTurn = false;
			}

		}
		else if(angleBetweenCurrAndNext<angleThreshold)
		{
			//Debug.Log("Else = State1");
			//Debug.Log ("Else Called");
			shouldTurn = false;
			state = 1;
		}



	}
	public void idle()
	{
		if(startIdleTimer)
		{
			idleTimer += Time.deltaTime;
		}
		
		//AIAgent.SetDestination (transform.position);
		
		
		Quaternion fromRotation = Quaternion.LookRotation(transform.forward);
		Quaternion toRotation = Quaternion.LookRotation(target- transform.position);
		transform.rotation = Quaternion.Slerp(fromRotation, toRotation, turnSpeed);
		
	}
	//state 1
	public void goToDestination()
	{
		timeDifference = 0;
		timeDif = 0;
		breakTimer = 0;
		movingTimer = 0;
		waitTimer = 0;

		//	Debug.Log (state);
		AIAgent.SetDestination (target);
		startIdleTimer = false;
		idleTimer = 0;
		state = 0;
		
	}
	
	
	//state 2
	public void foundPlayer()
	{
		Debug.Log ("Following player");
		AIAgent.speed = fastSpeed;
		AIAgent.SetDestination(playerPosition.position);
		if(System.Math.Abs(transform.position.x-playerPosition.position.x) < 2 && System.Math.Abs(transform.position.z - playerPosition.position.z) < 2)
		{
			//startKillTimer = true;
			state = 3;
		}
	}
	public void escapeAI()
	{
		if(escapeByTime)
		{
			RaycastHit hit;
			if(Physics.Raycast (transform.localPosition, (playerPosition.position-transform.position), out hit, Mathf.Infinity))
			{
				if(hit.collider.tag != "Player")
				{
					timeDif += Time.deltaTime;
					if(timeDif > escapeTime)
					{
						Debug.Log ("escape by time");
						startIdleTimer = true;
						
						Quaternion fromAngle = Quaternion.LookRotation(transform.forward);
						Quaternion toAngle = Quaternion.LookRotation(target - transform.position);
						angleBetweenCurrAndNext = Quaternion.Angle(fromAngle, toAngle);
						
						if (angleBetweenCurrAndNext > angleThreshold) 
						{
							AIAgent.updateRotation = false;
							AIAgent.Stop ();
							idle ();
							AIAgent.updateRotation = true;
							AIAgent.Resume();
							
							if (idleTimer > idleTime) 
							{
								state = 1;
							}
						}
						else if(angleBetweenCurrAndNext < angleThreshold)
						{
							//Debug.Log ("Else Called");
							state = 1;
						}
						
					}
				}
			}
		}
		if(escapeByDistance)
		{
			if(System.Math.Abs(transform.position.x-playerPosition.position.x) > escapeDistance && System.Math.Abs(transform.position.z - playerPosition.position.z) > escapeDistance)
			{
				Debug.Log ("escape by distance");
				startIdleTimer = true;
				
				Quaternion fromAngle = Quaternion.LookRotation(transform.forward);
				Quaternion toAngle = Quaternion.LookRotation(target - transform.position);
				angleBetweenCurrAndNext = Quaternion.Angle(fromAngle, toAngle);
				if (angleBetweenCurrAndNext > angleThreshold) 
				{
					AIAgent.updateRotation = false;
					AIAgent.Stop ();
					idle ();
					AIAgent.updateRotation = true;
					AIAgent.Resume();
					
					if (idleTimer > idleTime) 
					{
						state = 1;
					}
				}
				else if(angleBetweenCurrAndNext < angleThreshold)
				{
					//Debug.Log ("Else Called");
					state = 1;
				}
			}
		}
	}
	
	
	//state 3
	public void killPlayer()
	{
		timeDifference += Time.deltaTime;
		
		//method just for the timing for animations
		Debug.Log("Killing Player");
		//startKillTimer = false;
		AIAgent.SetDestination(transform.position);
		if(timeDifference > killAnimationTime)
		{
			Debug.Log("Wait after killing");
			isWaitTimer = true;
			state = 5;
		}
	}


	
	//state 4
	public void breakLoop()
	{
		//if its stuck it checks
		if(transform.position == previousPosition)
		{
			
			breakTimer += Time.deltaTime;
		}
		
		//tells it to go back to what it was doing after 5 seconds
		if(breakTimer > 5)
		{
			Debug.Log ("Is now Unstuck");
			startIdleTimer = true;
			
			Quaternion fromAngle = Quaternion.LookRotation(transform.forward);
			Quaternion toAngle = Quaternion.LookRotation(target - transform.position);
			angleBetweenCurrAndNext = Quaternion.Angle(fromAngle, toAngle);
			
			if (angleBetweenCurrAndNext > angleThreshold) 
			{
				AIAgent.updateRotation = false;
				AIAgent.Stop ();
				idle ();
				AIAgent.updateRotation = true;
				AIAgent.Resume();
				
				if (idleTimer > idleTime) 
				{
					state = 1;
				}
			}
			else if(angleBetweenCurrAndNext < angleThreshold)
			{
				state = 1;
			}
		}
		
		movingTimer += Time.deltaTime;
		
		//gets the position of the spectral every 1 second
		if(movingTimer > 5)
		{
			previousPosition = transform.position;
			//Debug.Log ("Prev" + previousPosition);
			movingTimer = 0;
		}
		
	}
	public void runningPlayer()
	{
		//NEED TO MAKE CURRENTSPEED PRIVATE IN THE PLAYERSCRIPT
		
		if(playerScript.currentSpeed == playerScript.sprintSpeed)
		{
			if(System.Math.Abs(transform.position.x-playerPosition.position.x) < playerDistance && System.Math.Abs(transform.position.z - playerPosition.position.z) < playerDistance)
			{
				oldPosition = playerPosition.position;
				Debug.Log ("Find by sound");
				if(state == 0)
				{
					state = 4;
				}
			}
		}
	}
	public void goToSound(Vector3 position)
	{
		AIAgent.SetDestination (position);
		float xDistance = Mathf.Abs(transform.position.x - position.x);
		float zDistance = Mathf.Abs(transform.position.z - position.z);
		
		breakTimer += Time.deltaTime;
		
		if(xDistance<.1 && zDistance< .1)
		{
			//going back to the previous waypoint
			Debug.Log("Return to Wander");
			state = 1;
		}
	}
	//state 5
	public void wait()
	{
		Debug.Log ("Wait: State=5");
		//stops the AI after it finds you to wait and look around for a bit 
		AIAgent.SetDestination (transform.position);
		if(isWaitTimer)
		{
			waitTimer += Time.deltaTime;
		}
		if(waitTimer>waitTime)
		{
			isWaitTimer = false;
			state  = 1;
		}
	}
}
