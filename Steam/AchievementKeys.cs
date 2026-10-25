using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public enum AchievementKeys
{
	BoatsPlaced_1,
	BoatsPlaced_2,
	BoatsPlaced_3,

	BoatsPlaced_Sloop_1,
	BoatsPlaced_Sloop_2,

	BoatsPlaced_Frigate_1,
	BoatsPlaced_Frigate_2,

	BoatsPlaced_Caravel_1,
	BoatsPlaced_Caravel_2,

	BoatsPlaced_Lichter_1,
	BoatsPlaced_Lichter_2,

	BoatsPlaced_Post_1,
	BoatsPlaced_Post_2,

	RoutesPlaced_1,
	RoutesPlaced_2,

	CargoDelivered_1,
	CargoDelivered_2,
	CargoDelivered_3,
	CargoDelivered_4,

	PirateBoatsSank_1,
	PirateBoatsSank_2,

	AdmiralBoatsSank_1,
	AdmiralBoatsSank_2,

	RottenPlantationsHealed_1,
	RottenPlantationsHealed_2,

	MutiniesStopped_1,
	MutiniesStopped_2,

	CampaignsPlayed_1,
	CampaignsPlayed_2,

	CampaignsGold_1,
	CampaignsGold_2,
}


public static class AchievementKeysConfig
{
	public static int GetAchievementThreshold(AchievementKeys key)
	{
		switch (key)
		{
			case AchievementKeys.BoatsPlaced_1: { return 10; }
			case AchievementKeys.BoatsPlaced_2: { return 100; }
			case AchievementKeys.BoatsPlaced_3: { return 2000; }
			case AchievementKeys.BoatsPlaced_Sloop_1: { return 5; }
			case AchievementKeys.BoatsPlaced_Sloop_2: { return 50; }
			case AchievementKeys.BoatsPlaced_Frigate_1: { return 5; }
			case AchievementKeys.BoatsPlaced_Frigate_2: { return 50; }
			case AchievementKeys.BoatsPlaced_Caravel_1: { return 5; }
			case AchievementKeys.BoatsPlaced_Caravel_2: { return 50; }
			case AchievementKeys.BoatsPlaced_Lichter_1: { return 5; }
			case AchievementKeys.BoatsPlaced_Lichter_2: { return 50; }
			case AchievementKeys.BoatsPlaced_Post_1: { return 5; }
			case AchievementKeys.BoatsPlaced_Post_2: { return 50; }
			case AchievementKeys.RoutesPlaced_1: { return 5; }
			case AchievementKeys.RoutesPlaced_2: { return 50; }
			case AchievementKeys.CargoDelivered_1: { return 50; }
			case AchievementKeys.CargoDelivered_2: { return 500; }
			case AchievementKeys.CargoDelivered_3: { return 5000; }
			case AchievementKeys.CargoDelivered_4: { return 50000; }
			case AchievementKeys.PirateBoatsSank_1: { return 3; }
			case AchievementKeys.PirateBoatsSank_2: { return 30; }
			case AchievementKeys.AdmiralBoatsSank_1: { return 3; }
			case AchievementKeys.AdmiralBoatsSank_2: { return 30; }
			case AchievementKeys.RottenPlantationsHealed_1: { return 3; }
			case AchievementKeys.RottenPlantationsHealed_2: { return 30; }
			case AchievementKeys.MutiniesStopped_1: { return 3; }
			case AchievementKeys.MutiniesStopped_2: { return 30; }
			case AchievementKeys.CampaignsPlayed_1: { return 1; }
			case AchievementKeys.CampaignsPlayed_2: { return 1; }
			case AchievementKeys.CampaignsGold_1: { return 1; }
			case AchievementKeys.CampaignsGold_2: { return 1; }
		}

		return 100;
	}

	public static string GetAchievementSteamKey(AchievementKeys key)
	{
		switch (key)
		{
			case AchievementKeys.BoatsPlaced_1: { return "ACH_001"; }
			case AchievementKeys.BoatsPlaced_2: { return "ACH_002"; }
			case AchievementKeys.BoatsPlaced_3: { return "ACH_003"; }
			case AchievementKeys.BoatsPlaced_Sloop_1: { return "ACH_004"; }
			case AchievementKeys.BoatsPlaced_Sloop_2: { return "ACH_005"; }
			case AchievementKeys.BoatsPlaced_Frigate_1: { return "ACH_006"; }
			case AchievementKeys.BoatsPlaced_Frigate_2: { return "ACH_007"; }
			case AchievementKeys.BoatsPlaced_Caravel_1: { return "ACH_008"; }
			case AchievementKeys.BoatsPlaced_Caravel_2: { return "ACH_009"; }
			case AchievementKeys.BoatsPlaced_Post_1: { return "ACH_010"; }
			case AchievementKeys.BoatsPlaced_Post_2: { return "ACH_011"; }
			case AchievementKeys.BoatsPlaced_Lichter_1: { return "ACH_012"; }
			case AchievementKeys.BoatsPlaced_Lichter_2: { return "ACH_013"; }
			case AchievementKeys.RoutesPlaced_1: { return "ACH_014"; }
			case AchievementKeys.RoutesPlaced_2: { return "ACH_015"; }
			case AchievementKeys.CargoDelivered_1: { return "ACH_016"; }
			case AchievementKeys.CargoDelivered_2: { return "ACH_017"; }
			case AchievementKeys.CargoDelivered_3: { return "ACH_018"; }
			case AchievementKeys.CargoDelivered_4: { return "ACH_019"; }
			case AchievementKeys.PirateBoatsSank_1: { return "ACH_020"; }
			case AchievementKeys.PirateBoatsSank_2: { return "ACH_021"; }
			case AchievementKeys.AdmiralBoatsSank_1: { return "ACH_022"; }
			case AchievementKeys.AdmiralBoatsSank_2: { return "ACH_023"; }
			case AchievementKeys.MutiniesStopped_1: { return "ACH_024"; }
			case AchievementKeys.MutiniesStopped_2: { return "ACH_025"; }
			case AchievementKeys.RottenPlantationsHealed_1: { return "ACH_026"; }
			case AchievementKeys.RottenPlantationsHealed_2: { return "ACH_027"; }
			case AchievementKeys.CampaignsPlayed_1: { return "ACH_028"; }
			case AchievementKeys.CampaignsPlayed_2: { return "ACH_030"; }
			case AchievementKeys.CampaignsGold_1: { return "ACH_029"; }
			case AchievementKeys.CampaignsGold_2: { return "ACH_031"; }
		}

		return "ACH_001";
	}
}