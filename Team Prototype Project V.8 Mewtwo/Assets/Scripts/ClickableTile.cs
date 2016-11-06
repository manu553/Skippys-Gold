using UnityEngine;
using System.Collections;
/**
 * This class makes the titles "clickable" by moving the the selected unit
 * (the player) to a certain tile. The tileX and tileY are the positions of
 * the grid units.
 */
public class ClickableTile : MonoBehaviour {

	public int tileX;
	public int tileY;
	public MakeGrid grid;

	void OnMouseUp() {
//		grid.MoveSelectedUnitTo (tileX, tileY);
	}
}
