using UnityEngine;
using System.Collections;

public class BoardNode : MonoBehaviour 
{
	public Vector2 position = Vector2.zero;
	public Color[] colors;

	bool selected = false;
	bool active = true;

	Color selectedColor = Color.black;
	Color defaultColor;

	void Awake()
	{
		defaultColor = colors[Random.Range(0, colors.Length)];
		SetColor(defaultColor);
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public bool IsActive()
	{
		return active;
	}

	public void Deactivate()
	{
		active = false;
	}

	public bool IsSelected()
	{
		return selected;
	}

	public void Select()
	{
		selected = true;
		SetColor(selectedColor);
		//print ("SELECTED: " + position.x + ", " + position.y);
	}

	public void UnSelect()
	{
		selected = false;

		if(active)
		{
			SetColor(defaultColor);
		}
		else
		{
			SetColor(Color.white);
		}
	}

	public void SetPosition(int x, int y)
	{
		position = new Vector2(x, y);
	}

	public void SetColor(Color color)
	{
		renderer.material.color = color;
	}

	public void SetDefaultColor(Color color)
	{
		defaultColor = color;
	}

	// Gets the default color assigned to this node
	public Color GetDefaultColor()
	{
		return defaultColor;
	}

	public Vector2 GetPosition()
	{
		return position;
	}
}
