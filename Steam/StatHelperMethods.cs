using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class StatHelperMethods
{
	public static StatKey ConvertBoatType_Placement(int boatType)
	{
		switch (boatType)
		{
			case 0: { return StatKey.BoatsPlaced_Sloop; }
			case 1: { return StatKey.BoatsPlaced_Frigate; }
			case 2: { return StatKey.BoatsPlaced_Caravel; }

			case 11: { return StatKey.BoatsPlaced_Post; }
			case 12: { return StatKey.BoatsPlaced_Lichter; }

			default: { Debug.LogWarning($"No case made for boat: {boatType}"); return StatKey.None; }
		}
	}
}