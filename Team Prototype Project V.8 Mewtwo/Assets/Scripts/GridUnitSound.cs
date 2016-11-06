using UnityEngine;
using System.Collections;

public class GridUnitSound : MonoBehaviour {

	// Use this for initialization
	
	GameSounds gs;
	
//	public bool playStartSound = true;
	
	void Start() {
		gs = this.GetComponent<GameSounds>();
		//		gs.PlaySoundWelcome();
	}
	
	
	// Update is called once per frame
	void Update () {
		
//		if (playStartSound) {
//			gs.PlaySoundWelcome();
//			playStartSound = false;
//		}
	}
	
	void OnMouseUp(){
//		gs.PlaySoundClick ();
	}
}
