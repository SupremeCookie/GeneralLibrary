using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeScale
{
	[field: SerializeField, Readonly] public string id { get; private set; } = "None";
	[field: SerializeField, Readonly] public float timescale { get; private set; } = 1.0f;

	public TimeScale(float scale, string id)
	{
		this.id = id;
		this.timescale = scale;
	}
}
