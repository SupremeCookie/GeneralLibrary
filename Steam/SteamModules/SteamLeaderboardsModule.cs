using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


// More Info:
// https://partner.steamgames.com/doc/features/leaderboards
public class SteamLeaderboardsModule : ISteamLeaderboards
{
	// What do we need to do:
	// - I want to display friends, and global leaderboards in 2 separate lists
	// Always start on global leaderboads
	// - First we should test with getting leaderboards asynchronously when we demand them of a level in the leaderboards menu
	// - After a level ends we need to store the score (if it is a new highscore) in the leaderboards
	// - When the game starts we need a delayed call to store the current highscores to the leaderboards

	public void Init()
	{
		if (!SteamManager.Initialized)
		{
			Debug.Log("Trying to setup the SteamCallbacksModule, but steammanager is not initialized");
			return;
		}

		//overlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
	}

	public void AddToLeaderboard(string leaderboardName, int score)
	{

	}

	public List<int> GetScores(string leaderboardName)
	{
		return new List<int>();
	}


#if UNITY_EDITOR
	string testLeaderboardKey = "TestingLeaderboard";
	string testLeaderboardKey_2 = "TestingLeaderboard2";
	string testLeaderboardKey_3 = "TestingLeaderboard3";

	private void PrintLeaderboard(string leaderboardName)
	{
		var scores = GetScores(leaderboardName);

		Debug.Log($"Scores for [{leaderboardName}]:  {scores}");
	}


	public void DrawControls()
	{
		GUILayout.Space(5);

		GUILayout.BeginVertical(EditorStyles.helpBox);

		GUILayout.Label($"Steam Leaderboards Debug Controls");

		GUILayout.Space(5);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button($"Set (1-10) points for [Leaderboard {testLeaderboardKey}]")) { AddToLeaderboard(testLeaderboardKey, Random.Range(1, 10)); }
		if (GUILayout.Button($"Set (1-10) points for [Leaderboard {testLeaderboardKey_2}]")) { AddToLeaderboard(testLeaderboardKey_2, Random.Range(1, 10)); }
		if (GUILayout.Button($"Set (1-10) points for [Leaderboard {testLeaderboardKey_3}]")) { AddToLeaderboard(testLeaderboardKey_3, Random.Range(1, 10)); }
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button($"Print points for [Leaderboard {testLeaderboardKey}]")) { PrintLeaderboard(testLeaderboardKey); }
		if (GUILayout.Button($"Print points for [Leaderboard {testLeaderboardKey_2}]")) { PrintLeaderboard(testLeaderboardKey_2); }
		if (GUILayout.Button($"Print points for [Leaderboard {testLeaderboardKey_3}]")) { PrintLeaderboard(testLeaderboardKey_3); }
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
	}
#endif
}
