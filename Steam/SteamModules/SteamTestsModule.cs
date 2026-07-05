using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class SteamTestsModule
{
	private Callback<GameOverlayActivated_t> overlayActivated;

	public SteamTestsModule()
	{
		if (!SteamManager.Initialized)
		{
			Log("Trying to setup the SteamTestsModule, but steammanager is not initialized");
			return;
		}

		overlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
	}


	public void LogDisplayName()
	{
		if (!SteamManager.Initialized)
		{
			Log("Get Display name for player, steam manager is not initialized");
			return;
		}

		string displayName = SteamFriends.GetPersonaName();
		Log($"Name: {displayName}");
	}

	private void OnGameOverlayActivated(GameOverlayActivated_t callback)
	{
		if (callback.m_bActive != 0)
		{
			Log("Steam Overlay has been activated");
		}
		else
		{
			Log("Steam Overlay has been closed");
		}
	}


	private void Log(string msg)
	{
		Debug.Log($"[SteamTestsModule]  {msg}");
	}
}
