using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISteamStatsAchievements
{
	void IncreaseStat(string statKey, string steamStatKey, int amount, out int newValue);
	void CompleteStat(string statKey, string steamStatKey);
}