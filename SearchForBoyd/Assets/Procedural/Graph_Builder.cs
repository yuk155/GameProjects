using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph_Builder : MonoBehaviour {

	public int numberOfRooms;
	public int numberOfDoors;
	public int numberOfButtons;

	public GameObject floorPlanner;
	public FloorPlan floorPlan;

	//public GameObject[] roomTesters = new GameObject[7];

	public List<List<int>> roomsList = new List<List<int>>();

	//public int [,] rooms = new int[7,4]; //6 is the number of rooms possible, 4 refers to the pieces of data for each room
	//The data is orderd {height, width, x coordinate of bottomLeftCorner, y coordinate of bottomLeftCorner}
	int increment = 1; //SUPER IMPORTANT, DO NOT DELETE!

	//public int[,,] borderPieces = new int[7,50,2]; //first value is number of rooms, second value is the number of possible border pieces,
	//third value is the coordinates in the elementIdMatrix

	public List<List<List<int>>> borderPiecesList = new List<List<List<int>>>();

	//The first value of rooms, borderPieces, and sharedPieces must be the same

	//public int[,,,] sharedPieces = new int[7,7, 50, 2]; //first value is the first connected room, second value is the second room connected to it,
	//third value is the number of possible connecting pieces, forth value is the x and y coordinates of those pieces

	public List<List<List<List<int>>>> sharedPiecesList = new List<List<List<List<int>>>>();

	public List<List<int>> roomConnections = new List<List<int>> ();

	//public int[,] sharedRooms = new int[10, 2]; //first value is the number of the connection, the second array holds two pieces of information:
	//the rooms that are connected

	public List<int> roomOrder = new List<int> (); //Room order used to build the room connections
	public List<int> finalRoomOrder = new List<int> (); //Actual room order to be used for button and door placement

	public List<List<int>> connectorsList = new List<List<int>> ();

	//list for new procedural generation
	//first is the actual room, second index is the number of the room that the button corresponds to 

	List<GameObject> doorList = new List<GameObject> ();
	List<GameObject> buttonList = new List<GameObject> ();

	public List<List<int>> levelList = new List<List<int>>(); //The level within the order tree that each room is one. First index is the level itself,
	//the second list is the rooms that occupy that level

	public List<int> buttonRoomsList = new List<int> ();

	public int buttonRoomIncrement = 0;
	public int calcRoomOrderIncrement = 0; //Keep track of what recursion of CalcRoomOrder we are on

	public int appendedRoomCount = 0;

	// Use this for initialization
	void Start () {
		floorPlan = floorPlanner.GetComponent<FloorPlan> ();
		numberOfRooms = floorPlan.numberOfWalls + 1;

		for (int i = 0; i < numberOfRooms; i ++)
		{
			List<int> roomData = new List<int>();
			for (int j = 0; j < 4; j ++)
			{
				roomData.Add(0);
			}
			roomsList.Add(roomData);
		}

		CalcCurrentRoom(0,0,0);

		/*
		for (int roomNumber = 0; roomNumber < roomsList.Count; roomNumber ++)
		{
			Debug.Log("Current Room: " + roomNumber);
			for (int k = 0; k < 4; k ++)
			{
				Debug.Log(roomsList[roomNumber][k]);
			}
		}*/

		FindBorders ();
		FindSharedEdges ();

		BuildConnections ();

		/*for(int k = 0; k < roomsList.Count; k ++)
		{
			Debug.Log("Current Room: " + k);
			{
				for(int j = 0; j < roomConnections[k].Count; j ++)
				{
					Debug.Log(roomConnections[k][j]);
				}
			}
		}*/

		StackRooms(FindLastRoom());

		CalcRoomOrder (0);


		Debug.Log ("Final Room Order: ");
		for(int i = 0; i < finalRoomOrder.Count; i ++)
		{
			Debug.Log(finalRoomOrder[i]);
		}
		Debug.Log ("Appended room connections: ");


		CalcAppendedRooms ();
		CalcMainConnections ();

		for(int k = 0; k <connectorsList.Count; k ++)
		{
			Debug.Log(connectorsList[k][0] + "," + connectorsList[k][1]);
		}

		BuildLevels ();
		FixLevels ();
		PlaceDoors ();

		PlaceButtons ();

		for(int i = 0; i < levelList.Count; i ++)
		{
			Debug.Log("Current Level: " + i);
			for (int j = 0; j < levelList[i].Count; j ++)
			{
				Debug.Log(levelList[i][j]);
			}
		}

		Debug.Log ("Button Rooms List: " + buttonRoomsList.Count);

		floorPlan.SendMessage ("InstantiateObjects");

		//Debug for looking at the room list and their associated buttons
		/*for (int k = 0; k < roomOrder.Count; k ++) 
		{
			Debug.Log("Current room: " + roomOrder[k]);
			for (int j = 0; j < buttonList[k].Count; j ++)
			{
				Debug.Log("Associated buttons: " + buttonList[k][j]);
			}
		}*/

		/*for (int roomNum = 0; roomNum < roomOrder.Count; roomNum ++) 
		{
			Debug.Log(roomOrder[roomNum]);
		}*/
		/*
		for(int firstroom = 0; firstroom < sharedPiecesList.Count; firstroom ++)
		{
			for (int secondroom = firstroom + 1; secondroom < sharedPiecesList[firstroom].Count; secondroom ++)
			{
				Debug.Log("Current Rooms: " + firstroom + ", and " + (secondroom ));
				for (int sharedPiecesIndex = 0; sharedPiecesIndex < sharedPiecesList[firstroom][secondroom].Count; sharedPiecesIndex ++)
				{
					Debug.Log(sharedPiecesList[firstroom][secondroom][sharedPiecesIndex][0] + "," + sharedPiecesList[firstroom][secondroom][sharedPiecesIndex][1]);
				}
			}
		}*/

		/*for (int roomNumber = 0; roomNumber < borderPiecesList.Count; roomNumber ++)
		{
			Debug.Log("Current Room: " + roomNumber);
			for(int k = 0; k < borderPiecesList[roomNumber].Count; k ++)
			{
				Debug.Log(borderPiecesList[roomNumber][k][0] + "," + borderPiecesList[roomNumber][k][1]);
			}
		}*/

		/*
		FindSharedEdges ();

		roomOrder.Add (FindLastRoom());
		CalcRoomOrder (FindLastRoom());

		for (int i = 0; i <  (roomOrder.Count - 1); i ++) 
		{
			AddDoors(roomOrder[i + 1], roomOrder[i]);
		}

		floorPlanner.SendMessage ("InstantiateObjects");

		//InstantiateRooms ();

		/*for (int i = 0; i < sharedRooms.GetLength(0); i ++) 
		{
			Debug.Log("CONNECTION: " + i);
			Debug.Log(sharedRooms[i,0] + "," + sharedRooms[i,1]);
		}*/

		/*for (int i = 0; i < borderPieces.GetLength(0); i ++) 
		{
			Debug.Log("ROOM: " + i);
			for (int j = 0; j < borderPieces.GetLength(1); j ++)
			{
				Debug.Log(borderPieces[i,j,0] + "," + borderPieces[i,j,1]);
			}
		}*/

		/*for(int i = 0; i <= floorPlan.numberOfWalls; i++)
		{
			Debug.Log("ROOM: " + i);
			for (int j = 0; j < 4; j++)
			{
				Debug.Log(rooms[i,j]);
			}
		}
		*/
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	void InstantiateRooms()
	{
		Vector3 roomPos;
		for (int roomTest = 0; roomTest < roomTesters.Length; roomTest ++) 
		{
			roomTesters[roomTest].transform.localScale = new Vector3((float)rooms[roomTest,1] * 3, (float)rooms[roomTest,0] * 3, 1.0f);
		}

		for (int room = 0; room < rooms.GetLength(0); room ++) 
		{
			roomPos.x = rooms[room, 2] * 3;
			roomPos.y = rooms[room,3] * 3;
			roomPos.z = 2;
			Instantiate(roomTesters[room],roomPos, Quaternion.identity);
		}

	}*/

	void CalcCurrentRoom(int currentRoom, int startingX, int startingY)
	{
		for (int rowNumber = startingY; rowNumber < floorPlan.elementIdMatrixList.Count; rowNumber++) //Check up
		{
			if(floorPlan.elementIdMatrixList[rowNumber][startingX][0] == 1)
			{
				roomsList[currentRoom][0] = rowNumber - startingY ; // Assign the row number - Starting y as the room height

				if (startingX == 0 || floorPlan.elementIdMatrixList[rowNumber + 1][startingX - 1][0] == 2)
				{
					bool inRoom = true;
					for(int listCheck = 1; listCheck < roomsList.Count; listCheck ++)
					{

						//Check the roomsList if the current x and y coordinates already exist within it. If they do, do not add the currentroom to the list
							if(roomsList[listCheck][2] == startingX && roomsList[listCheck][3] == (startingY + roomsList[currentRoom][0] + 1))
							{
								inRoom = false;
								continue;
							}
				
					}
					if(inRoom)
					{
						roomsList[increment][2] = startingX;//Assigning X and Y coordinates of Room 2
						roomsList[increment][3] = startingY +  roomsList[currentRoom][0] + 1;
						increment ++;
					}
				}
				break;
			}

			if((startingX == 0 && rowNumber + 1 == floorPlan.elementIdMatrixList.Count) || (rowNumber + 1 == floorPlan.elementIdMatrixList.Count))
			{
				roomsList[currentRoom][0] = rowNumber - startingY + 1;
				break;
			}


		}

		for (int colNumber = startingX; colNumber < floorPlan.elementIdMatrixList[0].Count; colNumber ++) //Check right
		{
			if (floorPlan.elementIdMatrixList[startingY][colNumber][0] == 2)
			{
				roomsList[currentRoom][1] = colNumber - startingX;//Assign current room width

				if (startingY == 0 || floorPlan.elementIdMatrixList[startingY - 1][colNumber + 1][0] == 1)
				{
					bool inRoom = true;
					for(int listCheck = 1; listCheck < roomsList.Count; listCheck ++)
					{	

							//Check the roomsList if the current x and y coordinates already exist within it. If they do, do not add the currentroom to the list
							if(roomsList[listCheck][3] == startingY && roomsList[listCheck][2] == (startingX + roomsList[currentRoom][1] + 1) )
							{
								inRoom = false;
								continue;
							}
					}
					if(inRoom)
					{
						roomsList[increment][2] = startingX +  roomsList[currentRoom][1] + 1;//Assigning X and Y coordinates of Room 2
						roomsList[increment][3] = startingY;
						increment ++;
					}
				}
				break;
			}

			if ((startingY == 0 && colNumber + 1 == floorPlan.elementIdMatrixList[0].Count)|| (colNumber + 1 == floorPlan.elementIdMatrixList[0].Count))
			{
				roomsList[currentRoom][1] = colNumber - startingX + 1;
				break;
			}

			if (colNumber + 1 == floorPlan.elementIdMatrixList[0].Count)
			{
				roomsList[currentRoom][1] = colNumber - startingX + 1;
			}
		}

		if(currentRoom + 1 <= floorPlan.numberOfWalls)
		{
			currentRoom++;
			CalcCurrentRoom(currentRoom, roomsList[currentRoom][2], roomsList[currentRoom][3]);
		}

	}

	void FindBorders()
	{
		for (int room = 0; room < roomsList.Count; room ++) 
		{
			int borderNum = 0;
			//Check bottom edges
			List<List<int>> borderWalls = new List<List<int>>();
			if(roomsList[room][3] != 0)
			{
				//check along the room's width value
				for (int w = 0; w < roomsList[room][1]; w++)
				{
					List<int> xYcoordinates = new List<int>();
					xYcoordinates.Add(w + roomsList[room][2]);
					xYcoordinates.Add(roomsList[room][3] - 1);

					/*borderPieces[room,borderNum,0] = w + roomsList[room][2]; //add the 'w' which is the number of spaces over to the starting X coordinate
					borderPieces[room,borderNum,1] = (roomsList[room][3] - 1);*/
					borderWalls.Add (xYcoordinates);

					//borderNum ++;
				}

			}
			//Check top edges
			if(roomsList[room][0] + roomsList[room][3] < floorPlan.elementIdMatrixList.Count)
			{
				for (int w = 0; w < roomsList[room][1]; w ++)
				{
					List<int> xYcoordinates = new List<int>();
					xYcoordinates.Add(w + roomsList[room][2]);
					xYcoordinates.Add((roomsList[room][0]) + roomsList[room][3]);

					borderWalls.Add(xYcoordinates);

					/*
					borderPieces[room,borderNum,0] = w + roomsList[room][2];//add the 'w' which is the number of spaces over to the starting X coordinate
					borderPieces[room,borderNum,1] = ((roomsList[room][0]) + roomsList[room][3]);
					borderNum ++;*/
				}
			}
			//Check left edges
			if (roomsList[room][2] != 0)
			{
				for (int h = 0; h < roomsList[room][0]; h ++)
				{
					List<int> xYcoordinates = new List<int>();
					xYcoordinates.Add(roomsList[room][2] - 1);
					xYcoordinates.Add(h + roomsList[room][3]);

					borderWalls.Add(xYcoordinates);
					/*
					borderPieces[room,borderNum, 1] = h + roomsList[room][3]; //add the 'h' which is the number of spaces up to the starting Y coordinate
					borderPieces[room,borderNum,0] = (roomsList[room][2] - 1);
					borderNum++;*/
				}
			}

			//Check right edges
			if ((roomsList[room][1] + roomsList[room][2]) < floorPlan.elementIdMatrixList[0].Count)
			{
				for (int h = 0; h < roomsList[room][0]; h++)
				{
					List<int> xYcoordinates = new List<int>();
					xYcoordinates.Add((roomsList[room][1]) + roomsList[room][2]);
					xYcoordinates.Add(h + roomsList[room][3]);

					borderWalls.Add(xYcoordinates);
					/*
					borderPieces[room,borderNum,1] = h + roomsList[room][3];
					borderPieces[room,borderNum,0] = ((roomsList[room][1]) + roomsList[room][2]);
					borderNum++;*/
				}
			}

			borderPiecesList.Add(borderWalls);
		}
	}


	void FindSharedEdges()
	{
		//int sharedRoomCount = -1;
		for (int firstRoom = 0; firstRoom < roomsList.Count; firstRoom ++) 
		{
			List<List<List<int>>> secondRoomList = new List<List<List<int>>>();
			for (int i = 0; i <= floorPlan.numberOfWalls; i++)
			{
				List<List<int>> filler = new List<List<int>>();
				secondRoomList.Add(filler);
			}
			//sharedRoomCount ++;
			for (int secondRoom = 0; secondRoom < roomsList.Count; secondRoom ++)
			{
				if(firstRoom == secondRoom)
				{
					continue;
				}

				int indexNum = 0;
				List<List<int>> piecesList = new List<List<int>>();

				for (int piece = 0; piece < borderPiecesList[firstRoom].Count; piece ++)
				{
					for (int secondPiece = 0; secondPiece < borderPiecesList[secondRoom].Count; secondPiece ++)
					{
						if ((borderPiecesList[firstRoom][piece][0] == borderPiecesList[secondRoom][secondPiece][0]) && 
						    (borderPiecesList[firstRoom][piece][1] == borderPiecesList[secondRoom][secondPiece][1]))
						{

							List<int> xYcoordinate = new List<int>();
							//sharedRooms[sharedRoomCount,0] = firstRoom;
							//sharedRooms[sharedRoomCount,1] = secondRoom;
							xYcoordinate.Add(borderPiecesList[firstRoom][piece][0]);
							xYcoordinate.Add(borderPiecesList[firstRoom][piece][1]);
							
							piecesList.Add(xYcoordinate);
							//sharedPieces[firstRoom,secondRoom,indexNum,0] = borderPiecesList[firstRoom][piece][0];
							//sharedPieces[firstRoom,secondRoom,indexNum,1] = borderPiecesList[firstRoom][piece][1];

							if ((borderPiecesList[firstRoom][piece][0]) != 0 && (borderPiecesList[firstRoom][piece][1] != 0))
							{
								indexNum ++;
							
							}
						}

					}
				}
				secondRoomList[secondRoom] = piecesList;

			}
			sharedPiecesList.Add(secondRoomList);
		}
	}

	void BuildConnections()
	{
		for(int firstRoom = 0; firstRoom < roomsList.Count; firstRoom ++)
		{
			List<int> connectedRooms = new List<int>();
			for (int secondRoom = 0; secondRoom < roomsList.Count; secondRoom ++)
			{
				if(sharedPiecesList[firstRoom][secondRoom].Count != 0)
				{
					connectedRooms.Add(secondRoom);
				}
				else
				{
					continue;
				}
			}
			roomConnections.Add(connectedRooms);
		}
	}

	//Create a list of the order the rooms progress, using the last room as a stopping point
	void CalcRoomOrder(int currentRoom)
	{
		int iterations = 0;
		finalRoomOrder.Add (currentRoom);

		bool roomContained = true;
		int nextRoom = 0;
		bool allContained = false;


		while(roomContained)
		{
			if(iterations > 40)
			{
				Debug.Log("Runaway Loop: CalcRoomOrder");
				break;
			}

			int randomRoom = Random.Range(0, roomConnections[currentRoom].Count); //A randomly picked room from the list of connections with currentRoom
			nextRoom = roomConnections [currentRoom] [randomRoom];

			allContained = false;

			//Check if every possible connection is already in the finalRoomOrder list
			for(int roomCheck = 0; roomCheck < roomConnections[currentRoom].Count; roomCheck ++)
			{
				if (finalRoomOrder.Contains(roomConnections[currentRoom][roomCheck]))
				{
					allContained = true;
				}

				else
				{
					allContained = false;
					break;
				}
			}



			//if next room is already in the list
			if (finalRoomOrder.Contains(nextRoom)) 
			{
				iterations ++;
				continue;
			}

			else if(allContained)
			{
				iterations ++;
				roomContained = false;
			}

			else
			{
				iterations ++;
				roomContained = false;
			}

		}
		if (nextRoom == FindLastRoom())
		{
			//if the next room is the top right corner, end recursion
			finalRoomOrder.Add(nextRoom);
			allContained = true;
		}

		if(allContained == false)
		{
			CalcRoomOrder(nextRoom);
		}


	}

	//Build connections for appended rooms not included in the final room order list
	void CalcAppendedRooms()
	{
		for(int currentRoom = 0; currentRoom < roomsList.Count; currentRoom ++)
		{
			if(finalRoomOrder.Contains(currentRoom))
			{
				continue;
			}
			else
			{
				List<int> connection = new List<int>();
				int lowestRoom = roomsList.Count;
				for(int connectedRoom = 0; connectedRoom < roomConnections[currentRoom].Count; connectedRoom ++)
				{
					if(roomConnections[currentRoom][connectedRoom] < lowestRoom)
					{
						if(roomConnections[currentRoom][connectedRoom] == FindLastRoom())
						{
							continue;
						}
						lowestRoom = roomConnections[currentRoom][connectedRoom];
					}
				}
				connection.Add(lowestRoom);
				connection.Add(currentRoom);
				connectorsList.Add(connection);
				appendedRoomCount ++;
			}
		}
	}

	void CalcMainConnections()
	{
		//Go through each room in the finalRoomOrder, minus the last room, hence the Count - 1
		for(int currentRoom = 0; currentRoom < finalRoomOrder.Count - 1; currentRoom ++)
		{
			List<int> connection = new List<int>();
			connection.Add(finalRoomOrder[currentRoom]);
			connection.Add(finalRoomOrder[currentRoom + 1]);
			connectorsList.Add(connection);
		}
	}

	void PlaceDoors()
	{
		GameObject horizontalDoor = floorPlan.three;
		GameObject verticalDoor = floorPlan.four;
		GameObject button = floorPlan.five;

		Object thisDoor = null;
		Object thisButton = null;
		for(int currentConnection = 0; currentConnection < connectorsList.Count; currentConnection ++)
		{
			int firstRoom = connectorsList[currentConnection][0];
			int secondRoom = connectorsList[currentConnection][1];

			int doorPlace = Random.Range(0, sharedPiecesList[firstRoom][secondRoom].Count);

			int doorX = sharedPiecesList[firstRoom][secondRoom][doorPlace][0];
			int doorY = sharedPiecesList[firstRoom][secondRoom][doorPlace][1];

			Vector2 coordinate = Vector2.zero;

			//Instantiate a horizontal door based on the coordinateMatrixList
			if(floorPlan.elementIdMatrixList[doorY][doorX][0] == 1)
			{
				floorPlan.elementIdMatrixList[doorY][doorX][0] = 0;
				coordinate.x = floorPlan.coordinateMatrixList[sharedPiecesList[firstRoom][secondRoom][doorPlace][1]][sharedPiecesList[firstRoom][secondRoom][doorPlace][0]][0];
				coordinate.y = floorPlan.coordinateMatrixList[sharedPiecesList[firstRoom][secondRoom][doorPlace][1]][sharedPiecesList[firstRoom][secondRoom][doorPlace][0]][1];

				thisDoor = Instantiate(horizontalDoor,coordinate,Quaternion.identity);
				GameObject gameDoor = (GameObject)(thisDoor);
				gameDoor.GetComponent<doorScript1>().roomOne = firstRoom;
				gameDoor.GetComponent<doorScript1>().roomTwo = secondRoom;
				doorList.Add(gameDoor);
			}

			//Instantiate a vertical door based on the coordinateMatrixList
			else if (floorPlan.elementIdMatrixList[doorY][doorX][0] == 2)
			{
				floorPlan.elementIdMatrixList[doorY][doorX][0] = 0;
				coordinate.x = floorPlan.coordinateMatrixList[sharedPiecesList[firstRoom][secondRoom][doorPlace][1]][sharedPiecesList[firstRoom][secondRoom][doorPlace][0]][0];
				coordinate.y = floorPlan.coordinateMatrixList[sharedPiecesList[firstRoom][secondRoom][doorPlace][1]][sharedPiecesList[firstRoom][secondRoom][doorPlace][0]][1];
				
				thisDoor = Instantiate(verticalDoor,coordinate,Quaternion.identity);
				GameObject gameDoor = (GameObject)(thisDoor);
				gameDoor.GetComponent<doorScript1>().roomOne = firstRoom;
				gameDoor.GetComponent<doorScript1>().roomTwo = secondRoom;
				doorList.Add(gameDoor);
			}

		}

		//Check room connections. If the second element of a connection never shows up as the first element in any other connections,
		//it has to have a button in it.
		for(int secondConnector = 0; secondConnector < connectorsList.Count; secondConnector ++)
		{
			//Get the number in the second element of the current connection
			int secondConnection = connectorsList[secondConnector][1];
			if(secondConnection == FindLastRoom())
			{
				continue;
			}
			bool containsConnector = false;
			for (int firstConnector = 0; firstConnector < connectorsList.Count; firstConnector ++)
			{
				//Compare it to the first element of every connection
				int firstConnection = connectorsList[firstConnector][0];
				if(secondConnection == firstConnection)
				{
					containsConnector = true;
					break;
				}
				
				else 
				{
					containsConnector = false;
				}
			}

			if(!containsConnector)
			{
				//Debug.Log("Appended room: " + secondConnection);
				Vector2 buttonCoordinate = Vector2.zero;
				//Debug.Log ("room x coordinate: " + roomsList[secondConnection][2]);
				//Debug.Log ("room y coordinate: " + roomsList[secondConnection][3]);
				int buttonY = (Random.Range(1, roomsList[secondConnection][0])) + (roomsList[secondConnection][3] - 1);
				int buttonX = (Random.Range(1, roomsList[secondConnection][1])) + (roomsList[secondConnection][2] - 1);
				
				buttonCoordinate.x = floorPlan.coordinateMatrixList[buttonY][buttonX][0];
				buttonCoordinate.y = floorPlan.coordinateMatrixList[buttonY][buttonX][1];
				
				thisButton = Instantiate(button,buttonCoordinate,Quaternion.identity);
				GameObject gameButton = (GameObject)thisButton;
				gameButton.GetComponent<Trigger>().room = secondConnection;
				buttonList.Add(gameButton);

				//Add the room the button was put in to the buttonRoomsList for determining how many ghosts to allow
				if(buttonRoomsList.Contains(secondConnection) == false)
				{
					buttonRoomsList.Add(secondConnection);
				}
			}
		}

		//Connect all buttons in pendent rooms to a door in a higher level than the button itself occupies
		for(int pendentButton = 0; pendentButton < buttonList.Count; pendentButton ++)
		{
			bool doorOccupied = true;
			int iterations = 0;
			bool notOnLast = true;

			while(doorOccupied)
			{
				if(iterations > 40)
				{
					Debug.Log("Runaway loop: Main Pendent room");
					break;
				}

				int occupiedRoom = buttonList[pendentButton].GetComponent<Trigger>().room;
				//Find the level of the room the button is currently in
				for(int levelCheck = 0; levelCheck < levelList.Count; levelCheck ++)
				{
					if(levelList[levelCheck].Contains(occupiedRoom))
					{
						//Pick a random level that is equal to or higher
						int randomLevel = Random.Range(levelCheck, levelList.Count);
						bool notCurrentRoom = false;
						int firstConnectedRoom = 0;
						int iterationsCheck = 0; //Stop the loop from running forever
						while(!notCurrentRoom)
						{
							if(iterationsCheck > 110)
							{
								Debug.Log("Runaway loop: room checker");
								break;
							}
							//Check if the occupied room is in the last level and has no neighbors in that level
							if(levelCheck == levelList.Count - 1 && levelList[levelCheck].Count == 1)
							{
								Debug.Log("On last level by itself");
								for(int doorCheck = 0; doorCheck < doorList.Count; doorCheck ++)
								{
									if(doorList[doorCheck].GetComponent<doorScript1>().roomOne == FindLastRoom()||
									   doorList[doorCheck].GetComponent<doorScript1>().roomTwo == FindLastRoom())
									{
										Debug.Log("Door has last room");
										doorList[doorCheck].GetComponent<doorScript1>().myTrigger = buttonList[pendentButton];
										notCurrentRoom = true;
										notOnLast = false;
										break;
									}
								}
							}
							//Fix for if two pendant rooms are in the same ending level 
							else if(levelCheck == levelList.Count - 1 && levelList[levelCheck].Count == 2)
							{
								if(levelList[levelCheck].Contains(FindLastRoom()))
								{
									if (levelList[levelCheck][0] == FindLastRoom())
									{
										int buttonRoom = levelList[levelCheck][1];
										for(int doorCheck = 0; doorCheck < doorList.Count; doorCheck ++)
										{
											for(int roomCheck = 0; roomCheck < buttonRoomsList.Count; roomCheck++)
											{
												if(doorList[doorCheck].GetComponent<doorScript1>().roomOne == FindLastRoom()||
											   	doorList[doorCheck].GetComponent<doorScript1>().roomTwo == FindLastRoom())
												{
													if(buttonRoom == buttonList[roomCheck].GetComponent<Trigger>().room)
													{
														Debug.Log("Door has last room");
														doorList[doorCheck].GetComponent<doorScript1>().myTrigger = buttonList[roomCheck];
														notCurrentRoom = true;
														notOnLast = false;
														break;
													}
												}
											}
										}
									}
									else
									{
										int buttonRoom = levelList[levelCheck][0];
										for(int doorCheck = 0; doorCheck < doorList.Count; doorCheck ++)
										{
											for(int roomCheck = 0; roomCheck < buttonRoomsList.Count; roomCheck++)
											{
												if(doorList[doorCheck].GetComponent<doorScript1>().roomOne == FindLastRoom()||
												   doorList[doorCheck].GetComponent<doorScript1>().roomTwo == FindLastRoom())
												{
													if(buttonRoom == buttonList[roomCheck].GetComponent<Trigger>().room)
													{
														Debug.Log("Door has last room");
														doorList[doorCheck].GetComponent<doorScript1>().myTrigger = buttonList[roomCheck];
														notCurrentRoom = true;
														notOnLast = false;
														break;
													}
												}
											}
										}

									}
								}
								else if(levelList[levelCheck].Contains(FindLastRoom()) == false)
								{
								//picks random room to assign to very last room
								int randomRoom1 = Random.Range(0,2);
								//RandomRoom2 is always the other room in the array 
								int randomRoom2 = 1 - randomRoom1;

								for(int doorCheck = 0; doorCheck < doorList.Count; doorCheck ++)
								{
									//sets randomRoom1 to the very last room 
									if(doorList[doorCheck].GetComponent<doorScript1>().roomOne == FindLastRoom()||
								   	doorList[doorCheck].GetComponent<doorScript1>().roomTwo == FindLastRoom())
									{
										for(int roomCheck = 0; roomCheck < buttonRoomsList.Count; roomCheck++)
										{
											if(randomRoom1 == buttonList[roomCheck].GetComponent<Trigger>().room)
											{
												Debug.Log("Door has last room");
												doorList[doorCheck].GetComponent<doorScript1>().myTrigger = buttonList[roomCheck];
												notCurrentRoom = true;
												notOnLast = false;
												break;
											}
										}
									}
									//sets randomRoom2 to the other room in the last level 
									else if(doorList[doorCheck].GetComponent<doorScript1>().roomOne == randomRoom1 ||
									        doorList[doorCheck].GetComponent<doorScript1>().roomTwo == randomRoom1)
									{
											for(int roomCheck = 0; roomCheck < buttonRoomsList.Count; roomCheck++)
											{
												if(randomRoom2 == buttonList[roomCheck].GetComponent<Trigger>().room)
												{
													Debug.Log("Door has last room");
													doorList[doorCheck].GetComponent<doorScript1>().myTrigger = buttonList[roomCheck];
													notCurrentRoom = true;
													notOnLast = false;
													break;
												}
											}
									}
								}
								}
								iterationsCheck ++;
							}

							iterationsCheck ++;

							if(notOnLast)
							{
								int randomLevelIndex = Random.Range(0,levelList[randomLevel].Count);
								//FIRST ROOM IS SECOND INDEX IN CONNECTOR <X, FIRST ROOM>
								firstConnectedRoom = levelList[randomLevel][randomLevelIndex];
								//Debug.Log("Picked Connected Room: " + firstConnectedRoom);
								//Debug.Log("Occupied Room: " + occupiedRoom);
								//Debug.Log("Selected Level: " + randomLevel);
								//Check to make sure the selected room is not the same as the room the button currently occupies
								if(firstConnectedRoom != occupiedRoom)
								{
									iterationsCheck ++;
									notCurrentRoom = true;
								}
								else
								{
									iterationsCheck ++;
									continue;
								}
							}
						}

						if(notOnLast)
						{
							Debug.Log("First Connected Room: " + firstConnectedRoom);

							//Create list of all room connections that have firstConnectedRoom as their zero index
							List<int> firstRoomCandidates = new List<int>(); //This list is a list of indices within connectorsList
							for(int connectorCheck = 0; connectorCheck < connectorsList.Count; connectorCheck ++)
							{
								//Debug.Log("Connected Room Check: " + connectorsList[connectorCheck][1]);
								if(connectorsList[connectorCheck][1] == firstConnectedRoom)
								{
									firstRoomCandidates.Add(connectorCheck);
								}
							}

							//Pick a connection index from firstRoomCandidates and get the second room from it
							int secondConnectedRoom = 0;
							int randomConnection = Random.Range(0,firstRoomCandidates.Count);
							Debug.Log("Selected Connection Index: " + randomConnection);
							secondConnectedRoom = connectorsList[firstRoomCandidates[randomConnection]][0];
							Debug.Log("Actual Connection Index: " + secondConnectedRoom);

							//Check through doorList to find which one has the 
							//roomOne and roomTwo variables that respectively match with the firstConnectedRoom and secondConnectedRoom variables

							for(int currentDoor = 0; currentDoor < doorList.Count; currentDoor ++)
							{
								if(doorList[currentDoor].GetComponent<doorScript1>().roomOne == secondConnectedRoom && 
								   doorList[currentDoor].GetComponent<doorScript1>().roomTwo == firstConnectedRoom)
								{
									if(doorList[currentDoor].GetComponent<doorScript1>().myTrigger == null)
									{
										doorList[currentDoor].GetComponent<doorScript1>().myTrigger = buttonList[pendentButton];
										doorOccupied = false;
									}
								}
								else
								{
									continue;
								}
							}
							iterations ++;
						}
						iterations ++;
					}
				}
			}
		}
	}

	void PlaceButtons()
	{
		GameObject button = floorPlan.five;
		bool isLastRoom = false;

		for(int door = 0; door < doorList.Count; door ++)
		{
			isLastRoom = false;
			//The door does not have a button assigned to it
			if (doorList[door].GetComponent<doorScript1>().myTrigger == null)
			{
				//Get the roomTwo variable(Which is on the higher level
				int secondRoom = doorList[door].GetComponent<doorScript1>().roomTwo;

				if(secondRoom == FindLastRoom())
				{
					isLastRoom = true;
				}

				switch (isLastRoom)
				{
				case true:
						for ( int levelCheck = 0; levelCheck < levelList.Count; levelCheck ++)
						{
							//Find the level that roomTwo is on
							if (levelList[levelCheck].Contains(secondRoom))
							{
								Object thisButton;
								int buttonRoom = 0;
								int iterations = 0;
								while(true)
								{
								if(iterations > 40)
								{
									buttonRoom = 1;
									break;
								}
									int randomLowerLevel = Random.Range(0, levelCheck);
									int randomRoomIndex = Random.Range(0, levelList[randomLowerLevel].Count);
									buttonRoom = levelList[randomLowerLevel][randomRoomIndex];
								if(buttonRoom == 0)
								{
									iterations ++;
									continue;
								}
								else
								{
									break;
								}
								}
								
								//Debug.Log("Appended room: " + secondConnection);
								Vector2 buttonCoordinate = new Vector2(1,1);
								//Debug.Log ("room x coordinate: " + roomsList[secondConnection][2]);
								//Debug.Log ("room y coordinate: " + roomsList[secondConnection][3]);
								int buttonY = (Random.Range(1, roomsList[buttonRoom][0])) + (roomsList[buttonRoom][3] - 1);
								int buttonX = (Random.Range(1, roomsList[buttonRoom][1])) + (roomsList[buttonRoom][2] - 1);

								if(buttonY == 0 && buttonX == 0)
								{
									buttonY = 1;
									buttonX = 1;
								}
								
								buttonCoordinate.x = floorPlan.coordinateMatrixList[buttonY][buttonX][0];
								buttonCoordinate.y = floorPlan.coordinateMatrixList[buttonY][buttonX][1];
								
								thisButton = Instantiate(button,buttonCoordinate,Quaternion.identity);
								GameObject gameButton = (GameObject)thisButton;
								gameButton.GetComponent<Trigger>().room = buttonRoom;
								if(buttonRoomsList.Contains(buttonRoom) == false)
								{
									buttonRoomsList.Add(buttonRoom);
								}
								
								//Assign button to the doorList[door]
								doorList[door].GetComponent<doorScript1>().myTrigger = gameButton;
							}
						}
					break;

				case false:
					for ( int levelCheck = 0; levelCheck < levelList.Count; levelCheck ++)
					{
						//Find the level that roomTwo is on
						if (levelList[levelCheck].Contains(secondRoom))
						{
							Object thisButton;
							
							int randomLowerLevel = Random.Range(0, levelCheck);
							int randomRoomIndex = Random.Range(0, levelList[randomLowerLevel].Count);
							int buttonRoom = levelList[randomLowerLevel][randomRoomIndex];
							
							//Debug.Log("Appended room: " + secondConnection);
							Vector2 buttonCoordinate = new Vector2(1,1);
							//Debug.Log ("room x coordinate: " + roomsList[secondConnection][2]);
							//Debug.Log ("room y coordinate: " + roomsList[secondConnection][3]);
							int buttonY = (Random.Range(1, roomsList[buttonRoom][0])) + (roomsList[buttonRoom][3] - 1);
							int buttonX = (Random.Range(1, roomsList[buttonRoom][1])) + (roomsList[buttonRoom][2] - 1);

							if(buttonY == 0 && buttonX == 0)
							{
								buttonY = 1;
								buttonX = 1;
							}
							
							buttonCoordinate.x = floorPlan.coordinateMatrixList[buttonY][buttonX][0];
							buttonCoordinate.y = floorPlan.coordinateMatrixList[buttonY][buttonX][1];
							
							thisButton = Instantiate(button,buttonCoordinate,Quaternion.identity);
							GameObject gameButton = (GameObject)thisButton;
							gameButton.GetComponent<Trigger>().room = buttonRoom;
							if(buttonRoomsList.Contains(buttonRoom) == false)
							{
								buttonRoomsList.Add(buttonRoom);
							}
							
							//Assign button to the doorList[door]
							doorList[door].GetComponent<doorScript1>().myTrigger = gameButton;
						}
					}
					break;
				default:
					break;
				}

			}
		}
	}


	void BuildLevels()
	{
		for(int currentRoom = 0; currentRoom < finalRoomOrder.Count; currentRoom ++)
		{
			List<int> levelOccupant = new List<int>();
			levelOccupant.Add(finalRoomOrder[currentRoom]);
			levelList.Add(levelOccupant);
		}

		for (int appendedRoom = 0; appendedRoom <= appendedRoomCount; appendedRoom ++)
		{
			//Look at the first element in the list of appended room connections
			int firstRoom = connectorsList[appendedRoom][0];
			for (int roomCheck = appendedRoomCount; roomCheck < connectorsList.Count; roomCheck ++)
			{
				//If the selected room number matches the first element of a list in the main connectorlist
				if(connectorsList[roomCheck][0] == firstRoom)
				{
					//Add the second element within the appended room connection list to the current level in the tree
					if(levelList[(roomCheck - appendedRoomCount) + 1].Contains(connectorsList[appendedRoom][1]))
					{
						continue;
					}
					else
					{
						levelList[(roomCheck - appendedRoomCount) + 1].Add(connectorsList[appendedRoom][1]);
						break;
					}
				}
			}
		}
	}

	void FixLevels()
	{
		//Check to make sure that the levels list has every room in it.
		for (int roomCheck = 0; roomCheck < roomsList.Count; roomCheck ++) 
		{
			bool numberContained = true;
			for (int levelCheck = 0; levelCheck < levelList.Count; levelCheck ++)
			{
				if(levelList[levelCheck].Contains(roomCheck))
				{
					numberContained = true;
					break;
				}
				
				else
				{
					numberContained = false;
					continue;
				}
			}
			
			if(numberContained == false)
			{
				//If the number is not contained within the levels list, look through the connections list to find what room it's connected to
				for(int connectionCheck = 0; connectionCheck < connectorsList.Count; connectionCheck ++)
				{
					if(connectorsList[connectionCheck][1] == roomCheck)
					{
						int connectedRoom = connectorsList[connectionCheck][0];
						//Look through the level list to find the connected room
						for(int levelCheck2 = 0; levelCheck2 < levelList.Count; levelCheck2 ++)
						{
							if(levelList[levelCheck2].Contains(connectedRoom))
							{
								if(levelCheck2 == (levelList.Count - 1))
								{
									//Add new level to the level list
									List<int> levelOccupant = new List<int>();
									levelOccupant.Add(roomCheck);
									levelList.Add(levelOccupant);
									break;
								}
								else
								{
									levelList[levelCheck2 + 1].Add(roomCheck);
									break;
								}
							}
							
							else
							{
								continue;
							}
						}
						break;
					}
				}
			}
		}
	}



	void StackRooms(int room)
	{
		for (int i = 0; i <= floorPlan.numberOfWalls; i ++) 
		{
			roomOrder.Add(i);
		}

		roomOrder.Remove (room);
		roomOrder.Add (room);
	}
	//TAKES IN LAST ROOM 
	/*
	void BuildButtonList()
	{
		//RANDOM NUMBER FOR ASSIGNING BUTTONS - cannot == 0 or 1

		for(int buttonRoom = 2; buttonRoom < buttonList.Count; buttonRoom ++)
		{
			int indexPlacement;
			List<int> insideButtonList = new List<int> (); //List to be added to buttonList
			while(true)
			{
				indexPlacement = Random.Range(1,roomOrder.Count - 1);
				if(indexPlacement == buttonRoom)
				{
					continue;
				}
				else
				{
					break;
				}
			}
			if(buttonList[indexPlacement][0] == 0)
			{
				insideButtonList.Add(buttonRoom);
				buttonList[indexPlacement] = insideButtonList;
			}

			else
			{
				buttonList[indexPlacement].Add(buttonRoom);
			}
		}

	}*/

	int FindLastRoom()
	{
		for (int room = 0; room < roomsList.Count; room ++) 
		{
			if (roomsList[room][0] + roomsList[room][3] == floorPlan.elementIdMatrixList.Count && 
			    roomsList[room][1] + roomsList[room][2] == floorPlan.elementIdMatrixList[0].Count)
			{
				return room;
			}

		}

		return 0;
	}



}
