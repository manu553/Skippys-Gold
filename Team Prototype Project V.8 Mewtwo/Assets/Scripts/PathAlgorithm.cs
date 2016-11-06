using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathAlgorithm : MonoBehaviour
{
	//AING 	array of ints. Each position is a posY*posX + posX coordinate of the path.
	//can't find first position of the path...?
	public List<GridUnitBehavior>pathList = new List<GridUnitBehavior>();	
//	public int pathPos;
//	public int fadePos;
//	public float duration = 0.5f;
//	public float fadeTime;
	//AING 	moved from while loop, to let update() know when to fade
	public bool redoMove = false;
	public bool stopLoop = false;


	//these are the variables for this code
	//these are the script references for this code
	public MakeGrid mg;

	//These are Integers for this script
	int deltaDir = 0;

	// Use this for initialization
	void Start ()
	{
		mg = this.GetComponent<MakeGrid> ();
		//AING	setting speed of the fade
//		duration = 0.5f;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public void GeneratePath ()
	{
//		Debug.LogError ("I have begun the operation, Sir");

		GridUnitBehavior gub;

		for(int ccc = 0; ccc < mg.gridObjects.Count; ccc++){
			gub = mg.gridObjects[ccc].GetComponent<GridUnitBehavior>();
			gub.setState(false);
			gub.setUserPathState (false);
//			Debug.LogWarning ("Some men just want to");
			gub.fadeMaterial();
		}

		int posX = Random.Range (0, mg.getXMax ());;
		int posY = 0;
		int yMax = mg.getYMax();

		GameObject obj = mg.getGridUnit (posX, 0);
		gub = obj.GetComponent<GridUnitBehavior>();
		gub.isStart = true;
		gub.setState(true);
		gub.setUserPathState (false);
		pathList.Add(gub);
		gub.pathPos = pathList.Count - 1;

//		bool redoMove = false;
		stopLoop = false;


		//here we start with creating a while-loop that keeps 
		//generating a path until it reaches the bottom of the grid
		while (stopLoop == false) {
//			Debug.Log(mg.getYMax() + " is max compared to " + posY);

			int direction = Random.Range (0, 3);

			do {
//				Debug.Log(mg.getYMax() + " is max compared to " + posY + " Sir.");
				switch (direction) {
				case 0: 
					//down
					if(posY < mg.getYMax() - 1){
						posY++;
					}
					redoMove = false;
					break;
				case 1: 
					//left
					if(posX <= 0){
						redoMove = true;
						direction = Random.Range(0,3);
//						Debug.Log("ALPHA");
					}
					else if(deltaDir == 2){
//						Debug.Log("BETA");
						redoMove = true;
						direction = Random.Range (0, 3);
					}
					else{
						posX--;
						redoMove = false;
					}
					break;
				case 2: 
					//right
					if(posX >= mg.getXMax() - 1){
						direction = Random.Range(0,2);
						redoMove = true;
					}
					else if(deltaDir == 1){
						redoMove = true;
						direction = Random.Range (0, 2);
					}
					else{
						posX++;
						redoMove = false;
					}
					break;
				default:
					posY++;
					redoMove = false;
					break;
				}

//				Debug.Log("Redo was " + redoMove);
			} while(redoMove);

			deltaDir = direction;
			gub = mg.getGridUnit(posX, posY).GetComponent<GridUnitBehavior>();
			gub.setState(true);
			gub.setUserPathState(false);

			//AING 		adding each path gub to pathList
			pathList.Add(gub);
			gub.pathPos = pathList.Count - 1;
//			Debug.Log("I have added " + gub.gameObject.name);

			if(posY == mg.getYMax() - 1){
				gub.isEnd = true;
				stopLoop = true;
			}
		}
	}

	//add fading effect
	public void materialize(GridUnitBehavior gub){
		gub.setMaterial();
	}
	
	public void fade(GridUnitBehavior gub){
//		Debug.LogWarning ("Watch the world burn");

		gub.fadeMaterial();

		
	}
}
