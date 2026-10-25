#define DKG_ISENABLED


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// More Info:
// https://partner.steamgames.com/doc/features/achievements
// https://partner.steamgames.com/doc/features/achievements/stats_guide
// https://partner.steamgames.com/doc/features/achievements/ach_guide
// Take a look at Global Stats, may be interesting for tracking
// Gonna have to localize the achievements
public class SteamStatsAndAchievementsModule : ISteamStatsAchievements
{
	// What do we need to do:
	// - Get or receive the currently stored stats of the save game
	// Compare it to what we have in steam, and get the highest of either and re-update the stats towards steam
	// This cache comparison should ever be done in the main game and only if SteamAPI.IsSteamRunning() is true

	// This should handle desyncs between demo and main game as well
	// - Some achievements are stat based, but some are stat and experience based, for example placing 10 boats should only happen after having done that action yourself
	// - Trigger achievements if the stats are backing up the achievement
	// - Updating stats when something happens
	// This last one needs to happen all the time, it is purely in-memory and steam handles stuff even when crashing and stuff
	// - From time to time call StoreStats, this should definitely happen when going from level to main menu, but also when pressing the quit button in the main menu, right before opening the confirmation dialogue
	// - After calling SetAchievement to unlock an achievement, also call StoreStats


	// Some stats:
	//  (global-can repurpose as 'tracking')
	//		For each type of boat, how many we've placed, then a running total called total boats placed based on those values
	//  (global-can repurpose as 'tracking')
	//		For each level, how often it's been started, and for each rank including none how often it's been finished
	//	Amount of routes created
	//  Amount of routes edited
	//  Amount of packages delivered
	//  Amount of events had, including every specific event type, so the total is an aggregate of each specific type
	//	Boats picked up after having been placed down
	//	Pirate boats sank, Admirals sank, Mutinies stopped

	//  Some achievements:
	//  Reaching the tutorial school halfway point + finishing it fully
	//  Unlocking each campaign
	//  Finishing a campaign fully on bronze, fully on silver, fully on gold
	//	Having started each of a campaign's levels
	//  Entering a campaign level for the first time
	//	boats placed, routes placed, boats picked up, routes edited
	//	Packages delivered
	//	Having accomplished events (pirates/admirals/mutinies)


	// Some thoughts:
	//Definitely need to add stats to the game (even in demo)
	//	more than I may need
	//	Let the game's stats be leading, we've got cloud saves anyway
	//	Then upload those stats to the steam(not in demo)
	//And have that contribute to unlocking achievements
	//That said, ALWAYS trigger stat based achievements after accomplishing the thing atleast 1 more time(so you don't get random achievements)
	//Do unlock "story" based progression when starting up again, if the achievement is to accomplish full gold stars in campaign 1, then don't make them play another level

	//> At the start of a game session, call ISteamUserStats::RequestCurrentStats to fetch the user's stats and achievement data from the Steam back end. You will receive a ISteamUserStats::UserStatsReceived_t callback when the data is ready.
	//This is important, but always keep highest stat

	public const string INVALID_STAT_KEY_NAME = "INVALID";


	private bool succeededLocalInit;
	private int retryLocalInit = 0;
	private bool succeededSteamInit;
	private int retrySteamInit = 0;

	private bool commitAllLocalStatsToSteam = false;

	private List<string> allLocalStatNames = null;
	private List<string> allSteamStatNames = null;

	private IStatsAchievementsDataIO localData;


	public void ShouldCommitAllLocalStatsToSteam()
	{
		commitAllLocalStatsToSteam = true;
	}

	public void SetAllStatNames(List<string> localStatNames, List<string> steamStatNames)
	{
		//Debug.Log($"[SteamStats] -- Local and Steam stats same length? [{localStatNames.Count}, {steamStatNames.Count}]");
		//for (int i = 0; i < localStatNames.Count; ++i)
		//{
		//	Debug.Log($"    [SteamStats] -- key: [{localStatNames[i]}, {steamStatNames[i]}]");
		//}


		this.allLocalStatNames = localStatNames;
		this.allSteamStatNames = steamStatNames;
	}


	public void CommitStatsChangesToSteam()
	{
		if (!SteamManager.Initialized)
			return;

		Debug.Log($"[SteamStats] -- Committing stats to steam");
		Steamworks.SteamUserStats.StoreStats();
	}


