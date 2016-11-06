using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MakeGrid : MonoBehaviour {

	//this is the array list for grid objects
	public List<GameObject> gridObjects = new List<GameObject>();
//	public GameObject[,] gridObjects;

	//these are the game object variables
	public GameObject gridUnit;
	public GameObject selectedUnit;

	//this is the rendered of the gridUnit
	Renderer gridRender;

	public int XUnits = 5;
	public int YUnits = 5;

	Camera cam;

	CameraControl cc;

	GridUnitBehavior gub;

	// Use this for initialization
	void Start () {

		cam = Camera.main;
		cc = cam.GetComponent<CameraControl> ();
		if (gridUnit == null) {
			Debug.LogError ("There is no gridUnit primitive!!");
		} else {
			gridRender = gridUnit.GetComponent<Renderer>();
		}
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.Return)) {
//			CreateGrid ();///////// changed
//			cc.increaseCam();
//		}
	}

	//the gridX and Y are for how many suqares you want in the grid, sizeX and Y is how long you want
	//the actual grid to be, and startX and Y are where you want the grid to start
	public void CreateGrid(){

		//here we want to split up the grid by it's length and size to position each square
		float sizeFullY = gridRender.bounds.size.y;
		float sizeFullX = gridRender.bounds.size.x;
		int arrayPos = 0;

		for(int jjj = 0; jjj < XUnits; jjj++){

			float posX = this.transform.position.x + (sizeFullX * jjj);

			for(int lll = 0; lll < YUnits; lll++){
				float posY = this.transform.position.y - (sizeFullY * lll);

//				Debug.Log(arrayPos);

				if(gridObjects.Count < arrayPos + 1){
					GameObject newObj = Instantiate(gridUnit) as GameObject;
					gridObjects.Add(newObj);

					//added in by Manu
					//created a a new class called ClickableTile which moves the unit(Player) to a certain tile
					//sets the variable tileX and tileY to the position of the grid array
					//ct.grid just chooses this grid
				}

				ClickableTile ct = gridObjects[arrayPos].GetComponent<ClickableTile>();
				ct.tileX = jjj;
				ct.tileY = lll;
				ct.grid = this;

				gridObjects[arrayPos].transform.position = new Vector2(posX, posY);
				gridObjects[arrayPos].gameObject.name = "gridUnit " + jjj + ", " + lll;
				gub = gridObjects[arrayPos].GetComponent<GridUnitBehavior>();
				gub.setX(jjj);
				gub.setY(lll);

				if(jjj == 0 || lll == 0 || jjj == XUnits - 1 || lll == YUnits - 1){
					gub.setIsWall(true);
				}else{
					gub.setIsWall(false);
				}

				arrayPos++;
			}
		}

		int excess = XUnits * YUnits;
		cleanGrid (excess);
	}

	void cleanGrid(int excessStart){

		int excess = (gridObjects.Count - excessStart);

		for (int mmm = 0; mmm < excess; mmm++) {
			Destroy(gridObjects[excessStart + mmm]);
		}

		gridObjects.RemoveRange (excessStart, excess);
	}
	//created and added by Manu
	//This method moves the selected unit which is the player(called unit)
	//to an x and y position on the grid, y is inverted since the grid is
	//built up instead of down.
	public void MoveSelectedUnitTo(int x, int y ) {
//		selectedUnit.transform.position = new Vector2 (x, -y);
	}

	void setPath(bool[] isPath){
		for(int nnn = 0; nnn < gridObjects.Count; nnn++){
			GameObject obj = gridObjects[nnn];
		}
	}

	//these methods return the X and Y unit max's
	public int getXMax (){
		return XUnits;
	}

	public void setXMax(int newX){
		XUnits = newX;
	}

	public int getYMax (){
		return YUnits;
	}

	public void setYMax(int newY){
		YUnits = newY;
	}

	public GameObject getGridUnit(int XPos, int YPos){

		int elPosition = (XPos * getYMax()) + YPos;
		return gridObjects[elPosition];
	}

	public GameObject getGridUnit(int pos){
		return gridObjects[pos];
	}
}





