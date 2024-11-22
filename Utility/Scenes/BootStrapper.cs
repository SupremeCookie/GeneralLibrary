using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ConditionalLoad
{
	public enum Condition
	{
		None,
		Imgui,
	}

	public Condition condition;
	public string[] scenes;
}

public class BootStrapper : MonoBehaviour
{
	// TODO DK: Make editor script that takes SceneAsset type, then a button to turn that list into a list of readonly strings that then get actually loaded.
	// That way I avoid typos.

	public string[] ScenesToLoad;
	public ConditionalLoad[] conditionalLoads;

	public void Awake()
	{
		List<string> openScenes = new List<string>(SceneManager.sceneCount);
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			openScenes.Add(SceneManager.GetSceneAt(i).name);
		}


		if (!ScenesToLoad.IsNullOrEmpty())
		{
			for (int i = 0; i < ScenesToLoad.Length; ++i)
			{
				if (!openScenes.Contains(ScenesToLoad[i]))
					SceneManager.LoadScene(ScenesToLoad[i], LoadSceneMode.Additive);
			}
		}

		if (!conditionalLoads.IsNullOrEmpty())
		{
			for (int i = 0; i < conditionalLoads.Length; ++i)
			{
				for (int k = 0; k < conditionalLoads[i].scenes.Length; ++k)
				{
					if (!HasConditionBeenMet(conditionalLoads[i].condition))
						continue;

					if (!openScenes.Contains(conditionalLoads[i].scenes[k]))
						SceneManager.LoadScene(conditionalLoads[i].scenes[k], LoadSceneMode.Additive);
				}
			}
		}
	}

	private bool HasConditionBeenMet(ConditionalLoad.Condition condition)
	{
		switch (condition)
		{
			case ConditionalLoad.Condition.None:
				return true;

			case ConditionalLoad.Condition.Imgui:
			{
#if DEBUG_MENU
				return true;
#else
				return false;
#endif
			}

			default:
			{
				Debug.Log($"No condition logic made for: {condition}, will be returning true");
				return true;
			}
		}
	}
}