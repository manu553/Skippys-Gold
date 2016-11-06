using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	public GameObject leaderButton;
	public GameObject mainButton;

	public Material[] leaderMat = new Material[2];
	public Material[] mainMat = new Material[2];

	RaycastHit hit;

	Ray ray;

	// Use this for initialization
	void Start () {
	
		if (!leaderButton) {
			Debug.LogError("Your leader button is missing, sir");
		}

		if (!mainButton) {
			Debug.LogError("Your main Button is missing, sir");
		}

		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;

	}
	
	// Update is called once per frame
	void Update () {

		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

//		Debug.Log ("AAAAAAAAAAA");
		if (Physics.Raycast (ray, out hit, 10)) {
//			Debug.Log ("CCCCCCCCCCCCCCC");
			if(hit.collider.tag.Equals("GUI")){
//				Debug.Log ("BBBBBBBBBB");
				if(hit.collider.name == "LeaderBoard_Button"){
					if(Input.GetMouseButton(0)){
						setState(true, leaderButton, leaderMat);
					}else if(Input.GetMouseButtonUp(0)){
						Application.LoadLevel(2);
						setState(false, leaderButton, leaderMat);
					}
				}
				if(hit.collider.name == "MainMenu_Button"){
//					Debug.Log ("CCCCCCCCCCCCCCC");
					if(Input.GetMouseButton(0)){
						setState(true, mainButton, mainMat);
					}else if(Input.GetMouseButtonUp(0)){
						Application.LoadLevel(0);
						setState(false, mainButton, mainMat);
					}
				}
			}
		}
	}

	void setState(bool clicked, GameObject button, Material[] mat){
		if (clicked) {
			button.GetComponent<Renderer> ().material = mat [1];
		} else {
			button.GetComponent<Renderer>().material = mat[0];
		}
	}
}
