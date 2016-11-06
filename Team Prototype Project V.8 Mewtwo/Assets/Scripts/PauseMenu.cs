using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public string mainMenu;
	public bool isPaused;

	public GameObject pauseMenuCanvas;
	public GameObject pauseButton;

	PauseButtonScript pbs;

	void Start(){
		pbs = pauseButton.GetComponent<PauseButtonScript>();
	}

	// Update is called once per frame
	void Update () {
	
		if (isPaused) {
			pauseMenuCanvas.SetActive (true);
			Time.timeScale = 0f;
		} else {
			pauseMenuCanvas.SetActive (false);
			Time.timeScale = 1f;
		}
//
//		if (Input.GetKeyDown (KeyCode.Escape)) {
//			isPaused = !isPaused;
//			pbs.setPause(isPaused);
//		}
	}

	public void Resume() {
		isPaused = false;
		pbs.setPause(isPaused);
	}

	public void Quit() {
		Application.LoadLevel (mainMenu);
	}

	public void setPause(bool pause){
		isPaused = pause;
	}
}
