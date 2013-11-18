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

	// Use this for initialization
	void Start () 
	{
		// Set the color of this block
		renderer.material.color = colors[Random.Range(0, colors.Length)];

		// Get the array of nodes that are on the board
		nodeArray = GameObject.Find("board").GetComponent<DivideBoardIntoNodes>().GetNodes();

		// "Spawn" block by setting which nodes to occupy
		for(int i = 0; i < blockSize.x; i++)
		{
			for(int j = 0; j < blockSize.y; j++)
			{
				occupiedNodes.Add(nodeArray[i, j]);
				nodeArray[i, j].GetComponent<BoardNode>().Select();
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			transform.Rotate(new Vector3(0, 0, 90));
		}

		if(Input.GetKeyDown(KeyCode.Return))
		{
			PlaceBlock();
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

			if(bn.GetDefaultColor() == renderer.material.color)
			{
				bn.Deactivate();
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
				nodesToBeOccupied.Add(nodeArray[currentX + x, currentY + y]);
				nodeArray[currentX + x, currentY + y].GetComponent<BoardNode>().Select();
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

	public Vector2 GetSize()
	{
		return blockSize;
	}
}
