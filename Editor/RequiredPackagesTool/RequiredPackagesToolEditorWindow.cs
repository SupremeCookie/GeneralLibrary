using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class RequiredPackagesToolEditorWindow : EditorWindow
{
	[MenuItem("DKat/Required Packages Window", priority = -10)]
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

		//for (int i = 0; i < manifestJsonContent.Count; ++i)
		//{
		//	Debug.Log($"FileContent ManifestJson: {manifestJsonContent[i]}");
		//}

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

						// If the entry is the last one in the dependencies list, we gotta remove the trailing ,
						if (fileContent[i + 1].IndexOf("}") >= 0 && splitLine[1].EndsWith(","))
							splitLine[1] = splitLine[1].Remove(splitLine[1].Length - 1);


						packageDependencyWasPresent = true;
					}

					fileContent[i] = string.Join(":", splitLine);

					lastCheckedIndex = i;
				}
			}
		}

		if (foundDependencies && !packageDependencyWasPresent)
		{
			Debug.Log($"The newtonsoft dependency will be added to the list");
			if (!fileContent[lastCheckedIndex].EndsWith(","))
				fileContent[lastCheckedIndex] += ",";

			string contentToAdd = manifestJsonContent[0];
			if (contentToAdd.EndsWith(","))
				contentToAdd = contentToAdd.Remove(contentToAdd.Length - 1);

			fileContent.Insert(lastCheckedIndex + 1, contentToAdd);
		}

		// scopedRegistries dependency
		string scopedRegistriesName = manifestJsonContent[1].Split(':')[0];
		string scopedRegistriesValueName = manifestJsonContent[3];
		bool foundScopedRegistries = false;
		bool foundOurEntry = false;

		int lastCheckedIndexSR = 0;
		int squareBracketStack = 0;

		for (int i = 0; i < fileContent.Count; ++i)
		{
			if (fileContent[i].IndexOf(scopedRegistriesName) >= 0)
			{
				foundScopedRegistries = true;
			}

			if (foundScopedRegistries)
			{
				if (fileContent[i].IndexOf("[") >= 0)
					squareBracketStack++;

				if (fileContent[i].IndexOf("]") >= 0)
					squareBracketStack--;

				if (squareBracketStack > 0)
					lastCheckedIndexSR = i;

				if (fileContent[i].IndexOf(scopedRegistriesValueName) >= 0)
				{
					foundOurEntry = true;
					for (int k = 3; k < (9 - 2); ++k)   // We start at the name part, and stop before the last squiggly bracket
					{
						fileContent[i + (k - 3)] = manifestJsonContent[k];
					}
				}
			}
		}

		// Add the entire scopedRegistries part
		if (!foundScopedRegistries)
		{
			int lastLineIndex = fileContent.Count - 1;
			if (fileContent[lastLineIndex - 1].EndsWith("}"))
				fileContent[lastLineIndex - 1] += ",";

			fileContent.RemoveAt(lastLineIndex);
			for (int i = 1; i < 10; ++i)
			{
				fileContent.Add(manifestJsonContent[i]);
			}

			fileContent.Add("}");
		}
		else if (!foundOurEntry)
		{
			// We add our registry stuff starting at the last checked indexSR
			if (!fileContent[lastCheckedIndexSR].EndsWith(","))
				fileContent[lastCheckedIndexSR] += ",";

			for (int i = 2; i < 9; ++i)
				fileContent.Insert(lastCheckedIndexSR + 1 + i - 2, manifestJsonContent[i]);
		}

		File.WriteAllLines(absoluteManifestPath, fileContent);
	}
}
