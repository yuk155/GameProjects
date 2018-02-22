using UnityEngine;
using System.Collections;

public class NavMeshAI_V15 : MonoBehaviour {
	
	//If the AI cannot index the last element in the array. it breaks it. Therefore, you should always place one more waypoint than needed
	//public GameObject soundMaster;
    public AudioClip losePlayerSound;
    bool onlyLostOnce;
    // public AudioClip wanderSound;
    //public AudioClip attackPlayerSound;
    //MasterSoundScript masterSoundScript;
    Animator animator;
	//AnimatorOverrideController animatorOverride;
	NavMeshAgent AIAgent;
	//creates an array with 4 different waypoints
	//for the wander
	public GameObject[] waypointArray = new GameObject[1];
	private int nextWaypoint = 0;
	private int currentWaypoint = 0;
	private float turnRadius;	//needs to be set in unity (how close it gets to the waypoints)
	// default = .05
	
	//for the idle 
	private bool startIdleTimer;
	private float idleTimer;	
	public float idleTime;		//needs to be set in unity: default = 2
	public float turnSpeed; 	// needs to be set in unity: default = .05 
	
	//to check if it should idle
	private float angleBetweenCurrAndNext;
	public float angleThreshold; //needs to be set in unity (what angle you want to allow to not pause at);
	private bool getNextOnce = false;
	WaypointCollision waypointCollision;
	private bool shouldTurn = false;
	private bool isAngleSmall = false;
	
	//default = 80
	//to control speed of AI
	public float slowSpeed;		//needs to be set in unity: default = 2
	public float fastSpeed;		//needs to be set in unity: default = 3
	
	//for the escapeAI
	public float timeDif = 0;
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
	private float killAnimationTime;		//needs to be set in unity : default = 3
	private float timeDifference = 0;
	private bool startKillTimer = false;
	
	//enum to store state changes
	//enum gameState{wander = 0, follow =1};
	//0 = set destination, 1 = go to destination, 2 = follow, 3 = attack , 4  = go to running player, 5 = wait, 6 = go to sound objects ;
	public int state;
	
	//get the player script so that you can get the distance & is running variables 
	public GameObject Player1;
	private PlayerScript_v4 playerScript;
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

	//for finding sound
	Vector3 soundPosition;
    //public GameObject masterSoundObject; 
    SoundObjects soundObjects;
    public float soundDistance;
	
	void Start () 
	{
		//masterSoundScript = soundMaster.GetComponent<MasterSoundScript> ();
		animator = GetComponent<Animator> ();
		//RuntimeAnimatorController myController = Animator.runtimeAnimatorController ();
		//animatorOverride = new AnimatorOverrideController ();
		//animator.runtimeAnimatorController = animatorOverride;
		AIAgent = GetComponent<NavMeshAgent>();
		//playerPosition = GameObject.Find ("Player").transform;
		//actual character controller
		playerPosition = GameObject.Find ("Player").transform;
		
		playerScript = Player1.GetComponent<PlayerScript_v4> ();
		
		nextWaypoint = Random.Range (0, waypointArray.Length - 1);
		target = waypointArray[nextWaypoint].transform.position;
		currentWaypoint = nextWaypoint;
		AIAgent.SetDestination (target);
		
		state = 0;
		
		AIAgent.updateRotation = true;
		
		angleBetweenCurrAndNext = 0;
		
		previousPosition = waypointArray [0].transform.position;

        onlyLostOnce = false;

       // masterSoundObject.GetComponent<SoundObjects>().enabled = true;
        //soundObjects = masterSoundObject.GetComponent<SoundObjects>();

		animator.ResetTrigger ("SpotPlayer");
		animator.ResetTrigger ("LosePlayer");
		animator.ResetTrigger ("Attack");
	}
	
