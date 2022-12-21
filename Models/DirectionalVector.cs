using System;
using UnityEngine;

[Serializable]
public struct DirectionalVector
{
	public Vector2 Vector;
	public OCTA_DIRECTION Direction;
}

[Serializable]
public struct CardinalDirectionalVector
{
	public Vector2 Vector;
	public CARDINAL_DIRECTION Direction;
}
