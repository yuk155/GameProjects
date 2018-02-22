using UnityEngine;
using System.Collections;

public class navMeshAI_V3 : MonoBehaviour {
	

	NavMeshAgent AIAgent;
	//creates an array with 4 different waypoints
	//for the wander
	GameObject[] waypointArray = new GameObject[4];
	public GameObject waypoint1;
	public GameObject waypoint2;
	public GameObject waypoint3;
	public GameObject waypoint4;
	public int nextWaypoint = 0;
	public int currentWaypoint = 0;
	
	
	//to control speed of AI
	public float slowSpeed;
	public float fastSpeed;
	
	//for the escapeAI
	public float timeDif = 0;
	public bool escapeByDistance = true;
	public int escapeDistance;
	public bool escapeByTime = true;
	public int escapeTime;
	
	//for find player
	public float distanceToPlayer;
	
	
	//for wander
	Vector3 target;
	Vector3 lastTarget;
	int num = 0;
	
	//find player stuff
	//public bool seenPlayer = false;
	public Transform playerPosition;

	//for kill player 
	public float killAnimationTime;
	public float timeDifference = 0;
	//public bool isDead = false;

	//enum to store state changes
	//enum gameState{wander = 0, follow =1};
	//0 = wander, 1 = follow,
	int state = 0;
	
	
	
	void Start () 
	{
		AIAgent = GetComponent<NavMeshAgent>();
		playerPosition = GameObject.Find ("Player").transform;
		waypointArray [0] = waypoint1;
		waypointArray [1] = waypoint2;
		waypointArray [2] = waypoint3;
		waypointArray [3] = waypoint4;
		
		nextWaypoint = Random.Range (0, waypointArray.Length - 1);
		target = waypointArray[nextWaypoint].transform.position;
		AIAgent.SetDestination (target);
		
		//lastTarget = waypointArray [num].transform.position;
		//AIAgent.SetDestination (lastTarget);
		
	}
	
	// Update is called once per frame
	void Update ()
	{	
		//if(isDead == false)
		//{
			searchPlayer ();
//		}
		switch (state)
		{
		case(0):
			wander ();

			break;
		case(1):
			foundPlayer();
			escapeAI();
			killPlayer();
			break;
		default:
			wander ();
			searchPlayer ();
			break;
		}
		
		
		
	}
	public void wander()
	{
		Debug.Log ("wandering");
		timeDifference = 0;
		timeDif = 0;
		AIAgent.speed = slowSpeed;
		//HAS AI PATH ALONG RANDOMLY WITH WAYPOINTS
		
		float distanceX = transform.position.x - target.x;
		float distanceZ = transform.position.z - target.z;
		if (distanceX < .05 && distanceZ < .05) 
		{
			
			//Paths to waypoint ^ saves last target variable
			
			/*
			int randNum = Random.Range(0, waypointArray.Length-1);
			if(target == lastTarget)
			{
				randNum = Random.Range (0, waypointArray.Length -1);
			}
			target = waypointArray[randNum].transform.position;

			AIAgent.SetDestination(target);
			lastTarget = target;
			*/
			
			//goes to waypoint
			
			target = waypointArray [currentWaypoint].transform.position;
			AIAgent.SetDestination (target);
			
			//sets it to next way point
			currentWaypoint = nextWaypoint;
			nextWaypoint = Random.Range (0, waypointArray.Length);
			if (nextWaypoint == currentWaypoint) 
			{
				nextWaypoint = Random.Range (0, waypointArray.Length - 1);
			}
			
			
			//rotate the AI
			//transform.LookAt (target);
			Quaternion fromRotation = Quaternion.LookRotation(Vector3.forward);
			Quaternion toRotation = Quaternion.LookRotation(target);
			Quaternion.Slerp(fromRotation, toRotation, .1f);
		}
	}
	
	public void foundPlayer()
	{
		Debug.Log ("Following player");
		AIAgent.speed = fastSpeed;
		AIAgent.SetDestination(playerPosition.position);
	}
	public void killPlayer()
	{
		//method just for the timing for animations
		if(System.Math.Abs(transform.localPosition.x-playerPosition.position.x) < .07 && System.Math.Abs(transform.localPosition.z - playerPosition.position.z) < .07)
		{
			Debug.Log("Killing Player");
			timeDifference += Time.deltaTime;
			AIAgent.SetDestination(playerPosition.position);
			if(timeDifference > killAnimationTime)
			{
				//state = 0;
				Debug.Log("Return to Wander");
				AIAgent.SetDestination (target);
				//isDead = true;
			}

		}
	}
	public void escapeAI()
	{
		if(escapeByTime)
		{
			RaycastHit hit;
			if(Physics.Raycast (transform.localPosition, playerPosition.position, out hit, 10f))
			{
				if(hit.collider.tag != "Player")
				{
					timeDif += Time.deltaTime;
					if(timeDif > escapeTime)
					{
						Debug.Log ("Return to Wander");
						state = 0;
						AIAgent.SetDestination (target);
					}
				}
			}
		}
		if(escapeByDistance)
		{
			if(transform.localPosition.magnitude - playerPosition.position.magnitude > escapeDistance)
			{
				Debug.Log ("Return to Wander");
				state = 0;
				AIAgent.SetDestination (target);
			}
		}
	}
	public void searchPlayer()
	{
		//CODE TO GET THE HUMAN
		Ray lineOfSite = new Ray(transform.localPosition, Vector3.forward);
		Ray leftRay = new Ray (transform.localPosition, Vector3.left);
		Ray rightRay = new Ray (transform.localPosition, Vector3.right);
		Debug.DrawRay (transform.localPosition, Vector3.forward, Color.blue, 2000f);
		
		Collider[] colliderArray = Physics.OverlapSphere (transform.position, distanceToPlayer);
		
		for (int i = 0; i< colliderArray.Length;  i++)
		{
			if(colliderArray[i].tag == "Player")
			{
				if(Vector3.Angle(lineOfSite.GetPoint(distanceToPlayer), playerPosition.position) < 90)
				{
					Debug.Log ("following Player");
					state = 1;
					
				}
				
			}
		}
	}
	

}
