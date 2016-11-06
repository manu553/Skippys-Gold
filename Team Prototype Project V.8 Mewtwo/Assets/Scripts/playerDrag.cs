using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerDrag : MonoBehaviour
{

//	public List<GameObject> userPath = new List<GameObject>();

//	Vector2 mousePosition = new Vector2 (0,0);
	Vector3 worldPos;
	int mousePositionX = 0;
	int mousePositionY = 0;
	MakeGrid mg;
	GameObject thisUnit;
	MasterControl mc;

	RaycastHit hit;
	Ray ray;

	void Start ()
	{
		mg = this.GetComponent<MakeGrid> ();
		mc = GameObject.Find ("MasterControl").GetComponent<MasterControl> ();
		thisUnit = this.gameObject;
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Mouse0) && mc.currentState == MasterControl.GameStates.gs_play) {
			pathSelected ();
		}
	}

	public void OnMouseOver ()
	{

	}

	public void pathSelected ()
	{
		GridUnitBehavior gub;

		mousePositionX = (int)Input.mousePosition.x;
		mousePositionY = (int)Input.mousePosition.y;
		
		//		mousePosition = new Vector2 (mousePositionX, mousePositionY);
		worldPos = new Vector3 (mousePositionX, mousePositionY, 0);
		
		worldPos = Camera.main.ScreenToWorldPoint (worldPos);
		
//		Debug.LogWarning (worldPos);

		float fixX = worldPos.x + 0.5f;
		float fixY = worldPos.y - 0.5f;

//		Debug.Log ("FIXX: " + fixX + " FIXY: " + fixY);
//		Debug.Log ("INTFIXX: " + (int)fixX + " INTFIXY: " + (int)fixY);
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 10)) {
//			Debug.Log(hit.collider.tag);
			if (hit.collider.tag.Equals ("GridUnit")) {
				if (fixX >= 0 && -fixY >= 0) {
					if (fixX <= mg.getXMax () && fixY <= mg.getYMax ()) {
						GameObject obj = mg.getGridUnit ((int)fixX, ((int)fixY * (-1)));
						gub = obj.GetComponent<GridUnitBehavior> ();
						GameObject chk = mc.userPath [mc.userPath.Count - 1];
						GridUnitBehavior chkGub = chk.GetComponent<GridUnitBehavior> ();

						int gubX = gub.getX ();
						int gubY = gub.getY ();

						int chkX = chkGub.getX ();
						int chkY = chkGub.getY ();

//			Debug.Log ("The selections is valid within the grid, sir");
//			Debug.Log("GUB.X: " + gub.getX() + " chkGUB.X: " + chkGub.getX());
//			Debug.Log("GUB.Y: " + gub.getY () + " chkGUB.Y: " + chkGub.getY());
						if (gubX == (chkX - 1) || gubX == (chkX + 1)) {
							if (gubY == chkY) {
								mc.userPath.Add (obj);
//				Debug.Log (obj);
//				Debug.Log ("The selection has a valid X value, sir");

								gub.setUserPathState (true);
							}

						} else if (gubY == (chkY - 1) || gubY == (chkY + 1)) {
							if (gubX == chkX) {
								mc.userPath.Add (obj);
								//				Debug.Log (obj);
								//				Debug.Log ("The selection is valid in the Y co-ordinates, sir");
								gub.setUserPathState (true);
							}
				
						} else {
//				Debug.Log ("The selection is invalid, sir");
						}
				
						//Debug.Log("worldX: " + (int)worldPos.x + " worldY: " + (int)worldPos.y + " LALALALALALALA");
					}
				}
			}
		}
			//		}	Debug.LogWarning("HIGHLIGHT");
	}
}
