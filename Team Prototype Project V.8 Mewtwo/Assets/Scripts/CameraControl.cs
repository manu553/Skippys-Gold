using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{

	/**this script is going to alter the camera behavior so it resizes to fit the Grid's width
	 * It wil also control the Camera's aspect ratio to be adaptable to all phone sizes. 
	 * in the future it might be able to negate phone rotations, assuming this isn't standard in unity android projects.
	 * We should consider a second camera for the GUI elements. 
	**/

	//here we intialize the camera variable so we can alter the camera.
	Camera cam; //the first camera a new scene opens with is automatically the main camera

	//Here we initialize the float variables for the script
	//this is the target aspect our camera will, eventually, aim for to determine if the camera needs letterboxing
	private float aspectTarget = 9.0f / 16.0f;
//	private float gridCenter;
	private float gridWidth;

	//these are the vector variables for this code
	private Vector2 gridCenter;
	private Vector3 gridCenter3;

	[SerializeField] private Plane[] planes;

	//here we intialize the scripting references for our code
	MakeGrid mg;

	// Use this for initialization
	void Start ()
	{
		mg = GameObject.Find ("GridMaster").GetComponent<MakeGrid> ();
		cam = Camera.main;
		planes = GeometryUtility.CalculateFrustumPlanes (cam);
	
	}
	
	// Update is called once per frame
	void Update ()
	{
//		Debug.Log (cam.orthographicSize);
		if (Input.GetKey (KeyCode.C)) {
			increaseCam();
		}
	}

	void LateUpdate(){
//		Debug.Log ("Let me set you on fire, sir");
		if(mg.gridObjects.Count > 0){
//			Debug.Log("We are counting sir");
			gridCenter = generateCenter ();
			gridCenter3 = new Vector3(gridCenter.x, gridCenter.y, this.transform.position.z);
			cam.transform.position = gridCenter3;
		}
	}

	private Vector2 generateCenter ()
	{
		Vector2 center = new Vector2 (0, 0);
		//here we have to generate the X center
		GameObject obj1 = mg.getGridUnit (0, 0);
		GameObject obj2 = mg.getGridUnit (mg.getXMax () - 1, mg.getYMax() - 1);

		float XDiff = obj2.transform.position.x - obj1.transform.position.x;
		float YDiff = obj2.transform.position.y - obj1.transform.position.y;
//		Debug.Log ("OBJ2 " + obj2.transform.position + " OBJ1 " + obj1.transform.position);


		XDiff /= 2;
		YDiff /= 2;

		center = new Vector2 (XDiff, YDiff);

//		Debug.Log ("CENTER " + center);
//		float Xcenter = XDiff + obj1.transform.position.x;
		//here we generate the Y center
		

//		gridCenter

		return center;
	}

	//this code is designed to resize the camera width and height in order to keep the whole grid in sight.
	//this will grab the vector2 of the furthest grid objects, and then calculate how much the camera must be resized
	public void increaseCam(){

		float camSize = 4.4f;
		int currentXMax = mg.getXMax ();
		int extra = currentXMax - 5;
		camSize += 0.9f * extra;

		cam.orthographicSize = camSize;



	}


}
