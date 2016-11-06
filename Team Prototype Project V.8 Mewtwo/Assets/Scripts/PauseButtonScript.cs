using UnityEngine;
using System.Collections;

public class PauseButtonScript: MonoBehaviour
{
	
	//this script controls the behaviors of this particular button. this script can be augmented and copied to do the behaviors of most GUI elements
	
	//here are the variables for this script
	bool isPaused = false;

	Renderer rend;
	Renderer resRend;
	Renderer quitRend;

	public GameObject masterCont;
	public GameObject pauseScreen;
	public GameObject resumeButt;
	public GameObject quitButt;

	public Material clickedMat;
	public Material regMat;
	public Material[] resumeMat = new Material[2];
	public Material[] quitMat = new Material[2];

	Vector3 mousePos;
	Vector3 fwd;

	RaycastHit hit;
	Ray ray;

	public Camera guiCam;

	MasterControl mc;
	PauseMenu pm;
	
	// Use this for initialization
	void Start ()
	{
		quitRend = quitButt.GetComponent<Renderer> ();
		resRend = resumeButt.GetComponent<Renderer> ();
		rend = this.GetComponent<Renderer> ();

		fwd = new Vector3 (0, 0, 10);
		
		mc = masterCont.GetComponent<MasterControl> ();

		pm = guiCam.GetComponent<PauseMenu> ();
		
		//		screenPos = new Vector2()
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		mousePos = Input.mousePosition;
		
		mousePos = guiCam.ScreenToWorldPoint (mousePos);
		
		//		if (Physics.Raycast (mousePos, fwd, out hit)) {
		//
		//		}
		
		ray = guiCam.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 10)) {
			if (hit.collider.tag == "GUI") {
				if (hit.collider.name.Equals ("PAUSE_Button")) {
					//					Debug.Log ("WHOOP WHOOP WHOOP!!");
					if (Input.GetMouseButton (0)) {
						//						Debug.Log("THIS HORSE IS ON FIYAAAAAAH!");
						rend.material = clickedMat;
					}
					
					if (Input.GetMouseButtonUp (0)) {
						isPaused = true;
						pm.setPause (isPaused);
//						rend.material = regMat;
//						mc.changeState(MasterControl.GameStates.gs_skippyMove);
					}
				} else if (hit.collider.name.Equals ("Resume_Button")) {
					if (Input.GetMouseButton (0)) {
						resRend.material = resumeMat [0];
					} else if (Input.GetMouseButtonUp (0)) {
						resRend.material = resumeMat [1];
						pm.Resume ();
					}
				} else if (hit.collider.name.Equals ("MainMenu_Button")) {
					if (Input.GetMouseButton (0)) {
						quitRend.material = quitMat [0];
					} else if (Input.GetMouseButtonUp (0)) {
						quitRend.material = quitMat [1];
						pm.Quit ();
					}
				}
			}
		}

		if (isPaused) {
			rend.material = clickedMat;
		} else {
			rend.material = regMat;
		}
	}

	public void setPause (bool isItPaused)
	{
		isPaused = isItPaused;
	}
}

