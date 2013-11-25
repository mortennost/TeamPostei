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
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Rotate();
		}

		if(Input.GetKeyDown(KeyCode.Return))
		{
			PlaceBlock();
			rb.GetNextBlockInfo();
			Spawn();
		}

		if(Input.GetKeyDown(KeyCode.W))
		{
			Move(0, -1);
		}
		
		if(Input.GetKeyDown(KeyCode.A))
		{
			Move(-1, 0);
		}
		
		if(Input.GetKeyDown(KeyCode.S))
		{
			Move(0, 1);
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			Move(1, 0);
		}
	}

	void PlaceBlock()
	{
		foreach(GameObject node in occupiedNodes)
		{
			BoardNode bn = node.GetComponent<BoardNode>();

			if(bn.GetDefaultColor() == sr.color)
			{
				bn.Deactivate();
			}
			else if(bn.IsActive())
			{
				bn.SetDefaultColor(sr.color);
			}
		}
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
