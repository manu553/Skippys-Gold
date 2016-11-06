using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour
{
		public Texture2D image;
		public AudioClip audioclip = new AudioClip ();
		public string text = "";

		void Start ()
		{
				Notification.Instance.setMsg (text);
		}

		void OnGUI ()
		{
				GUILayout.BeginArea (new Rect (10, 100, 600, 400));
				GUILayout.BeginHorizontal ();
				GUILayout.BeginVertical ();
				GUILayout.Label ("Scene");
				if (GUILayout.Button ("Achievement Screen")) {
						Application.LoadLevel ("Achievements");
				}
				GUILayout.EndVertical ();
				GUILayout.BeginVertical ();
				GUILayout.Label ("Notification");
				if (GUILayout.Button ("Normal notification")) {
						Notification.Instance.setMsg ("Test Notification");
				}
				if (GUILayout.Button ("With image")) {
						Notification.Instance.setMsg ("Image Test!", image);
				}
				if (GUILayout.Button ("With different sound")) {
						Notification.Instance.setMsg ("Sound Test!", audioclip);
				}
				if (GUILayout.Button ("With image + different sound")) {
						Notification.Instance.setMsg ("Woa this is AWESOME!", image, audioclip);
				}
				text = GUILayout.TextField (text);
				if (GUILayout.Button ("Test your own text")) {
						Notification.Instance.setMsg (text);
				}
				if (GUILayout.Button ("Test your own text + image")) {
						Notification.Instance.setMsg (text, image, audioclip);
				}
				if (GUILayout.Button ("Talk to NPC")) {
						Notification.Instance.setMsg ("This is to demonstrate delay notification");
						Notification.Instance.delayMsg ("I used to be an adventurer like you...", 5);
						Notification.Instance.delayMsg ("...Then I took an arrow in the knee :(", 10);	
				}
				GUILayout.EndVertical ();
				GUILayout.BeginVertical ();
				GUILayout.Label ("Achievement");
				if (GUILayout.Button ("Level up")) {
						AchievementManager.Instance.AddProgressToAchievement ("arrow", 1);
				}
				if (GUILayout.Button ("Reset data")) {
						AchievementManager.Instance.ResetProgress ();
						Notification.Instance.setMsg ("Achievement progress cleared");
				}
				GUILayout.EndVertical ();
				GUILayout.EndHorizontal ();
				GUILayout.EndArea ();
		}
}
