using UnityEngine;
using System.Collections;

public class IntroSceneScript : MonoBehaviour 
{
	public MovieTexture movie;
	public AudioSource audio;

	// Use this for initialization
	void Start () 
	{
		guiTexture.texture = movie;
		audio.clip = movie.audioClip;
		movie.Play();
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(movie.isPlaying)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				movie.Stop();
				audio.Stop();
			}
		}
		else
		{
			Application.LoadLevel(1);
		}
	}
}
