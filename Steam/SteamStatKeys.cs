using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public enum SteamStatKeys
{
	STAT_BOATS_PLACED_TOTAL,
	STAT_BOATS_PLACED_SLOOP,
	STAT_BOATS_PLACED_FRIGATES,
	STAT_BOATS_PLACED_CARAVEL,
	STAT_BOATS_PLACED_POST,
	STAT_BOATS_PLACED_LICHTER,
	STAT_ROUTES_PLACED_TOTAL,
	STAT_CARGO_DELIVERED_TOTAL,
	STAT_PIRATES_SANK_TOTAL,
	STAT_ADMIRALS_SANK_TOTAL,
	STAT_MUTINIES_STOPPED_TOTAL,
	STAT_ROTTENPLANT_TOTAL,
	STAT_CAMPAIGNS_PLAYED_1,
	STAT_CAMPAIGNS_GOLD_1,
	STAT_CAMPAIGNS_PLAYED_2,
	STAT_CAMPAIGNS_GOLD_2,
}

public static class SteamStatKeysExtension
{
	public static string ConvertToSteamStatKey(this StatKey key)
	{
		switch (key)
		{
			case StatKey.None:
			default: { return SteamStatsAndAchievementsModule.INVALID_STAT_KEY_NAME; }

			case StatKey.BoatsPlaced_Caravel: { return SteamStatKeys.STAT_BOATS_PLACED_CARAVEL.ToString(); }
			case StatKey.BoatsPlaced_Frigate: { return SteamStatKeys.STAT_BOATS_PLACED_FRIGATES.ToString(); }
			case StatKey.BoatsPlaced_Sloop: { return SteamStatKeys.STAT_BOATS_PLACED_SLOOP.ToString(); }
			case StatKey.BoatsPlaced_Lichter: { return SteamStatKeys.STAT_BOATS_PLACED_LICHTER.ToString(); }
			case StatKey.BoatsPlaced_Post: { return SteamStatKeys.STAT_BOATS_PLACED_POST.ToString(); }
			case StatKey.Amount_RoutesCreated: { return SteamStatKeys.STAT_ROUTES_PLACED_TOTAL.ToString(); }
			case StatKey.Amount_PackagesDelivered: { return SteamStatKeys.STAT_CARGO_DELIVERED_TOTAL.ToString(); }
			case StatKey.Events_PirateBoatsSank: { return SteamStatKeys.STAT_PIRATES_SANK_TOTAL.ToString(); }
			case StatKey.Events_AdmiralBoatsSank: { return SteamStatKeys.STAT_ADMIRALS_SANK_TOTAL.ToString(); }
			case StatKey.Events_MutiniesStopped: { return SteamStatKeys.STAT_MUTINIES_STOPPED_TOTAL.ToString(); }
			case StatKey.Events_PlantationsCured: { return SteamStatKeys.STAT_ROTTENPLANT_TOTAL.ToString(); }
		}
	}
}


public static class SteamStatKeysUtil
{
	public static List<string> GetAllNames()
	{
		var allKeys = Enum.GetNames(typeof(SteamStatKeys));
		if (allKeys == null || allKeys.Length == 0)
		{
			Debug.LogError($"[SteamStatKeys] No statkey names can be gotten from this method");
			return null;
		}

		return new List<string>(allKeys);
	}
}