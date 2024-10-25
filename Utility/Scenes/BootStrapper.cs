using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrapper : MonoBehaviour
{
	// TODO DK: Make editor script that takes SceneAsset type, then a button to turn that list into a list of readonly strings that then get actually loaded.
	// That way I avoid typos.

	public string[] ScenesToLoad;

	public void Awake()
	{
		List<string> openScenes = new List<string>(SceneManager.sceneCount);
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			openScenes.Add(SceneManager.GetSceneAt(i).name);
		}

		for (int i = 0; i < ScenesToLoad.Length; ++i)
		{
			if (!openScenes.Contains(ScenesToLoad[i]))
				SceneManager.LoadScene(ScenesToLoad[i], LoadSceneMode.Additive);
		}
	}
}