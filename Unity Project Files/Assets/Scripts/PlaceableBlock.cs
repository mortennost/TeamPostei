using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceableBlock : MonoBehaviour 
{
	public Color[] colors;

	Vector2 blockSize = new Vector2(2f, 2f);

	GameObject[,] nodeArray;
	List<GameObject> occupiedNodes = new List<GameObject>();

	int currentX = 0;
	int currentY = 0;
	int startX = 8;
	int startY = 4;

	SpriteRenderer sr;
	RandomizeBlock rb;

	float timeUntilNextInputUpdate = 0.1f;
	float inputTimer = 0.0f;

	int playerPoints = 0;
	int nextPointMilestone;
	int milestonesHit = 0;
	int draws = 0;

	GameObject drawCounter;
	GameObject pointsPopup;
	GameObject pointsText;
	GameObject milestoneText;
	GameObject rewardText;
	GameObject startText;
	GameObject currentText;
	SpriteRenderer reward;
	Color rewardColor;
	//float rewardAlpha = 255.0f;

	bool showReward = false;
	float timeToShowReward = 5.0f;
	float rewardTimer = 0.0f;

	bool canPlayBlocked = true;
	float blockedCooldown = 0.5f;
	float blockedCooldownTimer = 0.0f;

	bool popupEnabled = false;
	float popupAlpha = 1.0f;

	public AudioSource moveSound;
	public AudioSource placeSound;
	public AudioSource rotateSound;
	public AudioSource blockedSound;

	public GameObject[] rewardObjects;
	public AudioSource[] milestoneSounds;

	// Use this for initialization
	void Start () 
	{
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<RandomizeBlock>();
		blockSize = rb.GetShape();

		// Get the array of nodes that are on the board
		nodeArray = GameObject.Find("board").GetComponent<DivideBoardIntoNodes>().GetNodes();

		// "Spawn" block by setting which nodes to occupy
		//Spawn();

		drawCounter = GameObject.Find("DrawCounter");
		pointsPopup = GameObject.Find("PointsPopup");
		pointsText = GameObject.Find("PointsText");
		milestoneText = GameObject.Find("MilestoneText");
		rewardText = GameObject.Find("RewardText");
		startText = GameObject.Find("StartText");
		reward = GameObject.Find("Reward").GetComponent<SpriteRenderer>();
		currentText = GameObject.Find("Current:");
		rewardColor = reward.color;

		nextPointMilestone = 1000;
		pointsText.guiText.text = "POENG: " + playerPoints;
		milestoneText.guiText.text = "MÅL: " + nextPointMilestone;
		drawCounter.guiText.text = "TREKK: " + draws;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!showReward)
		{
			if(!canPlayBlocked)
			{
				blockedCooldownTimer += Time.deltaTime;

				if(blockedCooldownTimer >= blockedCooldown)
				{
					canPlayBlocked = true;
					blockedCooldownTimer = 0.0f;
				}
			}

			if(popupEnabled)
			{
				if(popupAlpha > 0.0f)
				{
					popupAlpha -= Time.deltaTime / 2f;
				}
				else
				{
					popupEnabled = false;
					pointsPopup.guiText.enabled = false;
				}

				pointsPopup.transform.position = Vector2.Lerp(new Vector2(0.57f, 0.9f), new Vector2(0.57f, 0.7f), popupAlpha);
				pointsPopup.guiText.color = Color.Lerp(new Color(Color.white.r, Color.white.g, Color.white.b, 0f), Color.white, popupAlpha);
			}

			if(playerPoints >= nextPointMilestone)
			{
				milestonesHit++;
				nextPointMilestone += 1000;
				milestoneText.guiText.text = "MÅL: " + nextPointMilestone;
				showReward = true;
			}
			
			inputTimer += Time.deltaTime;
			
			if(Input.GetKeyDown(KeyCode.Space))
			{
				Rotate();
			}
			
			if(Input.GetKeyDown(KeyCode.Return))
			{
				if(startText.guiText.enabled)
				{
					startText.guiText.enabled = false;
					pointsText.guiText.enabled = true;
					milestoneText.guiText.enabled = true;
					drawCounter.guiText.enabled = true;
					draws = 0;
				}
				else
				{
					draws++;
				}

				PlaceBlock();
				Spawn();
			}
			
			if(inputTimer >= timeUntilNextInputUpdate)
			{
				inputTimer = 0.0f;
				if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				{
					Move(0, -1);
				}
				
				if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				{
					Move(-1, 0);
				}
				
				if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				{
					Move(0, 1);
				}
				
				if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				{
					Move(1, 0);
				}
			}
		}
		else
		{
			if(milestonesHit < 6)
			{
				rewardTimer += Time.deltaTime;
				
				if(rewardTimer < timeToShowReward)
				{
					for(int i = 0; i < rewardObjects.Length; i++)
					{
						rewardObjects[i].GetComponent<SpriteRenderer>().enabled = true;
					}

					for(int i = 0; i < milestonesHit; i++)
					{
						rewardObjects[i].GetComponent<SpriteRenderer>().enabled = false;
					}

					ShowReward(true);
					pointsText.guiText.enabled = false;
					milestoneText.guiText.enabled = false;
					currentText.guiText.enabled = false;
					drawCounter.guiText.enabled = false;
					//rewardAlpha /= rewardTimer;
					//reward.color = new Color(rewardColor.r, rewardColor.g, rewardColor.b, rewardAlpha);
				}
				else
				{
					ShowReward(false);
					showReward = false;
					rewardTimer = 0.0f;
					pointsText.guiText.enabled = true;
					milestoneText.guiText.enabled = true;
					currentText.guiText.enabled = true;
					drawCounter.guiText.enabled = true;

					for(int i = 0; i < rewardObjects.Length; i++)
					{
						rewardObjects[i].GetComponent<SpriteRenderer>().enabled = false;
					}
				}
			}
			else
			{
				if(!reward.enabled)
				{
					reward.enabled = true;
					rewardText.guiText.enabled = true;
					pointsText.guiText.enabled = false;
					milestoneText.guiText.enabled = false;
					currentText.guiText.enabled = false;
					pointsPopup.guiText.enabled = false;
					drawCounter.guiText.enabled = false;
					rewardText.guiText.text = "Gratulerer! Du vant! Du brukte [" + draws + "] trekk. \nTrykk [SPACE] for å laste ned vinnerbildet! \nTrykk [ENTER] for å starte spillet på nytt!";

					for(int i = 0; i < rewardObjects.Length; i++)
					{
						rewardObjects[i].GetComponent<SpriteRenderer>().enabled = false;
					}
				}

				if(Input.GetKeyDown(KeyCode.Escape))
				{
					Application.LoadLevel(0);
				}
				if(Input.GetKeyDown(KeyCode.Space))
				{
					Application.OpenURL("http://mattisdelerud.com/nothingOfTheKind/gratulererDuKlarteDetJo1920.jpg");
				}
			}
		}
	}

	void ShowReward(bool show)
	{
		if(show)
		{
			if(!reward.enabled)
			{
				reward.enabled = true;
				rewardText.guiText.enabled = true;
				rewardText.guiText.text = "Gratulerer! Du har vunnet del " + milestonesHit + " av 6. \nGjør deg klar for neste runde!";
				//print("Picture: " + milestonesHit + " of 6");
			}
		}
		else
		{
			reward.color = new Color(rewardColor.r, rewardColor.g, rewardColor.b, 255.0f);
			reward.enabled = false;
			rewardText.guiText.enabled = false;
		}
	}

	void PlaceBlock()
	{
		int numOfBlocksToGivePoints = 0;
		int pointsThisBlock = 0;

		foreach(GameObject node in occupiedNodes)
		{
			BoardNode bn = node.GetComponent<BoardNode>();

			if(bn.GetDefaultColor() == rb.GetColor()/*sr.color*/)
			{
				//bn.Deactivate();
				bn.SetDefaultColor(Color.white);
				numOfBlocksToGivePoints++;
			}
			else //if(bn.IsActive())
			{
				bn.SetDefaultColor(rb.GetColor()/*sr.color*/);
			}
		}

		switch(numOfBlocksToGivePoints)
		{
		case 0:
			pointsThisBlock = 0;
			break;
		case 1:
			pointsThisBlock = 1;
			break;
		case 2:
			pointsThisBlock = 2;
			break;
		case 3:
			pointsThisBlock = 4;
			break;
		case 4:
			pointsThisBlock = 8;
			break;
		case 5:
			pointsThisBlock = 16;
			break;
		case 6:
			pointsThisBlock = 32;
			break;
		case 7:
			pointsThisBlock = 64;
			break;
		case 8:
			pointsThisBlock = 128;
			break;
		case 9:
			pointsThisBlock = 256;
			break;
		case 10:
			pointsThisBlock = 512;
			break;
		case 11:
			pointsThisBlock = 1024;
			break;
		case 12:
			pointsThisBlock = 1337;
			break;
		}

		if(numOfBlocksToGivePoints > 0)
		{
			popupEnabled = true;
			pointsPopup.guiText.enabled = true;
			popupAlpha = 1.0f;
			pointsPopup.guiText.text = "+" + pointsThisBlock;

			AudioSource milestoneSound = milestoneSounds[Random.Range(0, milestoneSounds.Length)];
			milestoneSound.Play();
		}

		playerPoints += pointsThisBlock;

		drawCounter.guiText.text = "TREKK: " + draws;
		pointsText.guiText.text = "POENG: " + playerPoints;
		rb.GetNextBlockInfo();
		placeSound.Play();
	}

	void Move(int x, int y)
	{
		// List of new nodes to be occupied
		List<GameObject> nodesToBeOccupied = new List<GameObject>();

		foreach(GameObject node in occupiedNodes)
		{
			BoardNode bn = node.GetComponent<BoardNode>();
			currentX = (int)bn.GetPosition().x;
			currentY = (int)bn.GetPosition().y;
			int newX = currentX + x;
			int newY = currentY + y;

			// Check if the block can be moved in desired direction, else don't move block at all
			if(newX < 19 && newX >= 0
			   && newY < 10 && newY >= 0)
			{
				nodesToBeOccupied.Add(nodeArray[newX, newY]);
				nodeArray[newX, newY].GetComponent<BoardNode>().Select();
			}
			else
			{
				if(canPlayBlocked)
				{
					blockedSound.Play();
					canPlayBlocked = false;
				}

				return;
			}
		}

		// Unselect "old" nodes
		foreach(GameObject node in occupiedNodes)
		{
			if(!nodesToBeOccupied.Contains(node))
			{
				node.GetComponent<BoardNode>().UnSelect();
			}
		}

		// Set the new List of nodes that are occupied
		occupiedNodes = nodesToBeOccupied;
		moveSound.Play();
	}

	void Spawn()
	{
		occupiedNodes.Clear();

		foreach(GameObject node in nodeArray)
		{
			node.GetComponent<BoardNode>().UnSelect();
		}

		blockSize = rb.GetShape();

		// "Spawn" block by setting which nodes to occupy
		for(int i = 0; i < blockSize.x; i++)
		{
			for(int j = 0; j < blockSize.y; j++)
			{
				occupiedNodes.Add(nodeArray[startX + i, startY + j]);
				nodeArray[startX + i, startY + j].GetComponent<BoardNode>().Select();
			}
		}

		if(rb.IsCornered())
		{
			occupiedNodes.Remove(nodeArray[startX + 0, startY + 2]);
			occupiedNodes.Remove(nodeArray[startX + 1, startY + 2]);
			occupiedNodes.Remove(nodeArray[startX + 0, startY + 3]);
			occupiedNodes.Remove(nodeArray[startX + 1, startY + 3]);
			nodeArray[startX + 0, startY + 2].GetComponent<BoardNode>().UnSelect();
			nodeArray[startX + 1, startY + 2].GetComponent<BoardNode>().UnSelect();
			nodeArray[startX + 0, startY + 3].GetComponent<BoardNode>().UnSelect();
			nodeArray[startX + 1, startY + 3].GetComponent<BoardNode>().UnSelect();
		}

	}

	void Rotate()
	{
		if(rb.GetShape() == new Vector2(2, 2))
			return;

		GameObject pivotNode = occupiedNodes[0];
		Vector2 pivotNodePosition = pivotNode.GetComponent<BoardNode>().GetPosition();

		List<GameObject> nodesToBeOccupied = new List<GameObject>();

		// Calculate new nodes to be occupied (except pivotNode)
		foreach(GameObject node in occupiedNodes)
		{
			if(node != pivotNode)
			{
				Vector2 nodePosition = node.GetComponent<BoardNode>().GetPosition();
				int newX = 0;
				int newY = 0;

				// Calculate new position of node (node to be selected after rotation)
				if((nodePosition.x > pivotNodePosition.x && nodePosition.y == pivotNodePosition.y)
				   || (nodePosition.x < pivotNodePosition.x && nodePosition.y == pivotNodePosition.y))
				{
					newX = (int)pivotNodePosition.x;
					newY = (int)(pivotNodePosition.y + (nodePosition.x - pivotNodePosition.x));
				}
				else if((nodePosition.x > pivotNodePosition.x && nodePosition.y > pivotNodePosition.y)
				        || (nodePosition.x < pivotNodePosition.x && nodePosition.y > pivotNodePosition.y)
				        || (nodePosition.x > pivotNodePosition.x && nodePosition.y < pivotNodePosition.y)
				        || (nodePosition.x < pivotNodePosition.x && nodePosition.y < pivotNodePosition.y))
				{
					newX = (int)(pivotNodePosition.x - (nodePosition.y - pivotNodePosition.y));
					newY = (int)(pivotNodePosition.y + (nodePosition.x - pivotNodePosition.x));
				}
				else if((nodePosition.y > pivotNodePosition.y && nodePosition.x == pivotNodePosition.x)
				        || (nodePosition.y < pivotNodePosition.y && nodePosition.x == pivotNodePosition.x))
				{
					newX = (int)(pivotNodePosition.x - (nodePosition.y - pivotNodePosition.y));
					newY = (int)pivotNodePosition.y;
				}

				// Check if node will be out of bounds after rotating
				if(newX < 19 && newX >= 0
				   && newY < 10 && newY >= 0)
				{
					nodesToBeOccupied.Add(nodeArray[newX, newY]);
				}
				else
				{
					//print (node.name + " OUT OF BOUNDS! " + new Vector2(newX, newY));
					blockedSound.Play();
					return;
				}
			}
		}

		// Unselect "old" nodes (except pivotNode)
		foreach(GameObject node in occupiedNodes)
		{
			if(node != pivotNode)
			{
				if(!nodesToBeOccupied.Contains(node))
				{
					node.GetComponent<BoardNode>().UnSelect();
				}
			}
		}
		
		// Clear and set the new List of nodes that are occupied
		occupiedNodes.Clear();
		occupiedNodes.Add(pivotNode);

		foreach(GameObject node in nodesToBeOccupied)
		{
			occupiedNodes.Add(node);
		}

		// Select() all newly occupied nodes
		foreach(GameObject node in occupiedNodes)
		{
			node.GetComponent<BoardNode>().Select();
		}

		rotateSound.Play();
	}

	public Vector2 GetSize()
	{
		return blockSize;
	}
}
