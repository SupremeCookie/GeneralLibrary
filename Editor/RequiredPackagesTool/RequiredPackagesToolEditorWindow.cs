using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class RequiredPackagesToolEditorWindow : EditorWindow
{
	[MenuItem("DKatGames/Required Packages Window")]
	public static void ShowWindow()
	{
		RequiredPackagesToolEditorWindow window = GetWindow<RequiredPackagesToolEditorWindow>();
		window.titleContent = new GUIContent($"{typeof(RequiredPackagesToolEditorWindow)}");
	}
	
	public void OnGUI()
	{
		if(GUILayout.Button("Setup Newtonsoft"))
		{
			TrySetupNewtonsoft();
		}
	}

	private void TrySetupNewtonsoft()
	{
		MonoScript ms = MonoScript.FromScriptableObject(this);
		string scriptFile = AssetDatabase.GetAssetPath(ms);
		FileInfo fi = new FileInfo(scriptFile);
		string scriptFolder = fi.Directory.ToString();
		Debug.Log($"Script Folder Path: {scriptFolder}");

		string fullScriptFolderPath = Path.GetFullPath(scriptFolder);
		string manifestContentPath = $"{fullScriptFolderPath}/manifestJson_newtonsoftPackageData.txt";
		List<string> manifestJsonContent = new List<string>(File.ReadAllLines(manifestContentPath));
		for(int i = 0; i < manifestJsonContent.Count; ++i)
		{
			Debug.Log($"FileContent ManifestJson: {manifestJsonContent[i]}");
		}

		Debug.Log("----");

		string absoluteManifestPath = Path.GetFullPath("Packages/manifest.json");
		List<string> fileContent = new List<string>(File.ReadAllLines(absoluteManifestPath));

		int lastCheckedIndex = -1;
		bool dependencyWasPresent = false;
		bool foundDependencies = true;

		for(int i = 0; i < fileContent.Count; ++i)
		{
			Debug.Log($"FileContent Line({i}): {fileContent[i]}");
			if(fileContent[i].IndexOf("dependencies") >= 0)
			{
				foundDependencies = true;
				string[] splitLine = fileContent[i].Split(':');
				if(splitLine.Length > 1)
				{
					//if(splitLine[0].IndexOf(""))
				}
			}

			if(foundDependencies)
				lastCheckedIndex = i;
		}
	}
}
