using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

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
		if (GUILayout.Button("Setup Newtonsoft"))
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
		string[] manifestJsonDependencyValue = manifestJsonContent[0].Split(':');
		string manifestJsonKey = manifestJsonDependencyValue[0];

		for (int i = 0; i < manifestJsonContent.Count; ++i)
		{
			Debug.Log($"FileContent ManifestJson: {manifestJsonContent[i]}");
		}

		Debug.Log("----");

		string absoluteManifestPath = Path.GetFullPath("Packages/manifest.json");
		List<string> fileContent = new List<string>(File.ReadAllLines(absoluteManifestPath));


		// Dependency injection
		int lastCheckedIndex = -1;
		bool packageDependencyWasPresent = false;
		bool foundDependencies = true;
		for (int i = 0; i < fileContent.Count; ++i)
		{
			//Debug.Log($"FileContent Line({i}): {fileContent[i]}");
			if (fileContent[i].IndexOf("dependencies") >= 0)
			{
				foundDependencies = true;
			}

			if (foundDependencies)
			{
				if (fileContent[i].IndexOf("}") >= 0 || fileContent[i].IndexOf("},") >= 0)
				{
					break;
				}

				string[] splitLine = fileContent[i].Split(':');
				if (splitLine.Length > 1)
				{
					//Debug.Log($"Splitline 0: {splitLine[0]}   ,  key: {manifestJsonKey}");
					if (splitLine[0].IndexOf(manifestJsonKey) >= 0) // Note: Equals don't work here, because there's leading spaces
					{
						Debug.Log($"We found the dependency in the list, gotta set the value then to what we've got stored in the txt");
						splitLine[1] = manifestJsonDependencyValue[1];
						packageDependencyWasPresent = true;
					}

					fileContent[i] = string.Join(':', splitLine);

					lastCheckedIndex = i;
				}
			}
		}

		if (foundDependencies && !packageDependencyWasPresent)
		{
			Debug.Log($"The newtonsoft dependency will be added to the list");
			if (!fileContent[lastCheckedIndex].EndsWith(","))
				fileContent[lastCheckedIndex] += ",";

			fileContent.Insert(lastCheckedIndex + 1, manifestJsonContent[0]);
		}

		// scopedRegistries dependency
		for (int i = 0; i < fileContent.Count; ++i)
		{

		}


		File.WriteAllLines(absoluteManifestPath, fileContent);
	}
}