	// Update is called once per frame
	void Update ()
	{	
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("SpotPlayer"))
		{
			Debug.Log ("AnimState = Spot Player");
			AIAgent.Stop();
			Quaternion fromAngle = Quaternion.LookRotation(transform.forward);
			Quaternion toAngle = Quaternion.LookRotation(playerPosition.position - transform.position);
			transform.rotation = Quaternion.Slerp(fromAngle, toAngle, 0.1f);
		}
		else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
		{
			Debug.Log ("AnimState = Chase");
			AIAgent.Resume();
			AIAgent.speed = fastSpeed;
			AIAgent.velocity = AIAgent.desiredVelocity;
			AIAgent.acceleration = 1000f;
			//Debug.Log (AIAgent.velocity);
			//Debug.Log (AIAgent.desiredVelocity);
			//Debug.Log (AIAgent.acceleration);
		}
		else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Wander"))
		{
			Debug.Log ("AnimState = Wander");
			AIAgent.Resume();
		}
		else if(animator.GetCurrentAnimatorStateInfo(0).IsName("LostPlayer"))
		{
			Debug.Log ("AnimState = LostPlayer");
            //timeDif = 0;
            if(onlyLostOnce == false)
            {
                GetComponent<AudioSource>().PlayOneShot(losePlayerSound);
                onlyLostOnce = true;
            }
            AIAgent.Stop();
		}
		
		
		switch (state) {
		case(0):
			Debug.Log ("State = 0");
			//Debug.Log ();
			//animatorOverride["Wander"] = animatorOverride.clips[; 
			//animatorOverride.
			//randomly walking animation
			searchPlayer ();
			runningPlayer();
			//soundActivate();
			setDestination ();
			animator.ResetTrigger("SpotPlayer");
			animator.ResetTrigger("LosePlayer");
			animator.ResetTrigger("Attack");
			break;
		case (1):
			Debug.Log ("State = 1");
			//no animation here
			goToDestination ();
			break;
		case(2):
			Debug.Log ("State = 2");
			//chasing the player animation
			foundPlayer ();
			escapeAI ();
			breakLoop();
			break;
		case (3):
			//player kill animation or lose player animation
			Debug.Log ("State = 3");
			killPlayer ();
			break;
		case(4):
			//searching for sound animation
			Debug.Log ("State = 4");
			goToSound (oldPosition);
			searchPlayer();
			breakLoop();
			break;
			//5 is not in rotation right now
		case(5):
			//waiting animation
			Debug.Log ("State = 5");
			wait ();
			searchPlayer();
			break;
		case(6):
            //find physics box sounds
			Debug.Log ("State = 6");
			searchPlayer ();
			breakLoop ();
			foundSound ();
			break;
		default:
			Debug.Log ("State = Default");
			setDestination ();
			searchPlayer ();
			break;
		}
	}
	//state 0
	public void searchPlayer()
	{
		onlyLostOnce = false;
		//CODE TO GET THE HUMAN
		Ray lineOfSite = new Ray(transform.localPosition, transform.forward);
		Ray lineToPlayer = new Ray (transform.position, (playerPosition.position-transform.position));
		RaycastHit hit;
		//Debug.DrawRay (transform.localPosition, transform.forward, Color.blue, 20f);
		//Debug.DrawRay (transform.position, Vector3.ClampMagnitude((playerPosition.position - transform.position), distanceToPlayer), Color.green, 10f);
		
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
					AIAgent.Stop();
					transform.rotation = Quaternion.Slerp(fromAngle, toAngle, 1);
					AIAgent.Resume();
                    gameObject.GetComponent<AudioSource>().Play();
                    Debug.Log ("following Player");
					state = 2;
				}
			}
		}
		
	}
	public void setDestination()
	{
		//BoxCollider waypointCollider = waypointArray [currentWaypoint].GetComponent<BoxCollider> ();
		waypointCollision = waypointArray[currentWaypoint].GetComponent<WaypointCollision>();
		
		//BoxCollider waypointCollider = target.
		//Debug.Log (state);
		AIAgent.speed = slowSpeed;
		//HAS AI PATH ALONG RANDOMLY WITH WAYPOINTS
		
		float distanceX = transform.position.x - target.x;
		float distanceZ = transform.position.z - target.z;
		
		//Debug.Log ("Set Destination");		
		if (waypointCollision.isColliding == true && !getNextOnce) 
		{
			AIAgent.SetDestination(transform.position);
			
			//Debug.Log("AI AGENT STOP");
			//AIAgent.Stop();
			//doing this makes it worse
			//AIAgent.updateRotation = false;
			//AIAgent.Stop ();
			
			Debug.Log("Collision Enter");
			getNextOnce = true;
			//Debug.Log("REACH TIMER");
			startIdleTimer = true;
			
			
			//sets it to next way point
			currentWaypoint = nextWaypoint;
			nextWaypoint = Random.Range (0, waypointArray.Length - 1);
			target = waypointArray [currentWaypoint].transform.position;
			
			
			
			//	Ray behindRay = new Ray(transform.localPosition, transform.forward);
			//	angleBetweenCurrAndNext= Vector3.Angle ( behindRay.GetPoint (-5), AIAgent.path.corners[0]);
			
			Quaternion fromRotation = Quaternion.LookRotation (transform.forward);
			Quaternion toRotation = Quaternion.LookRotation (target - transform.position);
			angleBetweenCurrAndNext = Quaternion.Angle (fromRotation, toRotation);
			Debug.Log (angleBetweenCurrAndNext);
			if(angleBetweenCurrAndNext>= angleThreshold)
			{
				shouldTurn = true;
			}
			else
			{
				isAngleSmall = true;
			}
			
			
		}
		if (shouldTurn) 
		{
			Debug.Log ("TRYING TO STOP AND TURN");
			AIAgent.updateRotation = false;
			AIAgent.Stop ();
			idle ();
			AIAgent.updateRotation = true;
			AIAgent.Resume();
			
			if (idleTimer > idleTime) 
			{
				//Debug.Log("Should Turn = State1");
				state = 1;
				
				
			}
			
		}
		else if(isAngleSmall)
		{
			Debug.Log("YOUR ANGLE IS TOO SMALL");
			//Debug.Log("Else = State1");
			//Debug.Log ("Else Called");
			/*
			AIAgent.updateRotation = false;
			AIAgent.Stop ();
			Quaternion fromRotation = Quaternion.LookRotation(transform.forward);
			Quaternion toRotation = Quaternion.LookRotation(target- transform.position);
			transform.rotation = Quaternion.Slerp(fromRotation, toRotation, 1);
			AIAgent.updateRotation = true;
			AIAgent.Resume();
			*/
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
	{//
		Debug.Log ("State 1");
		timeDifference = 0;
		timeDif = 0;
		breakTimer = 0;
		movingTimer = 0;
		waitTimer = 0;
		shouldTurn = false;
		isAngleSmall = false;
        //onlyLostOnce = false;
		
		//	Debug.Log (state);
		
		bool ai = AIAgent.SetDestination (target);
		Debug.Log (ai);
		getNextOnce = false;
		Debug.Log("Should Move to Destination");
		startIdleTimer = false;
		idleTimer = 0;
		Debug.Log ("State set to 0");
		state = 0;
		
		
	}
	
	
	//state 2
	public void foundPlayer()
	{
		animator.ResetTrigger ("Attack");
		animator.ResetTrigger ("LosePlayer");
		animator.SetTrigger("SpotPlayer");
		
		Debug.Log ("Spot Trigger");
		Debug.Log ("Following player");
		AIAgent.SetDestination(playerPosition.position);
		if(Mathf.Abs(playerPosition.position.x - transform.position.x) < 4 && Mathf.Abs(playerPosition.position.z - transform.position.z) < 4)
		{
			//startKillTimer = true;
			state = 3;
		}
		else if(playerScript.isDead == true)
		{
			state = 1;
		}
	}
	public void escapeAI()
	{
		if(playerPosition.position.y > 4)
		{
			Debug.Log ("Player is too high");
			state = 1;
		}
		if(escapeByTime)
		{
			RaycastHit hit;
			if(Physics.Raycast (transform.localPosition, (playerPosition.position-transform.position), out hit, Mathf.Infinity))
			{
				if(hit.collider.tag != "Player")
				{
					if(animator.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
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
                                    animator.ResetTrigger("SpotPlayer");
                                    animator.ResetTrigger("Attack");
                                    animator.SetTrigger("LosePlayer");
                                    state = 1;
								}
							}
							else if(angleBetweenCurrAndNext < angleThreshold)
							{
                                //Debug.Log ("Else Called");
                                animator.ResetTrigger("SpotPlayer");
                                animator.ResetTrigger("Attack");
                                animator.SetTrigger("LosePlayer");
                                state = 1;
							}
							
						}
					}
					else
					{
						timeDif = 0;
					}
				}
			}
		}
		if(escapeByDistance)
		{
			if((playerPosition.position.x - transform.position.x) > escapeDistance && (playerPosition.position.z - transform.position.z) > escapeDistance)
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
                        animator.ResetTrigger("SpotPlayer");
                        animator.ResetTrigger("Attack");
                        animator.SetTrigger("LosePlayer");
                        state = 1;
					}
				}
				else if(angleBetweenCurrAndNext < angleThreshold)
				{
                    //Debug.Log ("Else Called");
                    animator.ResetTrigger("SpotPlayer");
                    animator.ResetTrigger("Attack");
                    animator.SetTrigger("LosePlayer");
                    state = 1;
				}
			}
		}
	}
	
	
	//state 3
	public void killPlayer()
	{
		timeDifference += Time.deltaTime;
		animator.ResetTrigger ("SpotPlayer");
        animator.ResetTrigger("LosePlayer");
		animator.SetTrigger ("Attack");
		//method just for the timing for animations
		//Debug.Log("Killing Player");
		//startKillTimer = false;
		AIAgent.SetDestination(transform.position);
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Wander") == true) 
		{
			state = 1;
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
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chase") == true || state == 4) 
		{
			if (breakTimer > 1) {
				Debug.Log ("Is now Unstuck");
				startIdleTimer = true;
			
				Quaternion fromAngle = Quaternion.LookRotation (transform.forward);
				Quaternion toAngle = Quaternion.LookRotation (target - transform.position);
				angleBetweenCurrAndNext = Quaternion.Angle (fromAngle, toAngle);
			
				if (angleBetweenCurrAndNext > angleThreshold) {
					AIAgent.updateRotation = false;
					AIAgent.Stop ();
					idle ();
					AIAgent.updateRotation = true;
					AIAgent.Resume ();
				
					if (idleTimer > idleTime) {
                        animator.ResetTrigger("LosePlayer");
                        animator.ResetTrigger("Attack");
                        animator.SetTrigger("SpotPlayer");
						state = 2;
					}
				} else if (angleBetweenCurrAndNext < angleThreshold) {
                    animator.ResetTrigger("LosePlayer");
                    animator.ResetTrigger("Attack");
                    animator.SetTrigger("SpotPlayer");
                    state = 2;
				}
			}
		
			movingTimer += Time.deltaTime;
		
			//gets the position of the spectral every 1 second
			if (movingTimer > 1) {
				previousPosition = transform.position;
				//Debug.Log ("Prev" + previousPosition);
				movingTimer = 0;
			}
		}
		
	}
	public void runningPlayer()
	{
		//NEED TO MAKE CURRENTSPEED PRIVATE IN THE PLAYERSCRIPT
		
		if(Input.GetKeyDown(KeyCode.LeftShift))
		{
			if(Mathf.Abs(playerPosition.position.x - transform.position.x) < playerDistance && Mathf.Abs(playerPosition.position.z - transform.position.z) < playerDistance)
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

	//state 6 - sound activated target
    
	public void soundActivate()
	{
		Debug.Log ("Sound activated function");
        //Vector3 baseVector = new Vector3 (0, 0, 0);
        if (soundObjects == null)
            Debug.Log("soundObjects == null");
		if (soundObjects.soundObjectIndex != -1) 
		{
            Debug.Log("Sound Activated");
            soundPosition = soundObjects.getSoundObject().transform.position;
			if((playerPosition.position.x - transform.position.x) < soundDistance && (playerPosition.position.z - transform.position.z) <soundDistance)
            {
                AIAgent.SetDestination(soundPosition);
                state = 6;
            }

		}
	}
	public void foundSound()
	{
		Debug.Log ("Found Sound Function");
		float xDistance = transform.position.x - soundPosition.x;
		float zDistance = transform.position.z - soundPosition.z;


		if(xDistance<.1 && zDistance< .1)
		{
			//going back to the previous waypoint
			Debug.Log("Return to Wander");
			state = 1;
		}
	}
}
