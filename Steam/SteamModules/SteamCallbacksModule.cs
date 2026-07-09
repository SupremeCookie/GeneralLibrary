using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class SteamCallbacksModule : ISteamCallbacks
{
	// What do we need to do:
	// - Each callback needs to have a register and deregister with a string KEY and system.action<ICallback> attached so we can send along info if needed
	// - I want to detect when the steam overlay opens and closes, so I can trigger stuff
	// https://partner.steamgames.com/doc/features/overlay

	private Dictionary<string, System.Action<OverlayMode>> onOverlaySwitched = new Dictionary<string, System.Action<OverlayMode>>();

	private Callback<GameOverlayActivated_t> overlayActivated;


	public void Init()
	{
		if (!SteamManager.Initialized)
		{
			Debug.Log("Trying to setup the SteamTestsModule, but steammanager is not initialized");
			return;
		}

		overlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
	}


	// ===== STEAM CALLBACKS =====
	private void OnGameOverlayActivated(GameOverlayActivated_t callback)
	{
		if (callback.m_bActive != 0)
		{
			Debug.Log("Steam Overlay has been activated");
			if (onOverlaySwitched != null && onOverlaySwitched.Count > 0)
			{
				foreach (var cb in onOverlaySwitched)
					cb.Value?.Invoke(OverlayMode.Open);
			}
		}
		else
		{
			Debug.Log("Steam Overlay has been closed");
			if (onOverlaySwitched != null && onOverlaySwitched.Count > 0)
			{
				foreach (var cb in onOverlaySwitched)
					cb.Value?.Invoke(OverlayMode.Close);
			}
		}
	}


	// ===== GAME CALLBACK REGISTERS =====
	public bool SubscribeCallback_OnOverlayMode(string key, System.Action<OverlayMode> callback)
	{
		if (onOverlaySwitched.ContainsKey(key))
			return false;

		onOverlaySwitched.Add(key, callback);
		return true;
	}

	public void UnSubscribeCallback_OnOverlayMode(string key)
	{
		if (!onOverlaySwitched.ContainsKey(key))
			return;

		onOverlaySwitched.Remove(key);
	}
}
