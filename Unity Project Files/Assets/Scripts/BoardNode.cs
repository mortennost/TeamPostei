using UnityEngine;
using System.Collections;

public class BoardNode : MonoBehaviour 
{
	public Vector2 position = Vector2.zero;
	public Color[] colors;

	bool selected = false;
	bool isActive = true;

	Color selectedColor = Color.black;
	Color defaultColor;

	void Awake()
	{
		defaultColor = colors[Random.Range(0, colors.Length)];
		SetColor(Color.Lerp(Color.white, defaultColor, 0.8f));
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
		return isActive;
	}

	public void Deactivate()
	{
		isActive = false;
	}

	public bool IsSelected()
	{
		return selected;
	}

	public void Select()
	{
		selected = true;
		SetColor(Color.Lerp(selectedColor, defaultColor, 0.6f));
		//print ("SELECTED: " + position.x + ", " + position.y);
	}

	public void UnSelect()
	{
		selected = false;
		SetColor(Color.Lerp(Color.white, defaultColor, 0.8f));

		/*if(isActive)
		{
			SetColor(defaultColor);
		}
		else
		{
			SetColor(Color.white);
		}*/
	}

	public void SetPosition(int x, int y)
	{
		position = new Vector2(x, y);
	}

	public void SetColor(Color color)
	{
		/*renderer.material.color = new Color(Mathf.Lerp(renderer.material.color.r, color.r, 0.3f),
		                                    Mathf.Lerp(renderer.material.color.g, color.g, 0.3f),
		                                    Mathf.Lerp(renderer.material.color.b, color.b, 0.3f),
		                                    Mathf.Lerp(renderer.material.color.a, color.a, 0.3f));*/

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
