using UnityEngine;
using System.Collections;

public class GoButtonScript : MonoBehaviour {
	
	//this script controls the behaviors of this particular button. this script can be augmented and copied to do the behaviors of most GUI elements
	
	//here are the variables for this script
	Renderer rend;

	public GameObject masterCont;
	
	public Material clickedMat;
	public Material regMat;

	public Color nativeCol;
	public Color fadedColor;
	
	Vector3 mousePos;
	Vector3 fwd;
	
	RaycastHit hit;
	Ray ray;
	
	public Camera guiCam;
	
	MasterControl mc;
	
	// Use this for initialization
	void Start () {
		rend = this.GetComponent<Renderer> ();

		fwd = new Vector3 (0,0,10);

		mc = masterCont.GetComponent<MasterControl>();

		nativeCol = rend.material.color;
		fadedColor = new Color (nativeCol.r, nativeCol.g, nativeCol.b, (nativeCol.a / 2));
		
		//		screenPos = new Vector2()
	}
	
	// Update is called once per frame
	void Update () {
		
		mousePos = Input.mousePosition;

		mousePos = guiCam.ScreenToWorldPoint (mousePos);
		
		//		if (Physics.Raycast (mousePos, fwd, out hit)) {
		//
		//		}
		
		ray = guiCam.ScreenPointToRay (Input.mousePosition);
		if (mc.checkState (MasterControl.GameStates.gs_play)) {
			rend.material.color = nativeCol;
			if (Physics.Raycast (ray, out hit, 10)) {
				if (hit.collider.tag == "GUI") {
					if (hit.collider.name.Equals ("GO_Button")) {
						//					Debug.Log ("WHOOP WHOOP WHOOP!!");
						if (Input.GetMouseButton (0)) {
							//						Debug.Log("THIS HORSE IS ON FIYAAAAAAH!");
							rend.material = clickedMat;
						}
					
						if (Input.GetMouseButtonUp (0)) {
							rend.material = regMat;
							mc.changeState (MasterControl.GameStates.gs_skippyMove);
						}
					}
				}
			}
		} else {
			rend.material.color = fadedColor;
		}
	}
}
