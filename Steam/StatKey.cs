using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


// Note DK: Not happy this is where it is, in the dkatgames steam assembly
// But I don't have a good way of taking a more general approach
// A good solution could be to have a Dumb-Data-Assembly, where we would put these common StatKeys
// Even though they're game dependent, maybe make it a Dumb-Data-GameName-Assembly
public enum StatKey
{
	None = -1,

	BoatsPlaced_Sloop = 0,
	BoatsPlaced_Frigate,
	BoatsPlaced_Caravel,
	BoatsPlaced_Lichter,
	BoatsPlaced_Post,

	BoatsPickedUp_AfterPlacement = 20,

	#region LEVEL STARTED/ENDED
	Level_Started_C1_1 = 100,
	Level_Started_C1_2,
	Level_Started_C1_3,
	Level_Started_C1_4,
	Level_Started_C1_5,
	Level_Started_C1_6,
	Level_Started_C1_7,

	Level_Ended_C1_1_0 = 120,
	Level_Ended_C1_1_A,
	Level_Ended_C1_1_B,
	Level_Ended_C1_1_C,
	Level_Ended_C1_2_0,
	Level_Ended_C1_2_A,
	Level_Ended_C1_2_B,
	Level_Ended_C1_2_C,
	Level_Ended_C1_3_0,
	Level_Ended_C1_3_A,
	Level_Ended_C1_3_B,
	Level_Ended_C1_3_C,
	Level_Ended_C1_4_0,
	Level_Ended_C1_4_A,
	Level_Ended_C1_4_B,
	Level_Ended_C1_4_C,
	Level_Ended_C1_5_0,
	Level_Ended_C1_5_A,
	Level_Ended_C1_5_B,
	Level_Ended_C1_5_C,
	Level_Ended_C1_6_0,
	Level_Ended_C1_6_A,
	Level_Ended_C1_6_B,
	Level_Ended_C1_6_C,
	Level_Ended_C1_7_0,
	Level_Ended_C1_7_A,
	Level_Ended_C1_7_B,
	Level_Ended_C1_7_C,

	Level_Started_C2_1 = 175,
	Level_Started_C2_2,
	Level_Started_C2_3,
	Level_Started_C2_4,
	Level_Started_C2_5,
	Level_Started_C2_6,
	Level_Started_C2_7,

	Level_Ended_C2_1_0 = 195,
	Level_Ended_C2_1_A,
	Level_Ended_C2_1_B,
	Level_Ended_C2_1_C,
	Level_Ended_C2_2_0,
	Level_Ended_C2_2_A,
	Level_Ended_C2_2_B,
	Level_Ended_C2_2_C,
	Level_Ended_C2_3_0,
	Level_Ended_C2_3_A,
	Level_Ended_C2_3_B,
	Level_Ended_C2_3_C,
	Level_Ended_C2_4_0,
	Level_Ended_C2_4_A,
	Level_Ended_C2_4_B,
	Level_Ended_C2_4_C,
	Level_Ended_C2_5_0,
	Level_Ended_C2_5_A,
	Level_Ended_C2_5_B,
	Level_Ended_C2_5_C,
	Level_Ended_C2_6_0,
	Level_Ended_C2_6_A,
	Level_Ended_C2_6_B,
	Level_Ended_C2_6_C,
	Level_Ended_C2_7_0,
	Level_Ended_C2_7_A,
	Level_Ended_C2_7_B,
	Level_Ended_C2_7_C,

	Level_Started_C3_1 = 250,
	Level_Started_C3_2,
	Level_Started_C3_3,
	Level_Started_C3_4,
	Level_Started_C3_5,
	Level_Started_C3_6,
	Level_Started_C3_7,