	public void InitLocally(IStatsAchievementsDataIO localData)// , List<string> statKeys)
	{
		this.localData = localData;
		succeededLocalInit = TryToInitLocalStats();

		if (!succeededLocalInit)
			retryLocalInit = 5;

		UnityEngine.Debug.Log($"[SteamStats] -- Tried initializing localdata, result: [local: {succeededLocalInit}]");
	}

	public void InitSteam()
	{
		succeededSteamInit = TryToInitSteamStats();

		UnityEngine.Debug.Log($"[SteamStats] -- Tried initializing steam data, result: [steam: {succeededSteamInit}]");

		if (!succeededSteamInit)
			retrySteamInit = 5;
	}

	public void Update()
	{
		if (!succeededLocalInit)
		{
			if (retryLocalInit > 0)
				retryLocalInit--;

			if (retryLocalInit <= 0)
			{
				succeededLocalInit = TryToInitLocalStats();
				if (!succeededLocalInit)
					retryLocalInit = 10;
			}
		}

		if (!succeededSteamInit)
		{
			if (retrySteamInit > 0)
				retrySteamInit--;

			if (retrySteamInit <= 0)
			{
				succeededSteamInit = TryToInitSteamStats();
				if (!succeededSteamInit)
					retrySteamInit = 10;
			}
		}


		if (succeededLocalInit && succeededSteamInit && commitAllLocalStatsToSteam)
		{
			GetAndCommitAllLocalStatsToSteam();
		}
	}

	private bool TryToInitLocalStats()
	{
		return true;
	}

	private bool TryToInitSteamStats()
	{
		return true;
	}


	private void GetAndCommitAllLocalStatsToSteam()
	{
		if (localData == null)
			return;

		if (allLocalStatNames == null || allLocalStatNames.Count == 0)
			return;

		if (allSteamStatNames == null || allSteamStatNames.Count == 0)
			return;

		for (int i = 0; i < allLocalStatNames.Count; ++i)
		{
			PostLocalStatToSteam(allLocalStatNames[i], allSteamStatNames[i]);
		}

		commitAllLocalStatsToSteam = false;
	}



	// Interface implementations

	public void IncreaseStat(string statKey, string steamStatKey, int amount, out int newValue)
	{
#if !DKG_ISENABLED
		newValue = -1;
		return;
#endif

		UnityEngine.Debug.Log($"[SteamStats] Doing an increase for stat: {statKey}, increase: {amount}");

		if (statKey.Equals(INVALID_STAT_KEY_NAME))
		{
			newValue = -1;
			return;
		}

		if (!succeededLocalInit)
		{
			newValue = -1;
			Debug.LogError($"No initialization of steam and local inits have been accomplished yet, please fix");
			return;
		}

		IncreaseStat_Locally(statKey, amount, out newValue);

		if (!succeededSteamInit)
		{
			return;
		}

		if (steamStatKey.Equals(INVALID_STAT_KEY_NAME))
		{
			return;
		}

		PostLocalStatToSteam(statKey, steamStatKey);
	}

	// Note DK: These will be used to "unlock singular achievements" so everything is technically stat based
	// singular happenings simply have a 0-1 stat increase only, and then the achievement unlocks.
	public void CompleteStat(string statKey, string steamStatKey)
	{
		if (statKey.Equals(INVALID_STAT_KEY_NAME))
			return;

		if (!succeededLocalInit)
		{
			Debug.LogError($"No initialization of steam and local inits have been accomplished yet, please fix");
			return;
		}

		CompleteStat_Locally(statKey);

		if (!succeededSteamInit)
		{
			return;
		}

		if (steamStatKey.Equals(INVALID_STAT_KEY_NAME))
		{
			return;
		}

		PostLocalStatToSteam(statKey, steamStatKey);
	}


	private void CompleteStat_Locally(string statKey)
	{
		localData.SetValue(statKey, 1);
	}

	private void IncreaseStat_Locally(string statKey, int amount, out int newValue)
	{
		if (localData == null)
		{
			Debug.LogWarning($"No local data known, the initialisation failed. Likely steam is not instantiated?");
			newValue = -1;
			return;
		}

		var currentValue = localData.GetValue(statKey, amount);

		if (int.MaxValue - currentValue < amount)
		{
			newValue = int.MaxValue;
			return;
		}

		currentValue += amount;
		localData.SetValue(statKey, currentValue);

		newValue = currentValue;
	}

