using UnityEngine;
using System.Collections;

public class LoseScreen : MonoBehaviour {

	public string mainMenu;
	public string leaderBoard;

	// Update is called once per frame
	void Update () {
	
	}

	public void Quit() {
		Application.LoadLevel (mainMenu);
	}

	public void submitScore() {
		Application.LoadLevel (leaderBoard);
	}
}
