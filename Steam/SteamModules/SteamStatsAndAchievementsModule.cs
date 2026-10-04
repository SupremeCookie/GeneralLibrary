using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// More Info:
// https://partner.steamgames.com/doc/features/achievements
// https://partner.steamgames.com/doc/features/achievements/stats_guide
// https://partner.steamgames.com/doc/features/achievements/ach_guide
// Take a look at Global Stats, may be interesting for tracking
// Gonna have to localize the achievements
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


	// Some stats:
	//  (global-can repurpose as 'tracking')
	//		For each type of boat, how many we've placed, then a running total called total boats placed based on those values
	//  (global-can repurpose as 'tracking')
	//		For each level, how often it's been started, and for each rank including none how often it's been finished
	//	Amount of routes created
	//  Amount of routes edited
	//  Amount of packages delivered
	//  Amount of events had, including every specific event type, so the total is an aggregate of each specific type
	//	Boats picked up after having been placed down
	//	Pirate boats sank, Admirals sank, Mutinies stopped

	//  Some achievements:
	//  Reaching the tutorial school halfway point + finishing it fully
	//  Unlocking each campaign
	//  Finishing a campaign fully on bronze, fully on silver, fully on gold
	//	Having started each of a campaign's levels
	//  Entering a campaign level for the first time
	//	boats placed, routes placed, boats picked up, routes edited
	//	Packages delivered
	//	Having accomplished events (pirates/admirals/mutinies)


	// Some thoughts:
	//Definitely need to add stats to the game (even in demo)
	//	more than I may need
	//	Let the game's stats be leading, we've got cloud saves anyway
	//	Then upload those stats to the steam(not in demo)
	//And have that contribute to unlocking achievements
	//That said, ALWAYS trigger stat based achievements after accomplishing the thing atleast 1 more time(so you don't get random achievements)
	//Do unlock "story" based progression when starting up again, if the achievement is to accomplish full gold stars in campaign 1, then don't make them play another level

	//> At the start of a game session, call ISteamUserStats::RequestCurrentStats to fetch the user's stats and achievement data from the Steam back end. You will receive a ISteamUserStats::UserStatsReceived_t callback when the data is ready.
	//This is important, but always keep highest stat
}

// - Development options include:
// - Resetting al Stats and Achievements using a button
