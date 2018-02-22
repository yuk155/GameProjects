using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FloorPlan : MonoBehaviour {

	public int width;
	public int height;

	public int numberOfWalls;

	//Lists conversion
	//First dimension is the number of rows, second dimension is the number of columns, third dimension is the coordinates
	public List<List<List<int>>> coordinateMatrixList = new List<List<List<int>>>();
	public List<List<List<int>>> elementIdMatrixList = new List<List<List<int>>>();

	//public int[,,] coordinateMatrix = new int[10,14,2]; // 10 is the number of rows, 14 is the number of columns , 2 is how many coordinate are at each point
	//public int[,,] elementIdMatrix = new int[10, 14, 1]; // 1 is the number that will identify the part (0 = floor, 1 = Horizontal wall, 2 = Vertical Wall,
	//3 = Horizontal door, 4 = Vertical door, etc.)

	public GameObject zero;
	public GameObject one; //horizontal wall
	public GameObject two; //vertical wall
	public GameObject three; //Horizontal door
	public GameObject four; //Vertical door
	public GameObject five; //Button

	public GameObject floorTile;
	public GameObject verticalWall;
	public GameObject horizontalWall;

	//ONLY UNCOMMENT FOR FINAL BUILD
	public Difficulty_Setting difficulty;

	//bool to take care of failed room generations
	bool isFailed = false;

	// Use this for initialization
	void Awake() {

		difficulty = GameObject.Find ("DifficultySetting").GetComponent<Difficulty_Setting>();
		numberOfWalls = difficulty.difficultySetting;
		if(numberOfWalls <= 3)
		{
			height = 9;
			width = 14;
		}
		else
		{
			height = 13;
			width = 20;
		}
		GenerateStartCoordinates ();
		for (int i =0; i < numberOfWalls; i++) 
		{
			GenerateWall();
		}
		//InstantiateObjects ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	void GenerateStartCoordinates()
	{
		for (int row = 0; row < coordinateMatrix.GetLength(0); row ++) 
		{
			for (int col = 0; col < coordinateMatrix.GetLength(1); col ++)
			{
				coordinateMatrix[row,col,0] = col * 3;
				coordinateMatrix[row,col,1] = row * 3;
				elementIdMatrix[row,col,0] = 0;
			}
		}

	}*/

	void GenerateStartCoordinates()
	{
		for (int row = 0; row < height; row ++) 
		{
			List<List<int>> column  = new List<List<int>>(width);
			List<List<int>> columnId = new List<List<int>>(width);
			for (int col = 0; col< width; col++)
			{
				List<int> coordinate = new List<int>(2);
				List<int> coordinateId = new List<int>(1);

				coordinate.Add(col * 3 );
				coordinate.Add(row * 3 );
				coordinateId.Add(0);

				columnId.Add(coordinateId);
				column.Add(coordinate);
			}
			coordinateMatrixList.Add(column);
			elementIdMatrixList.Add(columnId);
		}
	}

	void InstantiateObjects()
	{
		//Go through all 140 multi-dimensional array indices
		for (int row = 0; row < elementIdMatrixList.Count; row ++) 
		{
			for (int col = 0; col < coordinateMatrixList[row].Count; col ++)
			{
				//Create placeholder Vector3 coordinate
				Vector3 globalCoordinate = Vector3.zero;
				globalCoordinate.x = coordinateMatrixList[row][col][0]; // Change x value to represent coordinateMatrix's 0 index
				globalCoordinate.y = coordinateMatrixList[row][col][1];

				if(elementIdMatrixList[row][col][0] == 0)
				{
					Instantiate(zero, globalCoordinate, Quaternion.identity);
				}
				else if (elementIdMatrixList[row][col][0] == 1)
				{
					Instantiate(one, globalCoordinate, Quaternion.identity);
				}

				else if (elementIdMatrixList[row][col][0] == 2)
				{
					Instantiate(two,globalCoordinate,Quaternion.identity);
				}
				/*
				else if (elementIdMatrixList[row][col][0] == 3)
				{
					Instantiate(three, globalCoordinate,Quaternion.identity);
				}

				else if (elementIdMatrixList[row][col][0] == 4)
				{
					Instantiate(four, globalCoordinate,Quaternion.identity);
				}*/

				else
				{
					continue;
				}


				/*else if (elementIdMatrix[row,col,0] == 5)
				{
					Instantiate(five, globalCoordinate,Quaternion.identity);
				}*/
			}
		}

		for (int outsideUp = 0; outsideUp < coordinateMatrixList.Count; outsideUp ++) 
		{
			Vector2 outsideCoordinate = Vector2.zero;
			outsideCoordinate.x = -3;

			outsideCoordinate.y = outsideUp * 3;
			Instantiate(two, outsideCoordinate, Quaternion.identity);

			outsideCoordinate.x = ((coordinateMatrixList[0].Count) * 3);
			Instantiate(two, outsideCoordinate, Quaternion.identity);
		}

		for (int outsideTop = 0; outsideTop < coordinateMatrixList[0].Count; outsideTop ++) 
		{
			Vector2 outsideCoordinate = Vector2.zero;
			outsideCoordinate.x = outsideTop * 3;
			outsideCoordinate.y = -3;
			Instantiate (one, outsideCoordinate, Quaternion.identity);

			outsideCoordinate.y = (coordinateMatrixList.Count * 3);
			Instantiate (one, outsideCoordinate, Quaternion.identity);
		}
	}
	

	int PickStartingPoint(int type, int side) //Used to make sure that walls aren't too close together or overlapping
	{
		bool isValid = false;
		int totalLoops = 1;
		if (type == 0) 
		{ //Horizontal Wall
			if(side == 0)//Start from the left
			{
				while (isValid == false) 
				{
					if (totalLoops > 40)
					{
						Debug.Log("Room Generation Failed");
						isFailed = true;
						break;
					}
					int startingPoint = Random.Range (2, (elementIdMatrixList.Count - 2));
					if (elementIdMatrixList [startingPoint][0][0] == 1 || elementIdMatrixList[startingPoint + 1][0][0] == 1 ||
						elementIdMatrixList[startingPoint - 1][0][0] == 1)
					{
						totalLoops ++;
						continue;
					}
					else 
					{
						isValid = true;

						return startingPoint;
					}
				}
			}

			else if(side == 1) //Start from the right
			{
				while (isValid == false) 
				{
					if (totalLoops > 40)
					{
						Debug.Log("Room Generation Failed");
						isFailed = true;
						break;
					}
					int startingPoint = Random.Range (2, (elementIdMatrixList.Count - 2));
					if (elementIdMatrixList [startingPoint][elementIdMatrixList[0].Count - 1][0] == 1 || 
					    elementIdMatrixList [startingPoint + 1][elementIdMatrixList[0].Count - 1][0] == 1 ||
					    elementIdMatrixList [startingPoint - 1][elementIdMatrixList[0].Count - 1][0] == 1)
					{
						totalLoops++;
						continue;
					}
					else 
					{
						isValid = true;

						return startingPoint;
					}
				}
			}
			else
			{
				Debug.Log("Invalid side Type");
				return 0;
			}
		} 
		else if (type == 1) //Vertical Wall
		{
			if (side == 0)//start from the bottom
			{
				while(isValid == false)
				{
					if (totalLoops > 40)
					{
						Debug.Log("Room Generation Failed");
						isFailed = true;
						break;

					}

					int startingPoint = Random.Range(2, (elementIdMatrixList[0].Count - 2));
					if (elementIdMatrixList[0][startingPoint][0] == 2 || elementIdMatrixList[0][startingPoint + 1][0] == 2 ||
					    elementIdMatrixList [0][startingPoint - 1][0] == 2)
					{
						totalLoops ++;
						continue;
					}
					else
					{
						isValid = true;

						return startingPoint;
					}
				}
			}
			else if (side == 1) //start from the top
			{
				while(isValid == false)
				{
					if (totalLoops > 40)
					{
						Debug.Log("Room Generation Failed");
						isFailed = true;
						break;

					}

					int startingPoint = Random.Range(2, (elementIdMatrixList[0].Count - 2));
					if(elementIdMatrixList[elementIdMatrixList.Count - 1][startingPoint][0] == 2 ||
					   elementIdMatrixList[elementIdMatrixList.Count - 1][startingPoint + 1][0] == 2 ||
					   elementIdMatrixList[elementIdMatrixList.Count - 1][startingPoint - 1][0] == 2)
					{
						totalLoops ++;
						continue;
					}
					else
					{
						isValid = true;

						return startingPoint;
					}
				}
			}
			else
			{
				Debug.Log("Invalid side Type");
				return 0;
			}
		}
		else
		{
			Debug.Log("Invalid wall type");
			return 0;
		}
		Debug.Log ("Invalid Input");
		return 0;

	}

	void GenerateWall()
	{
		//Decide to draw a vertical wall or horizontal wall
		int wallType = Random.Range(0, 2);
		int startingSide = Random.Range(0,2);
		if (wallType == 0) // Horizontal wall
		{
			int startingPoint = PickStartingPoint(wallType,startingSide); //Decide which row to create the wall from
			if(isFailed)
			{
				startingPoint = PickStartingPoint(wallType,startingSide);
				isFailed = false;

			}

			if(startingSide == 0) //Start from the Left
			{
				for(int col = 0; col < elementIdMatrixList[0].Count; col ++) //Iterate through each column in the row
				{
					if(elementIdMatrixList[startingPoint][col][0] != 1 && elementIdMatrixList[startingPoint][col][0] != 2)
					{
						elementIdMatrixList[startingPoint][col][0] = 1;
					}
					else
					{
						break;
					}
				}
			}

			else if(startingSide == 1) //Start from the right
			{
				for (int col = (elementIdMatrixList[0].Count - 1); col >= 0; col --)
				{
					if(elementIdMatrixList[startingPoint][col][0] != 1 && elementIdMatrixList[startingPoint][col][0] != 2)
					{
						elementIdMatrixList[startingPoint][col][0] = 1;
					}
					else
					{
						break;
					}
				}
			}
		}

		//-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-

		else if(wallType == 1) // Vertical wall
		{
			int startingPoint = PickStartingPoint(wallType,startingSide); //Decide which column to create the wall from
			if(startingSide == 0) //start from the bottom
			{
				for(int row = 0; row < elementIdMatrixList.Count; row ++)
				{
					if(elementIdMatrixList[row][startingPoint][0]!= 1 && elementIdMatrixList[row][startingPoint][0] != 2)
					{
						elementIdMatrixList[row][startingPoint][0] = 2;
					}
					else
					{
						break;
					}
				}
			}
			else if(startingSide == 1) //start from top
			{
				for (int row = (elementIdMatrixList.Count - 1); row >= 0; row --)
				{
					if(elementIdMatrixList[row][startingPoint][0]!= 1 && elementIdMatrixList[row][startingPoint][0] != 2)
					{
						elementIdMatrixList[row][startingPoint][0] = 2;
					}
					else
					{
						break;
					}
				}
			}
		}
	}


}
