#if SteamSDK && SteamModules && STEAMWORKS_NET
#define HAS_STEAM
#endif

using System.Collections;
using System.Collections.Generic;
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


	private void Awake()
	{
		var childObj = new GameObject("SteamManager");
		steamManager = childObj.AddComponent<SteamManager>();
	}

	private void Start()
	{
		//var moduleTest = new SteamTestsModule();

		//moduleTest.LogDisplayName();

		callbackModule = new SteamCallbacksModule();
		callbackModule.Init();
	}
#endif


	public void OpenWishlist()
	{
#if HAS_STEAM
		steamManager.OpenWishlistPage();
#else
		Debug.Log($"Opening the hyperlink: {steamHyperlink}");
		Application.OpenURL(steamHyperlink);
#endif
	}
}
