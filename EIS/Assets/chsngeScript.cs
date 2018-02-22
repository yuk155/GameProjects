using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class chsngeScript : MonoBehaviour {

	public GameObject ButtonController;
	public ButtonControl buttonControl;

//	public GameObject friendController;
//	public MeshController meshController;

	public Text promptText;
	public Text text1;
	public Text text2;
	public Text text3;
	public Text text4;

	//node tree for the script - hardcoded for 14 nodes + 5 values (prompt + 4 text)
	//0 is the prompt, 1 = text1, 2 = text2, 3 = text3, 4 = text4
	string[,] nodeTree = new string[14,5];
	public int currentNode = 0;
	

	//variable to see if the game has ended
	public bool isGameEnd =false;

	//array to hold the information to do data collection
	//the first number is the node, the second number is the button index
	int[,] saveData = new int[6,2];
	int saveDataIndex = 0;

	//some data 
	int goodAnswerNum = 0;
	int badAnswerNum = 0;
	int okayAnswerNum = 0;

	//In order to change poses
	public GameObject FriendController;
	//public GameObject friend;
	MeshController meshController;
	//0 is the start pose; 1 is the sad pose; 2 is the okay ending pose; 
	//3 is bad ending; 4 is the good ending 
	public int setPose;
	public int previousPose;

	public GameObject pose;

	// Use this for initialization
	void Start () {
		//setPose = 0;
		//previousPose = 0;

		goodAnswerNum = 0;
		badAnswerNum = 0;
		okayAnswerNum = 0;

		meshController = FriendController.GetComponent<MeshController> ();
		meshController.clearPoses ();

		//Debug.Log (meshController.poseArray [1].GetComponents <Texture>());

		//pose.SetActive (false);

	//	meshController.poseArray [1].SetActive(false);
	//	meshController.poseArray [0].SetActive (true);
	//	meshController.poseArray [1].SetActive(false);
	//	meshController.poseArray [2].SetActive(false);
	//	meshController.poseArray [3].SetActive(false);
	//	meshController.poseArray [4].SetActive(false);
		//meshController.poseArray [5].SetActive(false);

		buttonControl = ButtonController.GetComponent<ButtonControl>();

		//load the node tree with the data --> makes this easier to print files later
		//start node
		nodeTree [0,0] = "Hey, how's it going?";
		nodeTree [0,1] = "Good, how are you?";
		nodeTree [0,2] = "Good";
		nodeTree [0,3] = "I'm fine";
		nodeTree [0,4] = "My day is going good. How's yours?";

		//good node
		nodeTree [1,0] = "Oh...My day is not going so well";
		nodeTree [1,1] = "Oh I see";
		nodeTree [1,2] = "Oh no. What's wrong?";
		nodeTree [1,3] = "Did something happen?";
		nodeTree [1,4] = "That's too bad";

		//bad node
		nodeTree [2,0] = "Oh, I wish my day was going better";
		nodeTree [2,1] = "Ah I see";
		nodeTree [2,2] = "That's not good";
		nodeTree [2,3] = "Oh no. What's wrong?";
		nodeTree [2,4] = "I'm sorry";

		//good node
		nodeTree [3,0] = "I got my test back today, and it's not very good.";
		nodeTree [3,1] = "I'm sorry. How bad is it?";
		nodeTree [3,2] = "So it's bad?";
		nodeTree [3,3] = "Oh... You didn't do very well then?";
		nodeTree [3,4] = "That's not a good thing";

		//okay node
		nodeTree [4,0] = "Yea, it kind of sucks. I got my test back today and I didn't do well";
		nodeTree [4,1] = "I'm sorry. How bad is it?";
		nodeTree [4,2] = "So it's bad?";
		nodeTree [4,3] = "That's not good";
		nodeTree [4,4] = "Oh, you didn't do very well then?";

		//bad node
		nodeTree [5,0] = "Yea, I'm worried about my grade";
		nodeTree [5,1] = "Why?";
		nodeTree [5,2] = "Why are you worried?";
		nodeTree [5,3] = "Your grade on the test?";
		nodeTree [5,4] = "Did something happen?";

		//good node
		nodeTree [6,0] = "I failed";
		nodeTree [6,1] = "That's not good";
		nodeTree [6,2] = "I'm sorry";
		nodeTree [6,3] = "Did you not study?";
		nodeTree [6,4] = "That's too bad";

		//bad node
		nodeTree [7,0] = "I failed the test";
		nodeTree [7,1] = "That's too bad";
		nodeTree [7,2] = "Did you not study?";
		nodeTree [7,3] = "Oh no!";
		nodeTree [7,4] = "I'm so sorry";

		//good node
		nodeTree [8,0] = "I don't know what happened. I thought I studied enough.";
		nodeTree [8,1] = "Apparantly you didn't";
		nodeTree [8,2] = "I could help you study next time";
		nodeTree [8,3] = "Maybe next time you need to study more";
		nodeTree [8,4] = "You could ask the teacher for help for the next test";

		//okay node
		nodeTree [9,0] = "Yea, I know. I thought I studied enough";
		nodeTree [9,1] = "It probably wasn't hard enough";
		nodeTree [9,2] = "Maybe next time you'll get it";
		nodeTree [9,3] = "You should ask the teacher for help";
		nodeTree [9,4] = "Next time you should study harder";

		//bad node
		nodeTree [10,0] = "I thought I studied pretty hard";
		nodeTree [10,1] = "Maybe next time you can do a better job studying";
		nodeTree [10,2] = "Hopefully next time you can study harder";
		nodeTree [10,3] = "Maybe you could ask the teacher for help";
		nodeTree [10,4] = "It probably wasn't hard enough";

		//good ending
		nodeTree [11,0] = "Yea, That's a good idea! Thanks for your help!";
		nodeTree [11,1] = "";
		nodeTree [11,2] = "";
		nodeTree [11,3] = "";
		nodeTree [11,4] = "";

		//okay ending
		nodeTree [12,0] = "I guess so. I just don't want to fail again";
		nodeTree [12,1] = "";
		nodeTree [12,2] = "";
		nodeTree [12,3] = "";
		nodeTree [12,4] = "";

		//bad ending
		nodeTree [13,0] = "I guess I just didn't do a good job this time";
		nodeTree [13,1] = "";
		nodeTree [13,2] = "";
		nodeTree [13,3] = "";
		nodeTree [13,4] = "";

		//sets the text to be at the start node in the initialization
		setText();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
		{
			saveToArray();
			nextNode();
			setText();
			Debug.Log("Pose Update");
			setFriendMesh();
			meshController.changePose();

			//Debug.Log ("Num of Good: " + goodAnswerNum);
			//Debug.Log("Num of Bad: " + badAnswerNum);
			//Debug.Log("Num of Okay: " + okayAnswerNum);
		}

		//reset 
		if(Input.GetKeyDown(KeyCode.R))
		{
			goodAnswerNum = 0;
			badAnswerNum = 0;
			okayAnswerNum = 0;
			currentNode = 0;
			saveDataIndex = 0;
			setText();
			setFriendMesh();
			isGameEnd = false;
			meshController.clearPoses();
		}

		if(currentNode == 12 || currentNode == 13 || currentNode == 11)
		{
			//saves the last node
			saveData[5,0] = currentNode;
			isGameEnd = true;
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("TitleScreen");
		}
	
	}
	public void nextNode()
	{
		switch(currentNode)
		{
			case 0:
				//start node
				if(buttonControl.glowArrayIndex == 0)
				{	
					currentNode = 1;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{
					currentNode = 2;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 2;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 1;
					goodAnswerNum++;
				}
				break;
			case 1:
				//good node
				if(buttonControl.glowArrayIndex == 0)
				{
					currentNode = 5;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{
					currentNode = 3;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 3;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 4;
					okayAnswerNum++;
				}
				break;
			case 2:
				//bad node
				if(buttonControl.glowArrayIndex == 0)
				{
					currentNode = 5;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{	
					currentNode = 4;
					okayAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 3;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 4;
					okayAnswerNum++;
				}
				break;
			case 3:
				//good node
				if(buttonControl.glowArrayIndex == 0)
				{
					currentNode = 6;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{
					currentNode = 7;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 6;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 7;
					badAnswerNum++;
				}
				break;
			case 4:
				//okay node
				if(buttonControl.glowArrayIndex == 0)
				{
					currentNode = 6;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{
					currentNode = 7;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 7;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 6;
					goodAnswerNum++;
				}
				break;
			case 5:
				//bad node
				if(buttonControl.glowArrayIndex == 0)
				{
					currentNode = 7;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{
					currentNode = 6;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 7;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 6;
					okayAnswerNum++;
				}	
				break;
			case 6:	
				//good node
				if(buttonControl.glowArrayIndex == 0)
				{
					currentNode = 9;
					okayAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{
					currentNode = 8;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 10;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 8;
					goodAnswerNum++;
				}
				break;
			case 7:
				//bad node
				if(buttonControl.glowArrayIndex == 0)
				{
					currentNode = 9;
					okayAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{
					currentNode = 10;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 9;
					okayAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 8;
					goodAnswerNum++;
				}
				break;
			case 8:
				//good node
				if(buttonControl.glowArrayIndex == 0)
				{
					currentNode = 13;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{
					currentNode = 11;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 12;
					okayAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 11;
					goodAnswerNum++;
				}
				break;
			case 9:
				//okay node
				if(buttonControl.glowArrayIndex == 0)
				{
					currentNode = 13;
					badAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{
					currentNode = 12;
					okayAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 11;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 12;
					okayAnswerNum++;
				}
				break;
			case 10:
				//bad node
				if(buttonControl.glowArrayIndex == 0)
				{
					currentNode = 12;
					okayAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 1)
				{
					currentNode = 12;
					okayAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 2)
				{
					currentNode = 11;
					goodAnswerNum++;
				}
				if(buttonControl.glowArrayIndex == 3)
				{
					currentNode = 13;
					badAnswerNum++;
				}
				break;
		}
	}
	public void setText()
	{
		promptText.text = nodeTree [currentNode, 0];
		text1.text = nodeTree [currentNode, 1];
		text2.text = nodeTree [currentNode, 2];
		text3.text = nodeTree [currentNode, 3];
		text4.text = nodeTree [currentNode, 4];
	}

	public bool gameEnded()
	{
		if(isGameEnd)
		{
			return true;
		}
		return false;
	}

	public void saveToArray()
	{
		saveData [saveDataIndex, 0] = currentNode;
		saveData [saveDataIndex, 1] = buttonControl.glowArrayIndex + 1;

		saveDataIndex++;
	}
	public int[,] getSaveArray()
	{
		return saveData;
	}
	public string[,] getNodeTree()
	{
		return nodeTree;
	}
	public int getGoodAnswerNum()
	{
		return goodAnswerNum;
	}
	public int getBadAnswerNum()
	{
		return badAnswerNum;
	}
	public int getOkayAnswerNum()
	{
		return okayAnswerNum;
	}

	public void setFriendMesh()
	{
		switch(currentNode)
		{
		case 0:
			//start node 
			previousPose = setPose;
			setPose = 0;
			//Debug.Log ("Node 0");
			break;
		case 1:
			//good node
			previousPose = setPose;
			setPose = 1;
			break;
		case 2:
			//bad node
			previousPose = setPose;
			setPose = 2;
			break;
		case 3:
			//good node
			previousPose = setPose;
			setPose = 1;
			break;
		case 4:
			//okay node
			previousPose = setPose;
			setPose = 3;
			break;
		case 5:
			//bad node
			previousPose = setPose;
			setPose = 2;
			break;
		case 6:
			//good node
			previousPose = setPose;
			setPose = 2;
			break;
		case 7:
			//bad node
			previousPose = setPose;
			setPose = 2;
			break;
		case 8:
			//good node
			previousPose = setPose;
			setPose = 1;
			break;
		case 9:
			//okay node
			previousPose = setPose;
			setPose = 3;
			break;
		case 10:
			//bad node
			previousPose = setPose;
			setPose = 2;
			break;
		case 11:
			//good ending
			previousPose = setPose;
			setPose = 4;
			break;
		case 12:
			//okay ending 
			previousPose = setPose;
			setPose = 5;
			break;
		case 13:
			//bad ENDING
			previousPose = setPose;
			setPose = 6;
			break;
		}

	}
}
