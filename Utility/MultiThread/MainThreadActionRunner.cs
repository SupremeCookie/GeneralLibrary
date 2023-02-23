//#define LOGGING_ENABLED

using System.Collections.Generic;
using UnityEngine;

public class MainThreadActionRunner
{
	private const int MaxPerFrame = 25;

	private ThreadChecker tc = null;
	private Queue<System.Action> actions;

	public MainThreadActionRunner()
	{
		tc = new ThreadChecker(); tc.SetMainThreadID();
		actions = new Queue<System.Action>();
	}

	public void Update()
	{
		Debug.Assert(tc.IsCurrentThreadMainThread(), $"We are not on the main thread, please fix");

#if LOGGING_ENABLED
        if ((actions?.Count ?? -1) != 0)
		{
			Debug.Log($"Update ThreadActionRunner, actionsCount: {actions?.Count ?? -1}");
		}
#endif

		for (int i = 0; i < MaxPerFrame; ++i)
		{
			System.Action nextAction = null;
			if (actions.Count > 0)
			{
				nextAction = actions.Dequeue();
			}

			if (nextAction == null)
			{
				break;
			}

			nextAction.Invoke();
		}
	}

	public void PushAction(System.Action action)
	{
		actions.Enqueue(action);
	}
}