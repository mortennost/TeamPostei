using UnityEngine;
using System.Collections;

public class RandomizeBlock : MonoBehaviour 
{
	public Color[] blockColor;
	//public Sprite blockType;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			renderer.material.color = blockColor[Random.Range(0, (blockColor.Length))];
		}
	}
}
