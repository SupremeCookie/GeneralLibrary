using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalStepper
{
	private static Dictionary<int, List<Stepper>> steppers;

	static GlobalStepper()
	{
		steppers = new Dictionary<int, List<Stepper>>();
		CoroutineRunner.Instance.StartCoroutine(UpdateSteppers());
	}

	private static IEnumerator UpdateSteppers()
	{
		bool shouldBeRunning = true;
		while (shouldBeRunning)
		{
			if (steppers.IsNullOrEmpty())
				yield return null;

			foreach (var stc in steppers)
			{
				foreach (var st in stc.Value)
					st.Update(Time.unscaledDeltaTime);
			}

			yield return null;
		}
	}

	public static Stepper GetStepper(int interval, float speed = Stepper.DEFAULT_SPEED)
	{
		if (steppers.ContainsKey(interval))
			return GetStepper(steppers[interval], interval, speed);

		steppers.Add(interval, new List<Stepper>() { CreateStepper(interval, speed) });
		return GetStepper(steppers[interval], interval, speed);
	}

	private static Stepper GetStepper(List<Stepper> steppers, int interval, float speed)
	{
		if (steppers.IsNullOrEmpty())
			return null;

		foreach (var step in steppers)
		{
			if (step.speed.IsCloseTo(speed, 0.001f))
				return step;
		}

		var newStepper = CreateStepper(interval, speed);
		steppers.Add(newStepper);
		return newStepper;
	}

	private static Stepper CreateStepper(int interval, float speed)
	{
		var stepper = new Stepper(interval, speed);
		return stepper;
	}
}