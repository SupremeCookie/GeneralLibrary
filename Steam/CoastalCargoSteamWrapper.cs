#if SteamSDK && SteamModules && STEAMWORKS_NET
#define HAS_STEAM
#endif

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
#else
	private SteamStatsAndAchievementsModuleDummy statsAndAchievementsModule = new SteamStatsAndAchievementsModuleDummy();
	public ISteamStatsAchievements statsAchievements => statsAndAchievementsModule;
	public bool hasStatsAchievementsModule => statsAndAchievementsModule != null;
#endif


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
