using UnityEngine;
using System;
using System.Collections;

/**
Class GameSounds

contains array of game sounds and methods to play specific game sounds

Author: Alex Zielinski
Modified: 07/05/2015
**/

public class GameSounds : MonoBehaviour {

  // allocates memory for audio source file
	public AudioSource aSource;
  
  // creates array of audio clips
	public AudioClip [] audioClip;

	// At start accesses audio source files
	void Start () {
		aSource = this.GetComponent<AudioSource>();
	}
	
  // plays welcome sound when user starts game
  public void PlaySoundWelcome() {
    PlaySound(0);
  }
  
  // plays click sound when user clicks   
  public void PlaySoundClick() {
    PlaySound(0);
  }

  // plays specific audio in regards to audio elements in array
	public void PlaySound(int i) {
		aSource.clip = audioClip[i];
		aSource.Play();
	} 
}