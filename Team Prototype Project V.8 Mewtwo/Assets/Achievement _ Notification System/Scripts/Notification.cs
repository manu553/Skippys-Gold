using UnityEngine;
using System.Collections;

[System.Serializable]
public class Timing
{
		public float fadeInDuration = 1;
		public float fadeOutDuration = 1;
		public float fadeOutAfter = 4;
		public float selfDisableAfter = 5;
}

[System.Serializable]
public class NotificationBoxSettings
{
		public bool fixedSize;
		public int widthMin = 300;
		public int widthMax = 600;
		public Rect imgBox = new Rect (10, 5, 40, 40);
		/*This is position relative to screen size. For example:
		 * +(0,0) mean (top,left) 
		 * +(1,1) mean (bottom,right)
		 */
		public Vector2 startingPosition = new Vector2 (0.5f, 0);
		public Vector2 moveTo = new Vector2 (0.5f, 0.08f);
		public LeanTweenType fadeInStyle;
		public LeanTweenType fadeOutStyle;
}

[System.Serializable]
public class AutoFitSettings
{
		public float boxToFontSizeRatio = 0.64f;
		public float uppercaseLetterFactor = 0.1f;
		public float whitespaceFactor = -0.4f;
		public float symbolFactor = -0.1f;
}

public class Notification : MonoBehaviour
{
		public LTRect notificationBox = new LTRect (0f, 0f, 100f, 50f);
		public Timing timing;
		public NotificationBoxSettings notificationBoxSettings;
		public AutoFitSettings autofitSettings;
		public AudioClip defaultSound;
		public float volume = 0.6f;
		public GUIStyle boxStyle;
		public GUIStyle imageBoxStyle;

		private AudioClip soundclip;

		[HideInInspector]
		public GUIContent
				message;

		private static Notification instance;
		public static Notification Instance {
				get {
						return instance;
				}
		}

		//These are for LeanTween's tween id
		private int id1, id2, id3, id4;

		void Awake ()
		{
				if (Instance != null && Instance != this) {
						Destroy (this.gameObject);
				}
				instance = this;
				notificationBox.rect = new Rect (Screen.width * notificationBoxSettings.startingPosition.x - (notificationBox.rect.width / 2), Screen.height * notificationBoxSettings.startingPosition.y - (notificationBox.rect.height / 2), notificationBox.rect.width, notificationBox.rect.height);
				if (defaultSound != null)		
						soundclip = defaultSound;
		}

		public void setMsg (string ms)
		{
				changeWidth (calculateWidth (ms, 0));
				this.enabled = false;
				message = new GUIContent (ms);
				this.enabled = true;
		}

		public void setMsg (string ms, float fixedWidth)
		{
				changeWidth (fixedWidth);
				this.enabled = false;
				message = new GUIContent (ms);
				this.enabled = true;
		}

		public void setMsg (string ms, Texture2D img)
		{
				changeWidth (calculateWidth (ms, notificationBoxSettings.imgBox.width));
				this.enabled = false;
				message = new GUIContent (ms, img);
				this.enabled = true;
		}

		public void setMsg (string ms, Texture2D img, float fixedWidth)
		{
				changeWidth (fixedWidth);
				this.enabled = false;
				message = new GUIContent (ms, img);
				this.enabled = true;
		}

		public void setMsg (string ms, AudioClip clip)
		{
				changeWidth (calculateWidth (ms, 0));
				this.enabled = false;
				message = new GUIContent (ms);
				if (clip != null)
						soundclip = clip;
				this.enabled = true;
		}

		public void setMsg (string ms, AudioClip clip, float fixedWidth)
		{
				changeWidth (fixedWidth);
				this.enabled = false;
				message = new GUIContent (ms);
				if (clip != null)
						soundclip = clip;
				this.enabled = true;
		}

		public void setMsg (string ms, Texture2D img, AudioClip clip)
		{
				changeWidth (calculateWidth (ms, notificationBoxSettings.imgBox.width));
				this.enabled = false;
				message = new GUIContent (ms, img);
				if (clip != null)
						soundclip = clip;
				this.enabled = true;
		}

		public void setMsg (string ms, Texture2D img, AudioClip clip, float fixedWidth)
		{
				changeWidth (fixedWidth);
				this.enabled = false;
				message = new GUIContent (ms, img);
				if (clip != null)
						soundclip = clip;
				this.enabled = true;
		}

