//#define LOGGING_ENABLED

using System.Collections.Concurrent;
using UnityEngine;

public class MainThreadActionRunner : IPushAction
{
	private const int MaxPerFrame = 25;

	private ThreadChecker tc = null;
	private ConcurrentQueue<System.Action> actions;

	public MainThreadActionRunner()
	{
		tc = new ThreadChecker(); tc.SetMainThreadID();
		actions = new ConcurrentQueue<System.Action>();
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
				var success = actions.TryDequeue(out nextAction);
				Debug.Assert(success, $"Did not succeed in dequeue-ing an action, please try to fix");
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