using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceableBlock : MonoBehaviour 
{
	public GameObject nextBlock;
	public Color[] colors;

	Vector2 blockSize = new Vector2(2f, 2f);

	GameObject[,] nodeArray;
	List<GameObject> occupiedNodes = new List<GameObject>();

	int currentX = 0;
	int currentY = 0;

	SpriteRenderer sr;
	RandomizeBlock rb;

	float timeUntilNextInputUpdate = 0.1f;
	float inputTimer = 0.0f;

	int playerPoints = 0;
	int nextPointMilestone;
	int milestonesHit = 0;

	GameObject pointsText;
	GameObject milestoneText;
	GameObject rewardText;
	GameObject startText;
	SpriteRenderer reward;
	Color rewardColor;
	float rewardAlpha = 255.0f;

	bool showReward = false;
	float timeToShowReward = 5.0f;
	float rewardTimer = 0.0f;

	// Use this for initialization
	void Start () 
	{
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<RandomizeBlock>();
		blockSize = rb.GetShape();

		// Get the array of nodes that are on the board
		nodeArray = GameObject.Find("board").GetComponent<DivideBoardIntoNodes>().GetNodes();

		// "Spawn" block by setting which nodes to occupy
		Spawn();

		pointsText = GameObject.Find("PointsText");
		milestoneText = GameObject.Find("MilestoneText");
		rewardText = GameObject.Find("RewardText");
		startText = GameObject.Find("StartText");
		reward = GameObject.Find("Reward").GetComponent<SpriteRenderer>();
		rewardColor = reward.color;

		nextPointMilestone = 2000;
		pointsText.guiText.text = "Points: " + playerPoints;
		milestoneText.guiText.text = "Next Milestone: " + nextPointMilestone;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!showReward)
		{
			if(playerPoints >= nextPointMilestone)
			{
				milestonesHit++;
				nextPointMilestone += 2000;
				milestoneText.guiText.text = "Next Milestone: " + nextPointMilestone;
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
				}

				PlaceBlock();
				rb.GetNextBlockInfo();
				Spawn();
			}
			
			if(inputTimer >= timeUntilNextInputUpdate)
			{
				inputTimer = 0.0f;
				if(Input.GetKey(KeyCode.W))
				{
					Move(0, -1);
				}
				
				if(Input.GetKey(KeyCode.A))
				{
					Move(-1, 0);
				}
				
				if(Input.GetKey(KeyCode.S))
				{
					Move(0, 1);
				}
				
				if(Input.GetKey(KeyCode.D))
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
					ShowReward(true);
					pointsText.guiText.enabled = false;
					milestoneText.guiText.enabled = false;
					rewardAlpha /= rewardTimer;
					reward.color = new Color(rewardColor.r, rewardColor.g, rewardColor.b, rewardAlpha);
				}
				else
				{
					ShowReward(false);
					showReward = false;
					rewardTimer = 0.0f;
					pointsText.guiText.enabled = true;
					milestoneText.guiText.enabled = true;
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
					rewardText.guiText.text = "Congratulations! You have completed the game! \nPress [ENTER] to start a new game!";
				}

				if(Input.GetKeyDown(KeyCode.Return))
				{
					Application.LoadLevel(0);
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
				rewardText.guiText.text = "Congratulations! You received reward " + milestonesHit + " of 6. \nPrepare for the next round!";
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

		playerPoints += 100 * numOfBlocksToGivePoints;
		pointsText.guiText.text = "Points: " + playerPoints;
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
				occupiedNodes.Add(nodeArray[i, j]);
				nodeArray[i, j].GetComponent<BoardNode>().Select();
			}
		}

		if(rb.IsCornered())
		{
			occupiedNodes.Remove(nodeArray[0, 2]);
			occupiedNodes.Remove(nodeArray[1, 2]);
			occupiedNodes.Remove(nodeArray[0, 3]);
			occupiedNodes.Remove(nodeArray[1, 3]);
			nodeArray[0, 2].GetComponent<BoardNode>().UnSelect();
			nodeArray[1, 2].GetComponent<BoardNode>().UnSelect();
			nodeArray[0, 3].GetComponent<BoardNode>().UnSelect();
			nodeArray[1, 3].GetComponent<BoardNode>().UnSelect();
		}

	}

	void Rotate()
	{
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
	}

	public Vector2 GetSize()
	{
		return blockSize;
	}
}
