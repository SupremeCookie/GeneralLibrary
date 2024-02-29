using System;
using UnityEngine;

[Serializable]
public struct DirectionVector
{
	public Vector2 vector;
	public OctaDirection direction;
}

[Serializable]
public struct CardinalDirectionalVector
{
	public Vector2 vector;
	public CardinalDirection direction;
}
