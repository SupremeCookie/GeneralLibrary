using System.Collections;
using UnityEngine;

public class CoroutineRunner : SingletonMonoBehaviour<CoroutineRunner>
{
	public IEnumerator CallCallbackAfterCheck(System.Func<bool> check, System.Action callback, int extraFrameWaits = -1)
	{
		while (!check())
		{
			yield return null;
		}

		if (extraFrameWaits > 0)
		{
			for (int i = 0; i < extraFrameWaits; ++i)
			{
				yield return null;
			}
		}

		callback();
	}

	public static IEnumerator DelayedCallback(float delay, System.Action callback)
	{
		while (delay > 0)
		{
			delay -= Time.unscaledDeltaTime;
			yield return null;
		}

		callback();
	}
}
