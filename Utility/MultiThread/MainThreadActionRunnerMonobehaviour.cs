using System;
using UnityEngine;

public class MainThreadActionRunnerMonobehaviour : MonoBehaviour, IPushAction
{
	private MainThreadActionRunner actionRunner;

	private void Awake()
	{
		actionRunner = new MainThreadActionRunner();
	}

	private void Update()
	{
		Debug.Assert(actionRunner != null, $"No actionRunner to Update, this means it was destroyed or this method is called before Awake");
		actionRunner.Update();
	}

	public void PushAction(Action action)
	{
		Debug.Assert(actionRunner != null, $"No actionRunner to push the action to, this means it was destroyed or this method is called before Awake");
		actionRunner.PushAction(action);
	}

	public void DestroySelf()
	{
		PushAction(() =>
		{
			actionRunner = null;
			Destroy(this.gameObject);
		});
	}
}