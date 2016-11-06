using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class menuScript : MonoBehaviour {
	public Button startText;

	// AudioSource instance				
	public AudioSource aSource; //Alex's audio code

	// Use this for initialization
	void Start () {
		// initializes sound
		InitSoundWelcome();
		// plays welcome sound
		PlaySoundWelcome();
	}

	public void StartLevel() {
		Application.LoadLevel (1);
//		Debug.LogError ("First level called.");
	}
	// Update is called once per frame
	void Update () {
	
	}

	// plays welcome audio
	void PlaySoundWelcome() {	//Alex's audio Code
		aSource.Play();
	}

	// initializes audio
	void InitSoundWelcome() { //Alex's Audio code
		// adds audio source component
		aSource = (AudioSource)gameObject.AddComponent <AudioSource>();
		// creates audio clip
		AudioClip aClip;
		// stores audio file in audioClip variable
		aClip = (AudioClip)Resources.Load ("s6(Welcome)");
		// sets source to audio clip/file
		aSource.clip = aClip;
		// plays welcome sound
		aSource.Play();
	}
}
