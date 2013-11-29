using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomTextStartScript : MonoBehaviour 
{
	public AudioSource audio;
	public GUIText text;

	float alpha = 0.0f;
	Color selectedColor;

	public List<Color> colorsList = new List<Color>();
	public List<string> textList = new List<string>();

	// Use this for initialization
	void Start () 
	{
		text.text = textList[Random.Range(0, textList.Count)];
		selectedColor = colorsList[Random.Range(0, colorsList.Count)];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(alpha < 1f)
		{
			alpha += Time.deltaTime / 4f;
		}
		text.color = Color.Lerp(Color.black, selectedColor, alpha);

		if(!audio.isPlaying)
		{
			Application.LoadLevel(2);
		}
	}
}
