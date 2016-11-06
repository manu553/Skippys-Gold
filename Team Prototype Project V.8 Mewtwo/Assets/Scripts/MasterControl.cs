using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MasterControl : MonoBehaviour
{

	/**the purpose of master control is to automate the levels, wins, loss, and quit functions of the game.
	 * upon loading the scene, this code will create a 3x3 grid to start, place the player at the start, and reveal the path.
	 * Aing's script may need to be augmented so it's easier to see, but otherwise it works well.
	 * this script will also automatically reset the grid, playerunit, and re-create the path when the player gets to the end.
	 * If the player dies, this script will also play the death screen, and allow the player to restart or quit.
	**/

	//here we get the GameObject variables we need.
	public GameObject gridMaster;
	public GameObject playerUnit;
	public List<GameObject> userPath = new List<GameObject> ();
	public Camera cam;

	//here we grab the scripting references we need.
	private MakeGrid mg;
	private LevelReset lr;
	private GridUnitBehavior gub;
	private PathAlgorithm pa;
	private CameraControl cc;
	private PathTextureAlgorithm pat;
	private SkippyAnim sa;
	public int pathPos;
	public int fadePos;
	private int currentPathPos = 0;
	public float duration = 0.5f;
	public float fadeTime;
	public float animCounter = 0;
	public float animTimer = 0.5f;
	public float deathCounter = 0;
	public float deathTimer = 2;
	bool startTorch = true;
	public Button Go;
	public string gameOverScreen;

	/**
	 * ALEX'S AUDIO CODE
	 **/
	// AudioSource instance
	public AudioSource aSource;
	// creates array of audio clips
	public AudioClip[] aClip;
	// will determine amount of time before next phrase plays
	float randomTime;
	float moveTime = 0.5f;
	// counter for timer
	float timeCounter = 0;
	float moveCounter = 0;
	// set variable to pick random phrase
	int randomMusic;
	// bool used to start timer
	bool isPlaying = true;
	bool isPath = true;
	bool scoreUpdate;
//	public bool isGo;


	public enum GameStates
	{
		gs_start,
		gs_torch,
		gs_play,
		gs_skippyMove,
		gs_win,
		gs_lose,
		gs_nextLevel,
		gs_returnMainMenu,
	}

	public GameStates currentState = GameStates.gs_start;

	// Use this for initialization
	void Start ()
	{
		cam = Camera.main;
		
		cc = cam.GetComponent<CameraControl> ();
		mg = gridMaster.GetComponent<MakeGrid> ();
		lr = gridMaster.GetComponent<LevelReset> ();
		pa = gridMaster.GetComponent<PathAlgorithm> ();
		pat = this.GetComponent<PathTextureAlgorithm> ();
		sa = playerUnit.GetComponent<SkippyAnim> ();

		InitSoundPhrase ();

		randomTime = Random.Range (10, 16);

		//this is to stop screen rotation 
		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;

	}
	
	// Update is called once per frame
	void Update ()
	{
	
		switch (currentState) {
		case GameStates.gs_start:
			generateStart ();
			changeState (GameStates.gs_torch);
			startTorch = true;
			for (int aaa = 0; aaa < mg.gridObjects.Count; aaa++) {
//				Debug.Log("WE COULD BE IMMORTALS!");
				pat.decideTexture (mg.gridObjects [aaa].GetComponent<GridUnitBehavior> ());
			}
			break;
		case GameStates.gs_torch:

			if (startTorch) {
				for (int bbb = 0; bbb < pa.pathList.Count; bbb++) {
					pa.pathList [bbb].GetComponent<GridUnitBehavior> ().setToPitMat ();
				}

				pa.stopLoop = false;
				//				pa.GeneratePath();
				//				Debug.LogWarning("HELP HELP IM BEING OPPRESSED");
				
				//				pa.materialize (pa.pathList [0]);
				//				pa.materialize (pa.pathList [1]);
				//				pa.materialize (pa.pathList [2]);
				
				
				
				//changed pathPos to 0 and fadePos to 1					
				pathPos = 0;
				fadePos = 1;
				
				userPath.Add (pa.pathList [0].gameObject);
				startTorch = false;
			} else if (!startTorch) {
				pa.stopLoop = true;
			}
			
			//AING	stops the timer when the fade has completed
			if (fadePos < pa.pathList.Count) {
				fadeTime -= Time.deltaTime;
			}
			
			//AING	sets material for each position of the path
			if (pa.stopLoop && fadeTime <= 0 && pathPos < pa.pathList.Count) {
				//materializing wave
				pa.materialize (pa.pathList [pathPos]);
				pathPos++;
				
				//added fadeTime reseter
				fadeTime = duration;
			}
			//changed conditions for if statemtnt
			if (pa.stopLoop && fadeTime <= 0 && pathPos == pa.pathList.Count && fadePos < pa.pathList.Count) {
				pa.fade (pa.pathList [fadePos]);
				fadePos++;
				
				fadeTime = duration;
			}
			
			if (fadePos == pa.pathList.Count) {
				changeState (GameStates.gs_play);
			}

			break;
		case GameStates.gs_play:
			//we should be constantly in this state while the player is playing.
//			if(Input.GetKeyDown(KeyCode.N)){
//				changeState(GameStates.gs_nextLevel);
//			}

//			if(Input.GetMouseButtonDown(0)){
//				ScoreManager.score++;
//			}

//			if (currentPathPos < userPath.Count - 1) {
//				sa.WalkAnimation (animCounter, animTimer, "right");
//				
//				if (animCounter < animTimer) {
//					animCounter += Time.deltaTime;
//				} else if (animCounter >= animTimer) {
//					animCounter = 0;
//				}
			//}

			//here we initialize the random sound counter and do things
			if (timeCounter <= randomTime) {
				timeCounter += Time.deltaTime;
			} else {
				PlaySoundPhrase ();
				timeCounter = 0;
				randomTime = Random.Range (10, 16);
			}

			//AING		change the player selected path back to path texture
//			if(){
//				changeState(GameStates.gs_skippyMove);
//				
//				Debug.Log ("remaking path");
//				for(int i = 0; i < pa.pathList.Count; i++){
//					userPath[i].GetComponent<GridUnitBehavior>().isSelectedPath = false;
//					pa.materialize (pa.pathList [i]);
//					Debug.Log("setting material");
//				}
//			}
			//AING 		to update score in gs_skippyMove
			scoreUpdate = true;

//			onEnable();
			break;
		case GameStates.gs_skippyMove:
			//here is where skippy will move along the path.
			if (currentPathPos < userPath.Count - 1) {
				sa.WalkAnimation (animCounter, animTimer, getDirection ());

				if (animCounter < animTimer) {
					animCounter += Time.deltaTime;
				} else if (animCounter > animTimer) {
					animCounter = 0;
				}
			}

			if (currentPathPos < userPath.Count - 1) {
				if (ComparePaths (currentPathPos)) {//checkNext()){

					if (moveCounter < moveTime) {
						moveCounter += Time.deltaTime;

//						if(moveCounter >= moveTime/6){
//							Debug.Log("WAAAAAAAAALLEEEEEEE");
//							userPath[currentPathPos].GetComponent<GridUnitBehavior>().isSelectedPath = false;
//						}

						float percent = moveCounter / moveTime;

						if (percent >= 0.1f) {
							userPath [currentPathPos + 1].GetComponent<GridUnitBehavior> ().isSelectedPath = false;
						}

						playerUnit.transform.position = Vector2.Lerp (userPath [currentPathPos].transform.position, userPath [currentPathPos + 1].transform.position, percent);
					} else if (moveCounter >= moveTime) {
//						Debug.Log("I have finished moving!");
						userPath [currentPathPos].GetComponent<GridUnitBehavior> ().isSelectedPath = false;
						pa.materialize (pa.pathList [currentPathPos + 1]);
						moveCounter = 0;
						currentPathPos++;
					}

//					Debug.Log("My current position is " + currentPathPos + " and the count is " + userPath.Count);

				} else if (!ComparePaths (currentPathPos)) {//else if(!checkNext()){
					if (moveCounter < moveTime) {
						moveCounter += Time.deltaTime;
						
						float percent = (moveCounter / moveTime) / 2;

//						Vector2 midWay = Vector2.Distance(userPath[currentPathPos].transform.position, userPath[currentPathPos + 1].transform.position) + userPath[currentPathPos].transform.position;
						
						playerUnit.transform.position = Vector2.Lerp (userPath [currentPathPos].transform.position, userPath [currentPathPos + 1].transform.position, percent);
					} else if (moveCounter >= moveTime) {
//						Debug.Log("I have finished moving, and shall now fall to my death");
						moveCounter = 0;
						changeState (GameStates.gs_lose);
					}
				}
			} else if (userPath [currentPathPos - 1].GetComponent<GridUnitBehavior> ().isEnd) {
//				Debug.Log("Hello!");
				changeState (GameStates.gs_win);
			} else {
				changeState (GameStates.gs_win);
			}

			break;
		case GameStates.gs_win:
			//here we do the winning stuff, then move on to "next level" state.
			currentPathPos = 0;

			userPath.Clear ();
			changeState (GameStates.gs_nextLevel);
			break;
		case GameStates.gs_lose:
			//here we do the losing stuff, then move on from there to where we need to go.

			sa.DeathAnimation(deathCounter, deathTimer);

			if(deathCounter < deathTimer){
				deathCounter += Time.deltaTime;
			}else{
				Application.LoadLevel (gameOverScreen);
			}
			break;
		case GameStates.gs_nextLevel:
			//this should load the next level.
			generateNextLevel ();

			AchievementManager.Instance.AddProgressToAchievement ("Level 3!", 1);

//			Debug.Break();
//			Debug.LogError("We have stopped the code, sir");
			startTorch = true;
			break;
		case GameStates.gs_returnMainMenu:
			break;
		default:
			break;

		}

	}

//	void onEnable() {
//		Go.onClick.AddListener (send);
//	}
	
//	public void send() {
//		changeState(GameStates.gs_skippyMove);
//	}

	void generateStart ()
	{
		ScoreManager.level++;
		mg.XUnits = 3;
		mg.YUnits = 3;
		mg.CreateGrid ();
		cc.increaseCam ();
		pa.pathList.Clear ();
		pa.pathList = new List<GridUnitBehavior> ();
		pa.GeneratePath ();
		playerUnit.transform.position = pa.pathList [0].transform.position;
	}

	void generateNextLevel ()
	{
		ScoreManager.level++;
//		Debug.Log ("GENERATING NEXT LEVEL PROTOCOL.");
		pa.pathList [0].GetComponent<GridUnitBehavior> ().isStart = false;
		pa.pathList [pa.pathList.Count - 1].GetComponent<GridUnitBehavior> ().isEnd = false;
		pa.pathList.Clear ();
		pa.pathList = new List<GridUnitBehavior> ();
		lr.NextLevel ();
		cc.increaseCam ();
		pa.GeneratePath ();
		for (int aaa = 0; aaa < mg.gridObjects.Count; aaa++) {
//							Debug.Log("MY CAT KNOWS WHAT YOU DID IN THE DAAAAAARK!");
			pat.decideTexture (mg.gridObjects [aaa].GetComponent<GridUnitBehavior> ());
		}
		changeState (GameStates.gs_torch);
		playerUnit.transform.position = pa.pathList [0].transform.position;
	}

	public void changeState (GameStates newState)
	{
		currentState = newState;
	}

	private bool checkNext ()
	{
		bool isPath = false;
		GridUnitBehavior check = userPath [currentPathPos + 1].GetComponent<GridUnitBehavior> ();

		isPath = check.isPath;

		return isPath;
	}

	public bool ComparePaths (int currentPos)
	{
		bool compare = false; //to be returned if user's submission is acceptable
//		int pathCount = 0; //position in pathList to be compared 
//		Debug.Log ("WHY HOWDY THERE!");
		if (currentPos < userPath.Count) {
//			Debug.Log("EXTERMINATE!");
			if (userPath [currentPos + 1].gameObject == pa.pathList [currentPos + 1].gameObject) {
				compare = true;

				if (scoreUpdate) {
					ScoreManager.score++;
					AchievementManager.Instance.AddProgressToAchievement ("Gold Collector!", 1);
					AchievementManager.Instance.AddProgressToAchievement ("Gold Hunter!", 1);
//					Debug.Log("comparing position: " + currentPos + " = TRUE");
				}
//				Debug.Log("DEBUGDEBUGDEBUG!");
			} else {
//				Debug.Log("YOU FAIL!");
				compare = false;
			}
		}
		
		return compare;		
	}

	public bool checkState (GameStates check)
	{
		bool isState = false;

		if (check == currentState) {
			isState = true;
		} 
		return isState;
	}

	private string getDirection ()
	{
		string dir = "FAILURE";
		float XDif = userPath [currentPathPos].transform.position.x - userPath [currentPathPos + 1].transform.position.x;
//		float YDif = userPath [currentPathPos].transform.position.y - userPath [currentPathPos + 1].transform.position.y;
		if (XDif > 0) {
			dir = "left";
		} else if (XDif < 0) {
			dir = "right";
		} else {
			dir = "down";
		}

		return dir;
	}

	// picks random phrase and plays it
	void PlaySoundPhrase ()
	{ //Alex's Audio Code
		// generates random number between 0 and 7
		randomMusic = Random.Range (0, 7);
		
		// assigns audio clip to the different numbers that can be generated
		switch (randomMusic) {
		case 0:
			aSource.clip = aClip [0];
			break;
		case 1:
			aSource.clip = aClip [1];
			break;
		case 2:
			aSource.clip = aClip [2];
			break;
		case 3:
			aSource.clip = aClip [3];
			break;
		case 4:
			aSource.clip = aClip [4];
			break;
		case 5:
			aSource.clip = aClip [5];
			break;
		}
		Debug.Log ("Current Clip = music" + (randomMusic + 1));
		// plays the audio clip currently selected
		aSource.Play ();
	}

	// initializes audio
	void InitSoundPhrase ()
	{ //Alex's Audio Code
		// adds audio source component
		aSource = (AudioSource)gameObject.AddComponent <AudioSource> ();
		// stores audio file in audioClip variable
		aClip = new AudioClip[]
		{
			(AudioClip)Resources.Load ("s2(Payments)"),
			(AudioClip)Resources.Load ("s3(Want me treasure)"),
			(AudioClip)Resources.Load ("s4(Poop deck)"),
			(AudioClip)Resources.Load ("s5(Yarg)"),
			(AudioClip)Resources.Load ("s7(Help me)"),
			(AudioClip)Resources.Load ("s9(Lost me treasure)")
		};
	}
}
