using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// More Info:
// https://partner.steamgames.com/doc/features/leaderboards
public class SteamLeaderboardsModule : ISteamLeaderboards
{
	// What do we need to do:
	// - I want to display friends, and global leaderboards in 2 separate lists
	// Always start on global leaderboads
	// - First we should test with getting leaderboards asynchronously when we demand them of a level in the leaderboards menu
	// - After a level ends we need to store the score (if it is a new highscore) in the leaderboards
	// - When the game starts we need a delayed call to store the current highscores to the leaderboards
}