	private void PostLocalStatToSteam(string localStatKey, string steamStatKey)
	{
		if (!SteamManager.Initialized)
			return;

		if (localData == null)
			return;

		var value = localData.GetValue(localStatKey, 0);
		Steamworks.SteamUserStats.SetStat(steamStatKey, value);
	}


	public void PostStatToSteam(string steamStatKey, int value)
	{
		if (!SteamManager.Initialized)
			return;

		Steamworks.SteamUserStats.SetStat(steamStatKey, value);
	}


	public void EvaluateAchievementUnlock(string achievementKey, string steamStatKey, int threshold)
	{
		if (!SteamManager.Initialized)
			return;

		var gotValue = Steamworks.SteamUserStats.GetStat(steamStatKey, out int steamValue);
		if (!gotValue)
			return;

		if (steamValue >= threshold)
		{
			var gotAchData = Steamworks.SteamUserStats.GetAchievement(achievementKey, out bool isAchieved);

			Debug.Log($"[SteamStats] -- Acomplished achievement: {achievementKey},  [currentVal: {steamValue}]  [threshold: {threshold}]");
			Debug.Log($"[SteamStats] -- Commit booleans: got data: {gotAchData},  is achievend: {isAchieved}");
			Steamworks.SteamUserStats.SetAchievement(achievementKey);

			if (!gotAchData || !isAchieved)
				CommitStatsChangesToSteam();
		}
	}


#if UNITY_EDITOR
	int testIntStat = 0;
	string testIntStatKey = "TestingIntKey";


	public void DrawControls()
	{
		GUILayout.Space(5);

		GUILayout.BeginVertical(EditorStyles.helpBox);

		GUILayout.Label($"Steam Stats Debug Controls");
		GUILayout.Label($"Do we have local data: {localData != null}");

		GUILayout.Space(5);

		// TODO DK: Setup a bunch of buttons to handle displaying, and increasing stats
		// Base them on the expected stats we're gonna have
		// Don't bother about the achievements stuff yet
		// Then once the local data stuff works well, in between runs and outside runs, setup stats in steam itself
		// And test those out
		// Gotta get myself a dev key maybe?

		GUILayout.BeginHorizontal();
		GUILayout.Label($"[Int stat: {testIntStat}]");
		if (GUILayout.Button("Load current value")) { testIntStat = localData.GetValue(testIntStatKey, 0); }
		if (GUILayout.Button("Increase current value")) { IncreaseStat(testIntStatKey, INVALID_STAT_KEY_NAME, 1, out testIntStat); }
		GUILayout.EndHorizontal();


		GUILayout.Space(15);

		GUILayout.BeginHorizontal(EditorStyles.helpBox);
		if (GUILayout.Button("Reset all steam stats and achievements"))
		{
			var success = Steamworks.SteamUserStats.ResetAllStats(bAchievementsToo: true);
			Debug.Log($"Did we succeed?: {success}");
		}

		GUILayout.Space(15);

		if (GUILayout.Button($"Upload Current State"))
		{
			GetAndCommitAllLocalStatsToSteam();
			CommitStatsChangesToSteam();
		}

		GUILayout.Space(5);

		EditorGUI.BeginDisabledGroup(allSteamStatNames == null || allSteamStatNames.Count == 0);
		if (GUILayout.Button("Request Current Stats"))
		{
			Debug.Log($"[SteamStats] -------------------- ");
			for (int i = 0; i < allSteamStatNames.Count; ++i)
			{
				bool success = Steamworks.SteamUserStats.GetStat(allSteamStatNames[i], out int setData);
				if (success)
					Debug.Log($"[SteamStats] (Get Stat) id: {allSteamStatNames[i]},  value: {setData}");
				else
					Debug.Log($"[SteamStats] (Failed) id: {allSteamStatNames[i]}");
			}
			Debug.Log($"[SteamStats] -------------------- ");
		}
		EditorGUI.EndDisabledGroup();

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
	}
#endif
}

// - Development options include:
// - Resetting al Stats and Achievements using a button