		public float calculateWidth (string ms, float imgBoxWidth)
		{
				int uppercaseCount = 0;
				int whitespaceCount = 0;
				int symbolCount = 0;
				for (int i = 0; i < ms.Length; i++) {
						if (char.IsUpper (ms [i]))
								uppercaseCount++;
						if (char.IsWhiteSpace (ms [i]))
								whitespaceCount++;
						if (char.IsPunctuation (ms [i])) {
								symbolCount++;
						}
				}
				float w = boxStyle.fontSize * (ms.Length * autofitSettings.boxToFontSizeRatio 
						+ uppercaseCount * autofitSettings.uppercaseLetterFactor 
						+ whitespaceCount * autofitSettings.whitespaceFactor
						+ symbolCount * autofitSettings.symbolFactor) 
						+ imgBoxWidth;
				//print (ms.Length + " " + w);
				if (w < notificationBoxSettings.widthMin && notificationBoxSettings.widthMin > 0) {
						w = notificationBoxSettings.widthMin;
				}
				if (w > notificationBoxSettings.widthMax && notificationBoxSettings.widthMax > 0
						&& notificationBoxSettings.widthMax >= notificationBoxSettings.widthMin) {
						w = notificationBoxSettings.widthMax;
				}
				//Set the box starting position with the width has changed. The y and the height value still the same as in Awake()
				return w;
		}

		public void changeWidth (float w)
		{
				notificationBox.rect = new Rect (Screen.width * notificationBoxSettings.startingPosition.x - (w / 2), notificationBox.rect.y, w, notificationBox.rect.height);
		}
	
		public void delayMsg (string ms, float time)
		{
				StartCoroutine (showMsg (ms, time));
		}

		IEnumerator showMsg (string ms, float time)
		{
				yield return new WaitForSeconds (time);
				setMsg (ms);
		}
	
		void OnEnable ()
		{
				CancelInvoke ("selfDisable");
				AudioSource.PlayClipAtPoint (soundclip, Camera.main.transform.position, volume);
				soundclip = defaultSound;
				//Make it alpha 1 and make it move down from above
				id1 = LeanTween.move (notificationBox, new Vector2 (Screen.width * notificationBoxSettings.moveTo.x - (notificationBox.rect.width / 2), Screen.height * notificationBoxSettings.moveTo.y - (notificationBox.rect.height / 2)), timing.fadeInDuration)
						.setEase (notificationBoxSettings.fadeInStyle)
						.setUseEstimatedTime (true)
						.id;
				id2 = LeanTween.move (notificationBox, new Vector2 (Screen.width * notificationBoxSettings.startingPosition.x - (notificationBox.rect.width / 2), Screen.height * notificationBoxSettings.startingPosition.y - (notificationBox.rect.height / 2)), timing.fadeOutDuration)
						.setEase (notificationBoxSettings.fadeOutStyle)
						.setUseEstimatedTime (true)
						.setDelay (4)
						.id;
				//Ensure that notification box always transparent before show
				notificationBox.alpha = 0;
				id3 = LeanTween.alpha (notificationBox, 1, timing.fadeInDuration).setEase (LeanTweenType.easeInQuad).setUseEstimatedTime (true).id;
				id4 = LeanTween.alpha (notificationBox, 0, timing.fadeOutDuration).setEase (LeanTweenType.easeInQuad).setDelay (4).setUseEstimatedTime (true).id;
				if (timing.selfDisableAfter > 0)
						Invoke ("selfDisable", timing.selfDisableAfter);
		}

		void OnDisable ()
		{
				//Cancel any tween that still playing
				LeanTween.cancel (notificationBox, id1);
				LeanTween.cancel (notificationBox, id2);
				LeanTween.cancel (notificationBox, id3);
				LeanTween.cancel (notificationBox, id4);
				CancelInvoke ("selfDisable");
				//Reset the values
				notificationBox.alpha = 0;
				notificationBox.rect = new Rect (Screen.width * notificationBoxSettings.startingPosition.x - (notificationBox.rect.width / 2), Screen.height * notificationBoxSettings.startingPosition.y - (notificationBox.rect.height / 2), notificationBox.rect.width, notificationBox.rect.height);
				message = null;
		}
			
		void selfDisable ()
		{
				this.enabled = false;
		}

		void OnGUI ()
		{
				if (message.image == null) {
						//If this is just a simple message, show the button only
						GUI.Button (notificationBox.rect, message, boxStyle);
				} else {
						//If this message include image
						GUI.BeginGroup (notificationBox.rect, boxStyle);
						GUI.Box (notificationBoxSettings.imgBox, message.image, imageBoxStyle);
						//same style as BoxStyle but without the background
						GUIStyle tmp = new GUIStyle (boxStyle);
						tmp.normal.background = null;
						GUI.Box (new Rect (notificationBoxSettings.imgBox.width, 0, notificationBox.rect.width - notificationBoxSettings.imgBox.width, notificationBox.rect.height), message.text, tmp);
						GUI.EndGroup ();
				}
		}

		//Edit Here - B
		//
		//
		//Edit Here - E
}
