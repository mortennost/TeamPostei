using UnityEngine;
using System.Collections;

public class BrokeTheGameScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		guiText.text = "OH DAYUM!! \nYOU JUST \"BROKE\" THE GAME! \nNOW... START OVER! \n\n[SPACE] for å laste ned vinnerbilde. \n[ESCAPE] for å starte på nytt";
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel(0);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Application.OpenURL("http://mattisdelerud.com/nothingOfTheKind/gratulererDuKlarteDetJo1920.jpg");
		}
	}
}
