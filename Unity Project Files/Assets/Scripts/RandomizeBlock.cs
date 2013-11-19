using UnityEngine;
using System.Collections;

public class RandomizeBlock : MonoBehaviour 
{
	public GameObject nextBlock;
	public Color[] colors;
	public Vector2[] shapes;
	public Sprite[] sprites;

	SpriteRenderer sr;
	
	Vector2 selectedShape;
	bool cornered = false;

	// Use this for initialization
	void Start () 
	{
		sr = GetComponent<SpriteRenderer>();

		sr.color = colors[Random.Range(0, colors.Length)];
		SetSprite(Random.Range(0, shapes.Length));
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void SetSprite(int element)
	{
		cornered = false;

		if(element == 9)
		{
			cornered = true;
			//sr.sprite = sprites[element];
			//selectedShape = shapes[element];
			//return;
		}

		selectedShape = shapes[element];
		sr.sprite = sprites[element];
	}

	public void GetNextBlockInfo()
	{
		if(nextBlock != null)
		{
			RandomizeBlock next = nextBlock.GetComponent<RandomizeBlock>();
			sr.color = next.sr.color;
			sr.sprite = next.sr.sprite;
			selectedShape = next.selectedShape;
			cornered = next.cornered;
			next.GetNextBlockInfo();
		}
		else
		{
			SetSprite(Random.Range(0, shapes.Length));
			sr.color = colors[Random.Range(0, colors.Length)]; 
		}
	}

	public bool IsCornered()
	{
		return cornered;
	}

	public Vector2 GetShape()
	{
		return selectedShape;
	}
}
