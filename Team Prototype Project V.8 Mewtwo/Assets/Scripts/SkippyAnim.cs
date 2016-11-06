using UnityEngine;
using System.Collections;

public class SkippyAnim : MonoBehaviour
{

	public GameObject skippy;
	public Material[] walkAnim = new Material[2];
	bool oneTwo = false;
	Renderer rend;
	int count = 0;
	// Use this for initialization
	void Start ()
	{
	
		if (!skippy) {
			Debug.Log ("Oooooh Noooo. Skippy is missing.");
		}
		rend = this.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void WalkAnimation (float counter, float timer, string direction)
	{
//		Debug.Log ("I HAVE BEEN SUMMONED");
		rend = this.GetComponent<Renderer> ();

		switch (direction) {
		//the first material in the walk array is forward, the second is left or right
		case "down":
			rend.material = walkAnim [0];

			float spriteOffset = rend.material.GetTextureScale ("_MainTex").x;//3;
//			Debug.Log ("OFFSET IS " + spriteOffset);

			if (counter < timer) {

			} else if (counter >= timer) {
				if (count < 3) {
					count++;
				} else {
					count = 0;
				}
			}
//			Debug.Log("COUNTER: " + counter + " TIMER " + timer + " COUNT " + count);
			switch (count) {
			case 0:
//					Debug.Log ("HELLO");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (spriteOffset * 0, 0));
				break;
			case 1:
//					Debug.Log ("I'M");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (spriteOffset * 1, 0));
				break;
			case 2:
//					Debug.Log ("A");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (spriteOffset * 0, 0));
				break;
			case 3:
//					Debug.Log ("FRIEND!");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (spriteOffset * 2, 0));
				break;
			default:
				break;
			}
			//}

			break;
		case "left":
			rend.material = walkAnim [1];
			float animOffset = rend.material.GetTextureScale ("_MainTex").x;// / 4;
//			Debug.Log ("OFFSET IS TOTALLY " + animOffset);
			if (counter < timer) {
				
			} else if (counter >= timer) {
				if (count < 3) {
					count++;
				} else {
					count = 0;
				}
			}
			//			Debug.Log("COUNTER: " + counter + " TIMER " + timer + " COUNT " + count);
			switch (count) {
			case 0:
				//					Debug.Log ("HELLO");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (animOffset * 0, 0));
				break;
			case 1:
				//					Debug.Log ("I'M");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (animOffset * 4, 0));
				break;
			case 2:
				//					Debug.Log ("A");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (animOffset * 0, 0));
				break;
			case 3:
				//					Debug.Log ("FRIEND!");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (animOffset * 5, 0));
				break;
			default:
				break;
			}

			break;
		case "right":
			rend.material = walkAnim [1];
			animOffset = rend.material.GetTextureScale ("_MainTex").x;
			
			if (counter < timer) {
				
			} else if (counter >= timer) {
				if (count < 3) {
					count++;
				} else {
					count = 0;
				}
			}
			//			Debug.Log("COUNTER: " + counter + " TIMER " + timer + " COUNT " + count);
			switch (count) {
			case 0:
				//					Debug.Log ("HELLO");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (animOffset * 1, 0));
				break;
			case 1:
				//					Debug.Log ("I'M");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (animOffset * 2, 0));
				break;
			case 2:
				//					Debug.Log ("A");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (animOffset * 1, 0));
				break;
			case 3:
				//					Debug.Log ("FRIEND!");
				rend.material.SetTextureOffset ("_MainTex", new Vector2 (animOffset * 3, 0));
				break;
			default:
				break;
			}
			break;
		default:
			Debug.LogError ("That is not a valid direction for Skippy to go, sir");
			break;
		}


	}

	public void DeathAnimation (float counter, float timer)
	{
		rend.material = walkAnim [0];
		
		float spriteOffset = rend.material.GetTextureScale ("_MainTex").x;
		Vector3 stableV3 = skippy.transform.localScale;

		float percent = counter / timer;

		skippy.transform.Rotate (Vector3.forward, counter * 100);
		skippy.transform.localScale = Vector3.Lerp (stableV3, Vector3.zero, percent);


	}


}
