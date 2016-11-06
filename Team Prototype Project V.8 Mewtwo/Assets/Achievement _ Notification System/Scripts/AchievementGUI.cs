using UnityEngine;
using System.Collections;

public class AchievementGUI : MonoBehaviour
{
		private Vector2 achievementScrollviewLocation = Vector2.zero;
		public GUIStyle GUIStyleAchievementEarned;
		public GUIStyle GUIStyleAchievementNotEarned;
		public Rect panelPosition = new Rect (200.0f, 5.0f, 200.0f, 25.0f);

		void OnGUI ()
		{
				float yValue = 5.0f;
				float achievementGUIWidth = 500.0f;
		
				GUI.Label (panelPosition, "-- Achievements --");
		
				// Setup a scrollview, and then fill it with each achievement in our list.
		
				achievementScrollviewLocation = GUI.BeginScrollView (new Rect (0.0f, 25.0f, achievementGUIWidth + 25.0f, 400.0f), achievementScrollviewLocation,
		                                                     new Rect (0.0f, 0.0f, achievementGUIWidth, AchievementManager.Instance.achievements.Length * 80.0f));
		
				foreach (Achievement achievement in AchievementManager.Instance.achievements) {
						Rect position = new Rect (5.0f, yValue, achievementGUIWidth, 75.0f);
						itemOnGUI (achievement, position, GUIStyleAchievementEarned, GUIStyleAchievementNotEarned);
						yValue += 80.0f;
				}
		
				GUI.EndScrollView ();
		
				GUI.Label (new Rect (10.0f, 440.0f, 260.0f, 25.0f), "Unlocked: " + AchievementManager.Instance.CurrentUnlockedAchievements + "/" + AchievementManager.Instance.achievements.Length + " Reward Points: [" + AchievementManager.Instance.CurrentRewardPoints + " out of " + AchievementManager.Instance.PotentialRewardPoints + "]");
				if (GUI.Button (new Rect (270.0f, 440.0f, 50.0f, 25.0f), "Back")) {
						Application.LoadLevel ("Demo Scene");
				}
				if (GUI.Button (new Rect (330.0f, 440.0f, 50.0f, 25.0f), "Save")) {
						AchievementManager.Instance.SaveAchievements ();
				}
		}


		public void itemOnGUI (Achievement achievement, Rect position, GUIStyle GUIStyleAchievementEarned, GUIStyle GUIStyleAchievementNotEarned)
		{
				GUIStyle style = GUIStyleAchievementNotEarned;
				if (achievement.earned) {
						style = GUIStyleAchievementEarned;
				}
		
				GUI.BeginGroup (position);

				GUI.Box (new Rect (0.0f, 0.0f, position.width, position.height), "");
		
				if (achievement.earned) {
						GUI.Box (new Rect (0.0f, 0.0f, position.height, position.height), achievement.iconComplete);
				} else {
						GUI.Box (new Rect (0.0f, 0.0f, position.height, position.height), achievement.iconIncomplete);
				}
		
				GUI.Label (new Rect (80.0f, 5.0f, position.width - 80.0f - 50.0f, 25.0f), name, style);
		
				if (achievement.secret && !achievement.earned) {
						GUI.Label (new Rect (80.0f, 25.0f, position.width - 80.0f, 25.0f), "Description Hidden!", style);
						GUI.Label (new Rect (position.width - 50.0f, 5.0f, 25.0f, 25.0f), "???", style);
						GUI.Label (new Rect (position.width - 250.0f, 50.0f, 250.0f, 25.0f), "Progress Hidden!", style);
				} else {
						GUI.Label (new Rect (80.0f, 25.0f, position.width - 80.0f, 25.0f), achievement.description, style);
						GUI.Label (new Rect (position.width - 50.0f, 5.0f, 25.0f, 25.0f), achievement.rewardPoints.ToString (), style);
						GUI.Label (new Rect (position.width - 250.0f, 50.0f, 250.0f, 25.0f), "Progress: [" + achievement.currentProgress.ToString ("0.#") + " out of " + achievement.targetProgress.ToString ("0.#") + "]", style);
				}
		
				GUI.EndGroup ();
		}

		//Edit Here - B
		//
		//
		//Edit Here - E
}
