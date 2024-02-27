using System;
using UnityEngine;

[Serializable]
public struct DirectionalVector
{
	public Vector2 Vector;
	public OctaDirection Direction;
}

[Serializable]
public struct CardinalDirectionalVector
{
	public Vector2 Vector;
	public CardinalDirection Direction;
}
