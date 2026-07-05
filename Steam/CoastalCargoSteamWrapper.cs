#if SteamSDK && SteamModules && STEAMWORKS_NET
#define HAS_STEAM
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastalCargoSteamWrapper : MonoBehaviour
{
#if HAS_STEAM
	public SteamManager steamManager { get; private set; }

	private void Awake()
	{
		var childObj = new GameObject("SteamManager");
		steamManager = childObj.AddComponent<SteamManager>();
	}

	private void Start()
	{
		var moduleTest = new SteamTestsModule();

		moduleTest.LogDisplayName();
	}
#endif
}
