using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Latch
{
	public float latchValue;
	public bool hasJustLatched = false;
	public float currentLatchValue = 0f;

	public bool hasFinished { get; private set; } = false;

	public Latch() { }
	public Latch(float latchValue)
	{
		this.latchValue = latchValue;
	}

	public Latch Copy()
	{
		return new Latch()
		{
			latchValue = this.latchValue,
			hasJustLatched = this.hasJustLatched,
			currentLatchValue = this.currentLatchValue,
			hasFinished = this.hasFinished,
		};
	}

	public void Update(float dt)
	{
		currentLatchValue += dt;

		if (hasJustLatched)
			hasJustLatched = false;

		if (hasFinished)
			return;

		if (currentLatchValue > latchValue)
		{
			hasJustLatched = true;
			hasFinished = true;
		}
	}

	public void Restart()
	{
		hasJustLatched = false;
		hasFinished = false;
		currentLatchValue = 0f;
	}
}