using UnityEngine;
using System.Collections.Generic;

public class DivideBoardIntoNodes : MonoBehaviour 
{
	public GameObject boardNode;
	public Sprite boardSprite;

	float nodeSizeX;
	float nodeSizeY;
	GameObject thisNode;

	int startNodeX = 0;
	int startNodeY = 0;

	int maxNodesX = 19;
	int maxNodesY = 10;

	//List<GameObject> selectedNodesList = new List<GameObject>();
	GameObject[,] nodeArray = new GameObject[19, 10];

	// Use this for initialization
	void Start () 
	{
		boardSprite = GetComponent<SpriteRenderer>().sprite;

		nodeSizeX = boardSprite.bounds.size.x / maxNodesX;
		nodeSizeY = boardSprite.bounds.size.y / maxNodesY;

		//print ("x = " + nodeSizeX + " | y = " + nodeSizeY);
		for(int i = 0; i < maxNodesX; i++)
		{
			for(int j = 0; j < maxNodesY; j++)
			{
				thisNode = Instantiate(boardNode, new Vector3(transform.position.x + (i * nodeSizeX), transform.position.y - (j * nodeSizeY), 0), Quaternion.identity) as GameObject;
				thisNode.name = "bNode(" + (i) + ", " + (j) + ")";
				thisNode.transform.parent = gameObject.transform;
				nodeArray[i, j] = thisNode;
				nodeArray[i, j].GetComponent<BoardNode>().SetPosition(i, j);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public GameObject[,] GetNodes()
	{
		return nodeArray;
	}
	
	public List<GameObject> GetSelectedNodes(Vector2 size)
	{
		List<GameObject> selectedNodesList = new List<GameObject>();

		for(int i = 0; i < size.x; i++)
		{
			for(int j = 0; j < size.y; j++)
			{
				selectedNodesList.Add(nodeArray[startNodeX + i, startNodeY + j]);

				//BoardNode thisNode = nodeArray[startNodeX + i, startNodeY + j].GetComponent<BoardNode>();

				//thisNode.Select();
				//selectedNodesList.Add(thisNode.gameObject);
			}
		}

		return selectedNodesList;
	}

	/*
	public void TraverseNodes(int addX, int addY, Vector2 size)
	{
		for(int i = 0; i < maxNodesX; i++)
		{
			for(int j = 0; j < maxNodesY; j++)
			{
				BoardNode thisNode = nodeArray[i, j].GetComponent<BoardNode>();

				if(thisNode.IsSelected())
				{
					if(i + addX < maxNodesX)
					{
						thisNode.UnSelect();
						selectedNodesList.Add(nodeArray[i + addX, j + addY]);
						selectedNodesList.Remove(thisNode.gameObject);
					}
				}
			}
		}

		foreach(GameObject node in selectedNodesList)
		{
			node.GetComponent<BoardNode>().Select();
		}
		*/

		/*
		if(selectionX + addX <= maxX && selectionY + addY <= maxY
		   && selectionX + addX >= 1 && selectionY + addY >= 1)
		{
			selectedNode.renderer.material.color = normalColor;
			selectedNode = nodeArray[selectionX += addX, selectionY += addY];
			selectedNode.renderer.material.color = selectedColor;
		}

	}
	*/
}