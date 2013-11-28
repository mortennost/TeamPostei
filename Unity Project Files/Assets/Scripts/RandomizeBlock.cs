using UnityEngine;
using System.Collections;

public class RandomizeBlock : MonoBehaviour 
{
	public GameObject nextBlock;
	public Color[] colors;
	public Vector2[] shapes;
	public Sprite[] sprites;
	public Sprite[] blueSprites;
	public Sprite[] greenSprites;
	public Sprite[] pinkSprites;
	public Sprite[] redSprites;
	public Sprite[] yellowSprites;

	Color color;

	SpriteRenderer sr;
	
	Vector2 selectedShape;
	bool cornered = false;

	// Use this for initialization
	void Start () 
	{
		sr = GetComponent<SpriteRenderer>();

		color = colors[Random.Range(0, colors.Length)];
		//sr.color = colors[Random.Range(0, colors.Length)];
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

		if(color == colors[0])
			sr.sprite = blueSprites[element];

		else if(color == colors[1])
			sr.sprite = greenSprites[element];

		else if(color == colors[2])
			sr.sprite = pinkSprites[element];

		else if(color == colors[3])
			sr.sprite = redSprites[element];

		else if(color == colors[4])
			sr.sprite = yellowSprites[element];

		//sr.sprite = sprites[element];
	}

	public void GetNextBlockInfo()
	{
		if(nextBlock != null)
		{
			RandomizeBlock next = nextBlock.GetComponent<RandomizeBlock>();
			//sr.color = next.sr.color;
			color = next.color;
			sr.sprite = next.sr.sprite;
			selectedShape = next.selectedShape;
			cornered = next.cornered;
			next.GetNextBlockInfo();
		}
		else
		{
			color = colors[Random.Range(0, colors.Length)];
			SetSprite(Random.Range(0, shapes.Length));

			//SetColor(colors[Random.Range(0, colors.Length)]); 
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

	public void SetColor(Color newColor)
	{
		color = newColor;
	}

	public Color GetColor()
	{
		return color;
	}
}