	Level_Ended_C3_1_0 = 270,
	Level_Ended_C3_1_A,
	Level_Ended_C3_1_B,
	Level_Ended_C3_1_C,
	Level_Ended_C3_2_0,
	Level_Ended_C3_2_A,
	Level_Ended_C3_2_B,
	Level_Ended_C3_2_C,
	Level_Ended_C3_3_0,
	Level_Ended_C3_3_A,
	Level_Ended_C3_3_B,
	Level_Ended_C3_3_C,
	Level_Ended_C3_4_0,
	Level_Ended_C3_4_A,
	Level_Ended_C3_4_B,
	Level_Ended_C3_4_C,
	Level_Ended_C3_5_0,
	Level_Ended_C3_5_A,
	Level_Ended_C3_5_B,
	Level_Ended_C3_5_C,
	Level_Ended_C3_6_0,
	Level_Ended_C3_6_A,
	Level_Ended_C3_6_B,
	Level_Ended_C3_6_C,
	Level_Ended_C3_7_0,
	Level_Ended_C3_7_A,
	Level_Ended_C3_7_B,
	Level_Ended_C3_7_C,

	Level_Started_C4_1 = 325,
	Level_Started_C4_2,
	Level_Started_C4_3,
	Level_Started_C4_4,
	Level_Started_C4_5,
	Level_Started_C4_6,
	Level_Started_C4_7,

	Level_Ended_C4_1_0 = 345,
	Level_Ended_C4_1_A,
	Level_Ended_C4_1_B,
	Level_Ended_C4_1_C,
	Level_Ended_C4_2_0,
	Level_Ended_C4_2_A,
	Level_Ended_C4_2_B,
	Level_Ended_C4_2_C,
	Level_Ended_C4_3_0,
	Level_Ended_C4_3_A,
	Level_Ended_C4_3_B,
	Level_Ended_C4_3_C,
	Level_Ended_C4_4_0,
	Level_Ended_C4_4_A,
	Level_Ended_C4_4_B,
	Level_Ended_C4_4_C,
	Level_Ended_C4_5_0,
	Level_Ended_C4_5_A,
	Level_Ended_C4_5_B,
	Level_Ended_C4_5_C,
	Level_Ended_C4_6_0,
	Level_Ended_C4_6_A,
	Level_Ended_C4_6_B,
	Level_Ended_C4_6_C,
	Level_Ended_C4_7_0,
	Level_Ended_C4_7_A,
	Level_Ended_C4_7_B,
	Level_Ended_C4_7_C,
	#endregion

	Amount_RoutesCreated = 400,
	Amount_RoutesEdited,
	Amount_PackagesDelivered,

	Amount_Events_Had = 450,
	Amount_Events_Admirals,
	Amount_Events_Migration,
	Amount_Events_Mutiny,
	Amount_Events_Pirates,
	Amount_Events_PoetryContest,
	Amount_Events_RatPlague,
	Amount_Events_RottenPlantations,
	Amount_Events_Stowaway,
	Amount_Events_Windstorm,
	// When adding more event stats, add them to GameFlowManager's IncrementCorrectStats method

	Events_PirateBoatsSank = 500,
	Events_AdmiralBoatsSank,
	Events_MutiniesStopped,
	Events_PlantationsCured,
}

public static class StatKeyExtensions
{
	public static string ConvertToString(this StatKey key)
	{
		switch (key)
		{
			case StatKey.None: return SteamStatsAndAchievementsModule.INVALID_STAT_KEY_NAME;


			default:
			{
				//Debug.LogWarning($"No stat case assigned for: [{key}], please assign one");
				return key.ToString();
			}
		}
	}

	public static string ConvertToSteamString(this StatKey key)
	{
		switch (key)
		{
			case StatKey.None:
				return SteamStatsAndAchievementsModule.INVALID_STAT_KEY_NAME;

			default:
				return key.ConvertToSteamStatKey();
		}
	}
}


public static class StatKeyUtil
{
	public static List<string> GetAllNames()
	{
		var allKeys = Enum.GetNames(typeof(StatKey));
		if (allKeys == null || allKeys.Length == 0)
		{
			Debug.LogError($"[StatKey] No statkey names can be gotten from this method");
			return null;
		}

		return new List<string>(allKeys);
	}

	public static List<StatKey> GetAllKeys()
	{
		var allKeys = Enum.GetValues(typeof(StatKey)) as StatKey[];
		if (allKeys == null || allKeys.Length == 0)
		{
			Debug.LogError($"[StatKey] No statkey values can be gotten from this method");
			return null;
		}

		return new List<StatKey>(allKeys);
	}
}