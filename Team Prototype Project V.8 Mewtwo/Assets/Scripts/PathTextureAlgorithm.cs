using UnityEngine;
using System.Collections;

public class PathTextureAlgorithm : MonoBehaviour
{

	public Material[] wallMat = new Material[8];
	public Material[] pathMat = new Material[6];
	public Material[] pathWallMat = new Material[13];
	public Material pitMat;
	GameObject gridMaster;
	MakeGrid mg;
	PathAlgorithm pa;

	// Use this for initialization
	void Start ()
	{
		gridMaster = GameObject.Find ("GridMaster");
		pa = gridMaster.GetComponent<PathAlgorithm> ();
		mg = gridMaster.GetComponent<MakeGrid> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void decideTexture (GridUnitBehavior gub)
	{
//		Debug.Log ("I WILL CHANGE YOU, LIKE A REMIX, AND I'LL RAISE YOU, LIKE A PHEONIX!");
		int unitState = 0;

		int XPos = gub.getX ();
		int YPos = gub.getY ();
		int XMax = mg.getXMax () - 1;
		int YMax = mg.getYMax () - 1;
		int pathPos = gub.pathPos;
		int nextPathPos;
		int lastPathPos;

		string lastPos = "south";
		string nextPos = "north";

		if (XPos == 0 || YPos == 0 || XPos == XMax || YPos == YMax) {
//			Debug.Log ("We are probably not broken or something " + gub.name);
			if (!gub.isStart && !gub.isEnd) {
				unitState++;
//				Debug.Log ("We are falling in line quite well sir " + gub.name);
			}
		}
		if (gub.isPath || gub.isStart || gub.isEnd) {
			unitState += 2;
		}

		GridUnitBehavior next;

		GridUnitBehavior last;



		switch (unitState) {
		//walls
		case 1:

			if (XPos == 0 && YPos == 0) { // corner, top left
				gub.pitMat = wallMat [4];
			} else if (XPos == XMax && YPos == 0) { // corner top right
				gub.pitMat = wallMat [5];
			} else if (XPos == XMax && YPos == YMax) {// corner bottom right
				gub.pitMat = wallMat [7];
			} else if (XPos == 0 && YPos == YMax) {//corner bottom left
				gub.pitMat = wallMat [6];
			} else if (YPos == 0) {//top walls
				gub.pitMat = wallMat [0];
			} else if (XPos == XMax) {//right walls
				gub.pitMat = wallMat [1];
			} else if (YPos == YMax) {// bottom walls
				gub.pitMat = wallMat [2];
			} else if (XPos == 0) {// left walls
				gub.pitMat = wallMat [3];
			}
			

			break;

		//path
		case 2: // the start square will be in here

			if (gub.isStart) {//if this unit is the start, it's always from north to somewhere
				nextPathPos = 1;
				next = pa.pathList [nextPathPos].GetComponent<GridUnitBehavior> ();

				if (next.getX () == XPos + 1) {//this is going to the right
					nextPos = "west";
				} else if (next.getX () == XPos - 1) {//this is going to the left
					nextPos = "east";
				} else if (next.getY () == YPos + 1) {//this is going down
					nextPos = "south";
				} else {//not totally sure what direction this is going. 
					Debug.LogError ("There appears to be an error in PTA CASE2, sir " + gub.name);
				}

				switch (nextPos) {
				case "west":

					gub.pathMat = pathMat [5];
					gub.pitMat = pitMat;

					break;
				case "east":
					gub.pathMat = pathMat [4];
					gub.pitMat = pitMat;
					break;
				case "south":
					gub.pathMat = pathMat [1];
					gub.pitMat = pitMat;
					break;
				default:
					Debug.LogError ("THE NEXT POS DOESN'T FOLLOW EUCLIDEAN GEOMETRY! " + gub.name);
					break;
				}


			} else if (gub.isEnd) {
				nextPathPos = pa.pathList.Count - 2;
				last = pa.pathList [nextPathPos].GetComponent<GridUnitBehavior> ();
				if (XPos == 0) {
					gub.pathMat = pathWallMat[3];
				}else if(XPos == mg.getXMax() - 1){
					gub.pathMat = pathWallMat[1];
				}
				else {
					gub.pathMat = pathMat[1];
				}

			} else {// this isn't the start, therefor we have to check both connecting units
				nextPathPos = pathPos + 1;
				lastPathPos = pathPos - 1;
				next = pa.pathList [nextPathPos].GetComponent<GridUnitBehavior> ();
				last = pa.pathList [lastPathPos].GetComponent<GridUnitBehavior> ();

				if (next.getX () == XPos + 1) {
					nextPos = "west";
				} else if (next.getX () == XPos - 1) {
					nextPos = "east";
				} else if (next.getY () == YPos + 1) {
					nextPos = "south";
				} else {
					Debug.LogError ("There appears to be an error in checking the path's next position, sir " + gub.name);
				}

				if (last.getX () == XPos + 1) {
					lastPos = "west";
				} else if (last.getX () == XPos - 1) {
					lastPos = "east";
				} else if (last.getY () == YPos - 1) {
					lastPos = "north";
				} else {
					Debug.LogError ("There appears to be an error in checking the path's next position, sir, wiggle wiggle " + gub.name);
				}
				//here we're applying the material based on what the position of the last and next directions are
				switch (lastPos) {
				case "north"://this is from the top to either below, left, or right
					switch (nextPos) {
					case "south":
						gub.pathMat = pathMat [1];
						break;
					case "east":
						gub.pathMat = pathMat [4];
						break;
					case "west":
						gub.pathMat = pathMat [5];
						break;
					default:
						Debug.LogError ("SUCH ERROR, MANY BREAK " + gub.name);
						break;
					}
					break;
				case "west": //this is from the right to the left or the bottom. It's not from the top because that should be impossible
					if (nextPos.Equals ("east")) {
						gub.pathMat = pathMat [0];
					} else {
						gub.pathMat = pathMat [3];
					}
					break;
				case "east": // this is from left to right or bottom. like above, it can't go north.
					if (nextPos.Equals ("west")) {
						gub.pathMat = pathMat [0];
					} else {//this is south
						gub.pathMat = pathMat [2];
					}

					break;
				default:
					Debug.LogError ("ERROR, ERROR, ERROR! " + gub.name);
					break;
				}
			}
			gub.pitMat = pitMat;
			break;

		//wall and path
		case 3:
			nextPathPos = pathPos + 1;
			lastPathPos = pathPos - 1;
			next = pa.pathList [nextPathPos].GetComponent<GridUnitBehavior> ();
			last = pa.pathList [lastPathPos].GetComponent<GridUnitBehavior> ();

			//here we need to discover where the path is coming from and going

			if (XPos == 0) {//this is for the wall tiles that are to the very left
//				
				if (YPos == 0) {//obviously only one tile type can be in the corner and also be a path
					gub.pathMat = pathWallMat [4];
					gub.pitMat = wallMat[4];
				} else if (YPos == YMax) {//this is the bottom left path
					gub.pathMat = pathWallMat [5];
					gub.pitMat = wallMat[6];
				} else {//here are all the sections within 
					if (next.getX () == XPos + 1) {
						gub.pathMat = pathWallMat [13];
						gub.pitMat = wallMat[3];
					} else if (next.getY () == YPos + 1) {
						if (last.getY () == YPos - 1) {
							gub.pathMat = pathWallMat[3];
							gub.pitMat = wallMat[3];
						}else{
							gub.pathMat = pathWallMat[10];
							gub.pitMat = wallMat[3];
						}
					}
				}
			} else if(XPos == XMax){//this is for the tiles that are in the very right
				if(YPos == 0){ // this means it's the top right corner
					gub.pathMat = pathWallMat[5];
					gub.pitMat = wallMat[5];
				}else if (YPos == YMax){//this means it's the bottom right corner
					gub.pathMat = pathWallMat[4];
					gub.pitMat = wallMat[7];
				} else{// if it's not one of the corners, it has to be on the very right side
					if(next.getX() == XPos - 1){// if the next path position is coming from the left then it must be coming from up
						gub.pathMat = pathWallMat[12];
						gub.pitMat = wallMat[1];
					} else if(next.getY() == YPos + 1){//here if it's going down, there's two directions it can come from, left and top
						if(last.getY() == YPos - 1){//it must Be coming from the top
							gub.pathMat = pathWallMat[1];
							gub.pitMat = wallMat[1];
						}else{//otherwise it has to be coming from the left
							gub.pathMat = pathWallMat[8];
							gub.pitMat = wallMat[1];
						}
					}
				}
			}else if(YPos == 0 && XPos != 0 && XPos != XMax){//these are for the tiles that are at the top, but aren't corners
				//it's possible for tiles to be coming from either left or right, and going either left, right, or down.
				if(next.getY() == YPos + 1){// here the path is going down
					if(last.getX() == XPos - 1){// and now it's coming from the left
						gub.pathMat = pathWallMat[9];
						gub.pitMat = wallMat[0];
					}else if(last.getX() == XPos + 1){//now it's coming from the right
						gub.pathMat = pathWallMat[11];
						gub.pitMat = wallMat[0];
					}
				}else{//now if it's not going down, it must be going straight, therefor it's a verticle piece
					gub.pathMat = pathWallMat[0];
					gub.pitMat = wallMat[0];
				}
			}//we aren't checking the bottom walls, because if any are a wall and path, it's the end

//			gub.pitMat = pitMat;
			break;

		//pit
		default:
			gub.pathMat = pitMat;
			gub.pitMat = pitMat;
			break;
		}

	}

}
