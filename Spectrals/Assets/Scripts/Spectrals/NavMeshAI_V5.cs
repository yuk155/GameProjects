using UnityEngine;
using System.Collections;

public class NavMeshAI_V5 : MonoBehaviour {

	//If the AI cannot index the last element in the array. it breaks it. Therefore, you should always place one more waypoint than needed
	
	NavMeshAgent AIAgent;
	//creates an array with 4 different waypoints
	//for the wander
	public GameObject[] waypointArray = new GameObject[1];
	public int nextWaypoint = 0;
	public int currentWaypoint = 0;
	public float turnRadius;	//needs to be set in unity (how close it gets to the waypoints)
	// default = .05
	
	//for the idle 
	public bool startIdleTimer;
	public float idleTimer;	
	public float idleTime;		//needs to be set in unity: default = 2
	public float turnSpeed; 	// needs to be set in unity: default = .05 
	
	//to check if it should idle
	public float angleBetweenCurrAndNext;
	public float angleThreshold; //needs to be set in unity (what angle you want to allow to not pause at);
	//default = 80
	//to control speed of AI
	public float slowSpeed;		//needs to be set in unity: default = 2
	public float fastSpeed;		//needs to be set in unity: default = 3
	
	//for the escapeAI
	public float timeDif = 0;
	public bool escapeByDistance = true;
	public int escapeDistance;		//needs to be set in unity: default = 5, needs to be greater than distance to player
	public bool escapeByTime = true;
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
	public float timeDifference = 0;
	public bool startKillTimer = false;
	
	//enum to store state changes
	//enum gameState{wander = 0, follow =1};
	//0 = set destination, 1 = go to destination, 2 = follow, 3 = attack , 4 = idle,;
	public int state;
	
	
	
	void Start () 
	{
		AIAgent = GetComponent<NavMeshAgent>();
		playerPosition = GameObject.Find ("Player").transform;
		
		nextWaypoint = Random.Range (0, waypointArray.Length - 1);
		target = waypointArray[nextWaypoint].transform.position;
		AIAgent.SetDestination (target);
		
		state = 0;
		
		AIAgent.updateRotation = true;
		
		angleBetweenCurrAndNext = 0;
		
	}
	
	
	// Update is called once per frame
	void Update ()
	{	
		switch (state) {
		case(0):
			setDestination ();
			searchPlayer ();
			break;
		case (1):
			goToDestination ();
			break;
		case(2):
			foundPlayer ();
			escapeAI ();
			break;
		case (3):
			killPlayer ();
			break;
		default:
			setDestination ();
			searchPlayer ();
			break;
		}
	}
	public void searchPlayer()
	{
		//CODE TO GET THE HUMAN
		Ray lineOfSite = new Ray(transform.localPosition, transform.forward);
		Ray lineToPlayer = new Ray (transform.position, (playerPosition.position-transform.position));
		RaycastHit hit;
		Debug.DrawRay (transform.localPosition, transform.forward * 5f, Color.blue, 1f);
		Debug.DrawRay (transform.position, (Vector3.ClampMagnitude(playerPosition.position - transform.position, distanceToPlayer)), Color.green, 1f);

		if(Physics.Raycast(lineToPlayer, out hit, distanceToPlayer))
		{
			Debug.Log (hit.collider.tag);
			if(hit.collider.tag == "Player")
			{

				Quaternion fromRotation = Quaternion.LookRotation(transform.forward);
				Quaternion toRotation = Quaternion.LookRotation((playerPosition.position-transform.position));
				float angle = Quaternion.Angle(fromRotation, toRotation);
				Debug.Log (angle);
				if(angle < 60)
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
		//Debug.Log ("wandering");
		timeDifference = 0;
		timeDif = 0;
		
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
		}
		
		
		
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
	public void goToDestination()
	{
		//	Debug.Log (state);
		AIAgent.SetDestination (target);
		startIdleTimer = false;
		idleTimer = 0;
		state = 0;
		
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
	public void foundPlayer()
	{
		Debug.Log ("Following player");
		AIAgent.speed = fastSpeed;
		AIAgent.SetDestination(playerPosition.position);
		if(System.Math.Abs(transform.localPosition.x-playerPosition.position.x) < .07 && System.Math.Abs(transform.localPosition.z - playerPosition.position.z) < .07)
		{
			state = 3;
		}
	}
	public void killPlayer()
	{
		if(startKillTimer)
		{
			timeDifference += Time.deltaTime;
		}
		//method just for the timing for animations
		Debug.Log("Killing Player");
		startKillTimer = true;
		AIAgent.SetDestination(transform.position);
		if(timeDifference > killAnimationTime)
		{
			Debug.Log("Return to Wander");
			state = 1;
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
						state = 1;
					}
				}
			}
		}
		if(escapeByDistance)
		{
			if(transform.localPosition.magnitude - playerPosition.position.magnitude > escapeDistance)
			{
				Debug.Log ("escape by distance");
				state = 1;
			}
		}
	}
	

}
