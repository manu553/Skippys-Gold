using UnityEngine;
using System.Collections;

public class PlayButtonScript : MonoBehaviour {
	
	//this script controls the behaviors of this particular button. this script can be augmented and copied to do the behaviors of most GUI elements
	
	//here are the variables for this script
	Renderer rend;

	public Material clickedMat;
	public Material regMat;
	
	Vector2 nativePos;
	Vector2 screenPos;
	
	float nativeSizeX;
	float nativeSizeY;
	float hoverSizeInc = 1.1f;
	
	bool isClicked = false;
	
	Vector3 mousePos;
	Vector3 fwd;
	
	RaycastHit hit;
	Ray ray;
	
	public Camera guiCam;
	
	MasterControl mc;
	
	// Use this for initialization
	void Start () {
		rend = this.GetComponent<Renderer> ();
		
		//		nativePos = this.transform.position;

		guiCam = Camera.main;
		
		nativeSizeX = rend.bounds.size.x;
		nativeSizeY = rend.bounds.size.y;
		fwd = new Vector3 (0,0,10);

		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
		
		//		screenPos = new Vector2()
	}
	
	// Update is called once per frame
	void Update () {
		
		mousePos = Input.mousePosition;
		
		mousePos = guiCam.ScreenToWorldPoint (mousePos);
		
		//		if (Physics.Raycast (mousePos, fwd, out hit)) {
		//
		//		}
		
		ray = guiCam.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 10)) {
			if(hit.collider.tag == "GUI"){
				if(hit.collider.name.Equals("PlayButton")){
					//					Debug.Log ("WHOOP WHOOP WHOOP!!");
					if(Input.GetMouseButton(0)){
						//						Debug.Log("THIS HORSE IS ON FIYAAAAAAH!");
						rend.material = clickedMat;
					}
					
					if(Input.GetMouseButtonUp(0)){
						rend.material = regMat;
						Application.LoadLevel(1);
					}
				}
			}
		}
	}
}
