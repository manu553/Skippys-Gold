using System.Linq;
using UnityEngine;
using System.Collections;

/*
 * Credit to: Steve Gargolinski (Original Framework)
 * (from Progress – A Free Achievement Framework for Unity: http://www.stevegargolinski.com/progress-a-free-achievement-framework-for-unity/)
 * 
 * Changelog from original:
 * 	+ Separate GUI element to a whole different class
 * 	+ Apply Singleton pattern
 *  + Add autosave feature using PlayerPref (with the help of PlayerPrefSerializer class)
 * 	+ Intergrate with Notification system
 *  + Some minor inprovement
 */

[System.Serializable]
public class Achievement
{
		public string codename;
		public string name;
		public string description;
		public Texture2D iconIncomplete;
		public Texture2D iconComplete;
		public int rewardPoints;
		public float currentProgress = 0.0f;
		public float targetProgress;
		public bool secret;

		[HideInInspector]
		public bool
				earned = false;
	
		public bool AddProgress (float progress)
		{
				if (earned) {
						return false;
				}

				currentProgress += progress;
				if (currentProgress >= targetProgress) {
						Achieved ();
						return true;
				}

				return false;
		}

		public bool SetProgress (float progress)
		{
				if (earned) {
						return false;
				}

				currentProgress = progress;
				if (progress >= targetProgress) {
						Achieved ();
						return true;
				}

				return false;
		}

		public void Reset ()
		{
				currentProgress = 0;
				earned = false;
		}

		public void Achieved ()
		{
				earned = true;
		}
}

//This class is a simplified version of achievement for saving
[System.Serializable]
public class AchievementProgress
{
		public float[] currentProgress;

		public AchievementProgress (Achievement[] a)
		{
				this.currentProgress = new float[a.Count ()];
				for (int i =0; i<a.Count (); i++) {
						this.currentProgress [i] = a [i].currentProgress;
				}
		}

		public Achievement[] UpdateAchievement (Achievement[] a)
		{
				for (int i =0; i<a.Count (); i++) {
						a [i].SetProgress (this.currentProgress [i]);
				}
				return a;
		}
}
	

public class AchievementManager : MonoBehaviour
{
		public Achievement[] achievements;
		public string achievedText = " Unlocked!";
		public AudioClip earnedSound;
		public bool autosave = false;
		public bool debugLog = false;
		private int currentRewardPoints = 0;
		private int potentialRewardPoints = 0;
		private int currentUnlockedAchievements = 0;

		private static AchievementManager instance;

		public static AchievementManager Instance {
				get {
						return instance;
				}
		}

		public int CurrentRewardPoints {
				get {
						return this.currentRewardPoints;
				}
		}

		public int PotentialRewardPoints {
				get {
						return this.potentialRewardPoints;
				}
		}

		public int CurrentUnlockedAchievements {
				get {
						return this.currentUnlockedAchievements;
				}
		}

		void Awake ()
		{
				if (Instance != null && Instance != this) {
						Destroy (gameObject);
				}
				instance = this;

				LoadAchievements ();
				ValidateAchievements ();
				UpdateRewardPointTotals ();
		}

		public void LoadAchievements ()
		{
				AchievementProgress tmp = PlayerPrefsSerializer.Load <AchievementProgress> ("Achievements");
				if (tmp != null && tmp.currentProgress.Count () == achievements.Count ()) {
						achievements = tmp.UpdateAchievement (achievements);
						if (debugLog)
								print ("Achievements loaded!");
				}
		}

		public void SaveAchievements ()
		{
				AchievementProgress tmp = new AchievementProgress (achievements);
				PlayerPrefsSerializer.Save ("Achievements", tmp);
				if (debugLog)
						print ("Achievements Saved!");
		}
	
		// Make sure the setup assumptions we have are met.
		private void ValidateAchievements ()
		{
				ArrayList usedNames = new ArrayList ();
				foreach (Achievement achievement in achievements) {
						if (achievement.rewardPoints < 0) {
								Debug.LogError ("AchievementManager::ValidateAchievements() - Achievement with negative RewardPoints! " + achievement.codename + " gives " + achievement.rewardPoints + " points!");
						}

						if (usedNames.Contains (achievement.codename)) {
								Debug.LogError ("AchievementManager::ValidateAchievements() - Duplicate achievement names! " + achievement.codename + " found more than once!");
						}
						usedNames.Add (achievement.codename);
				}
		}

		private Achievement GetAchievementByCodeName (string achievementCodeName)
		{
				return achievements.FirstOrDefault (achievement => achievement.codename == achievementCodeName);
		}

		private void UpdateRewardPointTotals ()
		{
				currentRewardPoints = 0;
				potentialRewardPoints = 0;
				currentUnlockedAchievements = 0;

				foreach (Achievement achievement in achievements) {
						if (achievement.earned) {
								currentUnlockedAchievements++;
								currentRewardPoints += achievement.rewardPoints;
						}

						potentialRewardPoints += achievement.rewardPoints;
				}
		}

		private void AchievementEarned (Achievement achievement)
		{
				if (Notification.Instance != null)
						Notification.Instance.setMsg (achievement.name + achievedText, achievement.iconComplete, earnedSound);
				UpdateRewardPointTotals ();
		}

		public void AddProgressToAchievement (string achievementCodeName, float progressAmount)
		{
				Achievement achievement = GetAchievementByCodeName (achievementCodeName);
				if (achievement == null) {
						Debug.LogWarning ("AchievementManager::AddProgressToAchievement() - Trying to add progress to an achievement that doesn't exist: " + achievementCodeName);
						return;
				}

				if (achievement.AddProgress (progressAmount)) {
						if (Notification.Instance != null)
								Notification.Instance.setMsg (achievement.codename + achievedText, achievement.iconComplete, earnedSound);
						AchievementEarned (achievement);
				}

				if (autosave)
						SaveAchievements ();
		}

		public void AddProgressToAchievement (int index, float progressAmount)
		{
				if (index < 0 || index >= achievements.Count ()) {
						Debug.LogWarning ("AchievementManager::AddProgressToAchievement() - Trying to add progress to an achievement that doesn't exist: " + index);
						return;
				}
				//print (Achievements [index].Name);
				if (achievements [index].AddProgress (progressAmount)) {
						AchievementEarned (achievements [index]);
				}

				if (autosave)
						SaveAchievements ();
		}

		public void SetProgressToAchievement (string achievementCodeName, float newProgress)
		{
				Achievement achievement = GetAchievementByCodeName (achievementCodeName);
				if (achievement == null) {
						Debug.LogWarning ("AchievementManager::SetProgressToAchievement() - Trying to add progress to an achievement that doesn't exist: " + achievementCodeName);
						return;
				}

				if (achievement.SetProgress (newProgress)) {
						AchievementEarned (achievement);
				}

				if (autosave)
						SaveAchievements ();
		}

		public void SetProgressToAchievement (int index, float newProgress)
		{
				if (achievements [index] == null) {
						Debug.LogWarning ("AchievementManager::SetProgressToAchievement() - Trying to add progress to an achievement that doesn't exist: " + index);
						return;
				}
		
				if (achievements [index].SetProgress (newProgress)) {
						AchievementEarned (achievements [index]);
				}

				if (autosave)
						SaveAchievements ();
		}

		public void ResetProgress ()
		{
				for (int i=0; i<achievements.Count(); i++) {
						achievements [i].Reset ();
				}
				UpdateRewardPointTotals ();
				if (autosave)
						SaveAchievements ();
		}

		//Edit Here - B
		//
		//
		//Edit Here - E
}
