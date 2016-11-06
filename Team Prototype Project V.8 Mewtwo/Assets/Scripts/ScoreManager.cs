using UnityEngine;
using System.Collections;
using System;

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
	
public class ScoreManager : MonoBehaviour
{
		
	// static score variable
	public static int score;
	public static int level;
		
	// instance of Text object
	public Text text;
		
	void OnAwake ()
	{
		// accesses Text component
		text = GetComponent<Text> ();
		// sets score to 0
		score = 0;
		level = 1;
	}
		
	// Update is called once per frame
	void Update ()
	{
		// updates the score text
		text.text = "Score: " + score + Environment.NewLine + "Level: " + level;
		PlayerPrefs.SetInt ("score", score);
	}
		
		
	// gets the users score
	public static int getScore ()
	{
		return score;
	}
}

