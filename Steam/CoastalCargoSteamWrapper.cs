#if SteamSDK && SteamModules && STEAMWORKS_NET
#define HAS_STEAM
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoastalCargoSteamWrapper : MonoBehaviour
{
	public const string steamHyperlink = "https://store.steampowered.com/app/4549920/Coastal_Cargo/";


#if HAS_STEAM
	public SteamManager steamManager { get; private set; }
	public bool managerHasInitialized => SteamManager.Initialized;


	private SteamCallbacksModule callbackModule;
	public ISteamCallbacks callBacks => callbackModule;
	public bool hasCallbackModule => callbackModule != null;

	private SteamStatsAndAchievementsModule statsAndAchievementsModule;
	public ISteamStatsAchievements statsAchievements => statsAndAchievementsModule;
	public bool hasStatsAchievementsModule => statsAndAchievementsModule != null;

	private IStatsAchievementsDataIO statsAchievementsLocalData;
	private bool statsAchievementsLocalDataIsDirty;
	private bool statsAchievementsSteamDataIsDirty;

	private bool hasDoneAdditionalInitialization = false;


	private void Awake()
	{
		var childObj = new GameObject("SteamManager");
		steamManager = childObj.AddComponent<SteamManager>();
	}

	private void Start()
	{
		//var moduleTest = new SteamTestsModule();
		//moduleTest.LogDisplayName();


		// Pre-IO init
		callbackModule = new SteamCallbacksModule();
		callbackModule.Init();

		// Post-IO init, will instantiate, but no Init yet
		statsAndAchievementsModule = new SteamStatsAndAchievementsModule();
	}

	private void Update()
	{
		if (statsAchievementsLocalDataIsDirty)
		{
			if (statsAndAchievementsModule != null)
			{
				UnityEngine.Debug.Log($"[SteamStats] -- Calling [Local] Init on the stats and achievements module");
				statsAndAchievementsModule.InitLocally(statsAchievementsLocalData);
				statsAchievementsLocalDataIsDirty = false;
			}
		}

		if (statsAchievementsSteamDataIsDirty)
		{
			if (statsAndAchievementsModule != null)
			{
				UnityEngine.Debug.Log($"[SteamStats] -- Calling [Steam] Init on the stats and achievements module");
				statsAndAchievementsModule.InitSteam();
				statsAchievementsSteamDataIsDirty = false;
			}
		}

		if (statsAndAchievementsModule != null)
			statsAndAchievementsModule.Update();


		if (hasDoneAdditionalInitialization)
			return;

		var localKeys = StatKeyUtil.GetAllKeys();
		var localKeyStrings = new List<string>(localKeys.Count);
		var steamKeys = new List<string>(localKeys.Count);
		for (int k = localKeys.Count - 1; k >= 0; --k)
		{
			if (localKeys[k].ConvertToSteamStatKey() == SteamStatsAndAchievementsModule.INVALID_STAT_KEY_NAME)
				localKeys.RemoveAt(k);
		}

		// Only add localkeys converted to steamkeys to the steamkeys collections
		for (int k = 0; k < localKeys.Count; ++k)
		{
			steamKeys.Add(localKeys[k].ConvertToSteamStatKey().ToString());
			localKeyStrings.Add(localKeys[k].ToString());
		}

		// Then we add the missing steam keys so that we will have the full picture available for debug purposes
		// BUT the indices from local to steam are still linked this way
		var allSteamKeys = Enum.GetNames(typeof(SteamStatKeys));
		for (int k = 0; k < allSteamKeys.Length; ++k)
		{
			if (steamKeys.Contains(allSteamKeys[k].ToString()))
				continue;

			steamKeys.Add(allSteamKeys[k].ToString());
		}


		statsAndAchievementsModule.SetAllStatNames(localKeyStrings, steamKeys);

#if !DEMO_BUILD
		statsAndAchievementsModule.ShouldCommitAllLocalStatsToSteam();
		UpdateStatFor_TotalBoatsPlaced();
#endif

		hasDoneAdditionalInitialization = true;
	}

	public void SetStatsAchievementsLocalData(IStatsAchievementsDataIO data)
	{
		UnityEngine.Debug.Log($"[SteamStats] -- Storing the local data and dirtying the flag");
		this.statsAchievementsLocalData = data;
		statsAchievementsLocalDataIsDirty = true;
	}

	public void SetStatsAchievementsSteamInitDirty()
	{
		statsAchievementsSteamDataIsDirty = true;
	}


	public void UpdateStatFor_TotalBoatsPlaced()
	{
		Debug.LogWarning($"[SteamStats] -- UPDATE STAT FOR TOTAL BOATS PLACED");
		if (statsAchievementsLocalData == null)
			return;

		// TODO DK: Add more instances when we add more boat types.

		// keys
		var key_boat_1 = StatHelperMethods.ConvertBoatType_Placement(0);
		var key_boat_2 = StatHelperMethods.ConvertBoatType_Placement(1);
		var key_boat_3 = StatHelperMethods.ConvertBoatType_Placement(2);

		var key_boat_4 = StatHelperMethods.ConvertBoatType_Placement(11);
		var key_boat_5 = StatHelperMethods.ConvertBoatType_Placement(12);


		// values
		var boat_value_1 = statsAchievementsLocalData.GetValue(key_boat_1.ConvertToString(), 0);
		var boat_value_2 = statsAchievementsLocalData.GetValue(key_boat_2.ConvertToString(), 0);
		var boat_value_3 = statsAchievementsLocalData.GetValue(key_boat_3.ConvertToString(), 0);

		var boat_value_4 = statsAchievementsLocalData.GetValue(key_boat_4.ConvertToString(), 0);
		var boat_value_5 = statsAchievementsLocalData.GetValue(key_boat_5.ConvertToString(), 0);


		int totalBoatCount = boat_value_1 + boat_value_2 + boat_value_3 + boat_value_4 + boat_value_5;

		statsAchievements.PostStatToSteam(SteamStatKeys.STAT_BOATS_PLACED_TOTAL.ToString(), totalBoatCount);
	}

	public void UpdateStatsFor_Campaign_1()
	{
		Debug.LogWarning($"[SteamStats] -- UPDATE STAT FOR LEVELS PLAYED AND LEVELS ATTAINED ON GOLD -- CAMPAIGN 1");

		// Once a condition is completed, call CompleteStat on the SteamStatsAndAchievementsModule

		int totalPlayedScore = 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C1_1_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C1_2_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C1_3_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C1_4_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C1_5_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C1_6_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C1_7_0) > 0 ? 1 : 0;

		if (totalPlayedScore >= 7)
			statsAchievements.PostStatToSteam(SteamStatKeys.STAT_CAMPAIGNS_PLAYED_1.ToString(), 1);


		int totalGoldScored = 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C1_1_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C1_2_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C1_3_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C1_4_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C1_5_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C1_6_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C1_7_C) ? 1 : 0;

		if (totalGoldScored >= 7)
			statsAchievements.PostStatToSteam(SteamStatKeys.STAT_CAMPAIGNS_GOLD_1.ToString(), 1);


		Debug.Log($"[SteamStats] -- Campaign state, [played: {totalPlayedScore}],  [goldCompleted: {totalGoldScored}]");
	}

	public void UpdateStatsFor_Campaign_2()
	{
		Debug.LogWarning($"[SteamStats] -- UPDATE STAT FOR LEVELS PLAYED AND LEVELS ATTAINED ON GOLD -- CAMPAIGN 2");

		// Once a condition is completed, call CompleteStat on the SteamStatsAndAchievementsModule

		int totalPlayedScore = 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C2_1_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C2_2_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C2_3_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C2_4_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C2_5_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C2_6_0) > 0 ? 1 : 0;
		totalPlayedScore += GetLevelValue(StatKey.Level_Ended_C2_7_0) > 0 ? 1 : 0;

		if (totalPlayedScore >= 7)
			statsAchievements.PostStatToSteam(SteamStatKeys.STAT_CAMPAIGNS_PLAYED_2.ToString(), 1);


		int totalGoldScored = 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C2_1_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C2_2_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C2_3_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C2_4_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C2_5_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C2_6_C) ? 1 : 0;
		totalGoldScored += HasCompletedGoldLevel(StatKey.Level_Ended_C2_7_C) ? 1 : 0;

		if (totalGoldScored >= 7)
			statsAchievements.PostStatToSteam(SteamStatKeys.STAT_CAMPAIGNS_GOLD_2.ToString(), 1);


		Debug.Log($"[SteamStats] -- Campaign state, [played: {totalPlayedScore}],  [goldCompleted: {totalGoldScored}]");
	}


	private int GetLevelValue(StatKey baseStatKey)
	{
		int totalValue = statsAchievementsLocalData?.GetValue(baseStatKey.ConvertToString(), 0) ?? 0;
		totalValue += statsAchievementsLocalData?.GetValue(((StatKey)(int)(baseStatKey) + 1).ConvertToString(), 0) ?? 0;
		totalValue += statsAchievementsLocalData?.GetValue(((StatKey)(int)(baseStatKey) + 1).ConvertToString(), 0) ?? 0;
		totalValue += statsAchievementsLocalData?.GetValue(((StatKey)(int)(baseStatKey) + 1).ConvertToString(), 0) ?? 0;

		return totalValue;
	}

	private bool HasCompletedGoldLevel(StatKey levelKey)
	{
		var value = statsAchievementsLocalData?.GetValue(levelKey.ConvertToString(), 0) ?? 0;
		return value > 0;
	}

