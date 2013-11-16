using UnityEngine;
using System.Collections.Generic;

public class DivideBoardIntoNodes : MonoBehaviour 
{
	public GameObject boardNode;
	public Sprite boardSprite;

	float nodeSizeX;
	float nodeSizeY;
	GameObject thisNode;

	int selectionX = 1;
	int selectionY = 1;
	int maxX = 19;
	int maxY = 10;

	Color selectedColor = Color.blue;
	Color normalColor = Color.white;

	GameObject selectedNode;
	GameObject[,] nodeArray = new GameObject[20, 11];

	// Use this for initialization
	void Start () 
	{
		boardSprite = GetComponent<SpriteRenderer>().sprite;

		nodeSizeX = boardSprite.bounds.size.x / 19;
		nodeSizeY = boardSprite.bounds.size.y / 10;

		//print ("x = " + nodeSizeX + " | y = " + nodeSizeY);
		for(int i = 0; i < 19; i++)
		{
			for(int j = 0; j < 10; j++)
			{
				thisNode = Instantiate(boardNode, new Vector3(transform.position.x + (i * nodeSizeX), transform.position.y - (j * nodeSizeY), 0), Quaternion.identity) as GameObject;
				thisNode.name = "bNode(" + (i + 1) + ", " + (j + 1) + ")";
				thisNode.transform.parent = gameObject.transform;
				nodeArray[i+1, j+1] = thisNode;
			}
		}

		selectedNode = nodeArray[selectionX, selectionY];
		selectedNode.renderer.material.color = selectedColor;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.W))
		{
			TraverseNodes(0, -1);
		}

		if(Input.GetKeyDown(KeyCode.A))
		{
			TraverseNodes(-1, 0);
		}

		if(Input.GetKeyDown(KeyCode.S))
		{
			TraverseNodes(0, 1);
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			TraverseNodes(1, 0);
		}

		/*
		if(Input.GetMouseButtonDown(0))
		{
			//RaycastHit2D hit = Physics2D.Raycast(new Vector2(Input.mousePosition.x, Input.mousePosition.y), -Vector2.up);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			//Ray2D ray = Ray2D(new Vector2(Input.mousePosition.x, Input.mousePosition.y), -Vector2.up);
			if(Physics.Raycast(ray.origin, Vector3.forward, out hit))
			{
				hit.transform.gameObject.renderer.material.color = Color.blue;
				print(hit.transform.gameObject.name.ToString());
			}
		}
		*/
	}

	void TraverseNodes(int addX, int addY)
	{
		if(selectionX + addX <= maxX && selectionY + addY <= maxY
		   && selectionX + addX >= 1 && selectionY + addY >= 1)
		{
			selectedNode.renderer.material.color = normalColor;
			selectedNode = nodeArray[selectionX += addX, selectionY += addY];
			selectedNode.renderer.material.color = selectedColor;
		}
	}
}