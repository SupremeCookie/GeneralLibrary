using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrapper : MonoBehaviour
{
	// TODO DK: Make editor script that takes SceneAsset type, then a button to turn that list into a list of readonly strings that then get actually loaded.
	// That way I avoid typos.

	public string[] ScenesToLoad;

	public void Awake()
	{
		for (int i = 0; i < ScenesToLoad.Length; ++i)
		{
			SceneManager.LoadScene(ScenesToLoad[i], LoadSceneMode.Additive);
		}
	}
}