using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


// Acts as a dummy class to allow game code to always execute without compilation guards needed
public class SteamStatsAndAchievementsModuleDummy : ISteamStatsAchievements
{
	public void IncreaseStat(string statKey, int amount) { }
}