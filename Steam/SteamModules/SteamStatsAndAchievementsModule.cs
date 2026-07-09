using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// More Info:
// https://partner.steamgames.com/doc/features/achievements
// https://partner.steamgames.com/doc/features/achievements/stats_guide
// https://partner.steamgames.com/doc/features/achievements/ach_guide
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
}

// - Development options include:
// - Resetting al Stats and Achievements using a button
