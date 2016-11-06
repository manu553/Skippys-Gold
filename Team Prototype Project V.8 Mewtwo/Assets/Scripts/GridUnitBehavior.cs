using UnityEngine;
using System.Collections;

public class GridUnitBehavior : MonoBehaviour {

	//this is the coordinates for the units position in the grid
	private int xGridPos = 0;
	private int yGridPos = 0;
	public int pathPos;

	//these are the gameobjects for this script
	private GameObject thisUnit;
	public GameObject overlayUnit;
	public GameObject sparkleGO;
	//these are the units next to this unit
//	private GameObject leftUnit;
//	private GameObject rightUnit;
//	private GameObject topUnit;
//	private GameObject belowUnit;

	//these are the booleans for this script
	public bool isWall = false;
	public bool isPath = false; //if it's not a path, it's a pit
	public bool isSelectedPath = false;
	public bool isStart = false;
	public bool isEnd = false;
	
	//these are the renderer's for this script
	private Renderer rend;
	private Renderer overRend;

	//these are the material objects for the units
	public Material pitMat;
	public Material pathMat;
	public Material startMat;
	public Material endMat;

	public Material selectedPathMat;

//	private ParticleSystem sparkle;

	// AudioSource instance					
	public AudioSource aSource;	//Alex's audio code
	
	// Use this for initialization
	void Start () {
		thisUnit = this.gameObject;
//		overlayUnit = this.GetComponentInChildren<GameObject> ();

		rend = thisUnit.GetComponent<Renderer> ();
		overRend = overlayUnit.GetComponent<Renderer> ();
		overRend.enabled = false;

		// initializes sound
		InitSoundBoop(); //Alex's Audio Code

		sparkleGO = GameObject.Find ("CAVE_END");

//		sparkle = sparkleGO.GetComponentInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		setState(isPath);
		setUserPathState (isSelectedPath);
		if (!isPath) {
			rend.material = pitMat;
		}

		activateOverlay ();//isSelectedPath);
	}

	//this sets the grid Unit's X position
	public void setX(int xPos){
		xGridPos = xPos;
	}

	//this returns the grid Units Y position
	public int getX(){
		return xGridPos;
	}

	//this sets the Units Y position
	public void setY(int yPos){
		yGridPos = yPos;
	}

	//this returns the Y position
	public int getY(){
		return yGridPos;
	}

	//this function will allow us to change the units state
	public void setState(bool path){
		if (path) {
			this.isPath = true;
		} else {
			this.isPath = false;
		}
		//setMaterial ();
	}

	public void setIsWall(bool totallyWall){
		if (totallyWall) {
			isWall = true;
		} else {
			isWall = false;
		}
	}

	public void setUserPathState(bool path) {
		if (path) {
			this.isSelectedPath = true;
		} else {
			this.isSelectedPath = false;
		}
		
		activateOverlay ();//isSelectedPath);
	}

	public void setMaterial(){
//		if (this.isPath && this.isWall) {
////			rend.material = wallPathMat;
		if (this.isPath) {
			rend.material = pathMat;
//		} else if (this.isWall) {
////			rend.material = wallMat;
		} else {
			rend.material = pitMat;
		}
	}

	public void setToPitMat(){
		rend.material = pitMat;
	}

	private void activateOverlay(){//bool isOn) {//used to be set selected material
//		if (this.isSelectedPath) {
		overRend = overlayUnit.GetComponent<Renderer> ();
//			rend.material = selectedPathMat;
			overRend.enabled = isSelectedPath;
		 if (isEnd) {
//			overRend.material = endMat;
//			overRend.enabled = isEnd;
			sparkleGO.transform.position = this.transform.position;

//			sparkle.enableEmission = true;
		}
//		}
	}
	
	//AING changes object to pit material
	public void fadeMaterial(){
		if (rend == null) {
			rend = this.gameObject.GetComponent<Renderer>();
		}
		if(rend != null){
			rend.material = pitMat;
		} else {
//			Debug.Log ("I have failed you, my sire. " + this.name);
		}
	}

	//added in by Manu
	//This checks if the player has clicked on the path or off the path
	//A message will appear in the console and change if clicked on or off
	//the path.
	void OnMouseUp() {
//		if (this.isSelectedPath == true && this.isPath == true) {
////			Debug.Log ("Yay you didn't fall into a bottomless pit and die a horriable death");
//		} else if (this.isPath == false) {
////			Debug.Log ("You fell into a bottomless pit and died a horrible death, GG");
//			
//		}

		// plays boop sound		
		PlaySoundBoop();//Alex's Audio Code
	}

	// plays welcome audio
	void PlaySoundBoop() {	//Alex's Audio code
		aSource.Play ();
	}

	// initializes audio
	void InitSoundBoop() { //Alex's Audio Code
		// adds audio source component
		aSource = (AudioSource)gameObject.AddComponent <AudioSource>();
		// creates audio clip
		AudioClip aClip;
		// stores audio file in audioClip variable
		aClip = (AudioClip)Resources.Load ("s1(Boop)");
		// sets source to audio clip/file
		aSource.clip = aClip;
	}

	//this sets what grid Unit's the unit is next to
//	private void setConnections(){
////		int maxX = 
//	}

	//this will set the "state" of the grid unit (wall, path, etc)

}