#else
	private SteamStatsAndAchievementsModuleDummy statsAndAchievementsModule = new SteamStatsAndAchievementsModuleDummy();
	public ISteamStatsAchievements statsAchievements => statsAndAchievementsModule;
	public bool hasStatsAchievementsModule => statsAndAchievementsModule != null;
#endif


	public void StoreStats()
	{
#if !DEMO_BUILD
		statsAndAchievementsModule.CommitStatsChangesToSteam();
#endif
	}

	public void OpenWishlist()
	{
#if HAS_STEAM
		bool openedOverlay = steamManager.OpenWishlistPage();
		if (openedOverlay)
			return;
#endif

		Debug.Log($"Opening the hyperlink: {steamHyperlink}");
		Application.OpenURL(steamHyperlink);
	}

	public string GetCurrentLanguage()
	{
#if HAS_STEAM
		return steamManager.GetCurrentGameLanguage();
#else
		return string.Empty;
#endif
	}



#if UNITY_EDITOR
	private bool shouldDrawStats = false;
	public void DrawControls()
	{
		shouldDrawStats = GUILayout.Toggle(shouldDrawStats, "Should draw Stats controls");
		if (shouldDrawStats && statsAndAchievementsModule != null)
			statsAndAchievementsModule.DrawControls();
	}
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(CoastalCargoSteamWrapper))]
public class CoastalCargoSteamWrapperEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(15);

		(target as CoastalCargoSteamWrapper).DrawControls();
	}
}
#endif
