using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


// Higher positioned (so lower code line number) ignores timescales of those positioned below it.
public enum TimescaleLevel
{
	Global,
	Gameplay,
	Tutorial,
}
