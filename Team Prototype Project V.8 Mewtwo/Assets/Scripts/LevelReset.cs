using UnityEngine;
using System.Collections;

public class LevelReset : MonoBehaviour {

	//these are the variables for this code
	//these are the int variables for this script
	public int gridMax = 9;

	private int countIssue = 0;

	//these are the game object variables for this code
	//this GO is the object that creates and handles the grid. We require it for reference.
	private static GameObject gridMaster;

	//These are the reference objects for the scripts we need to use
	private MakeGrid mg;
	private PathAlgorithm pa;

	Camera cam;

	CameraControl cc;

	// Use this for initialization
	void Start () {
		gridMaster = GameObject.Find("GridMaster");
		mg = gridMaster.GetComponent<MakeGrid>();
		pa = gridMaster.GetComponent<PathAlgorithm>();
		cam = Camera.main;
		cc = cam.GetComponent<CameraControl> ();

		//here we just want to check to make sure our assignments are good
		if (mg == null) {
			Debug.LogError("Sir, your reference to MAKEGRID is invalid.");
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void NextLevel(){
		//this code will increase the level of the game, and re-create the grid accordingly
		//we only check to see if X has met the grid max, because Y should be the same number
		//unless the code is edited elsewhere. in which case there's an issue.
		Debug.Log ("We have entered the NextLevel method, sir " + countIssue++);
		if(gridMax > mg.getXMax()){
			mg.setXMax(mg.getXMax()+1);
			mg.setYMax(mg.getYMax()+1);
//			Debug.Log("We have entered the if statement, sir");
		}
		mg.CreateGrid ();
//		pa.GeneratePath ();
	}
}
