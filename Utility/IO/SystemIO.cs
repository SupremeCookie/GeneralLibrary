using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SystemIO
{
	public static string GetPathOfSpecificDirectory(string directoryName, string parentPath, bool logChildren = false)
	{
		string destinationPath = null;
		var subDirs = GetPathsOfChildDirectories(parentPath);
		if (subDirs != null)
		{
			foreach (var item in subDirs)
			{
				if (logChildren)
				{
					Debug.Log("--Child Dirs: " + item);
				}

				if (item.IndexOf(directoryName) >= 0)
				{
					string dirOfPath = GetFinalDirectoryOfPath(item);

					if (dirOfPath == directoryName)
					{
						destinationPath = item;

						if (!logChildren)
						{
							break;
						}
					}
				}
			}
		}
		else
		{
			Debug.Log("No SubDirs in: " + parentPath);
			return string.Empty;
		}

		if (destinationPath == null)
		{
			Debug.LogWarning("No Directory named: " + directoryName + " -- found in path: " + parentPath);
			return string.Empty;
		}

		return destinationPath;
	}

	public static IEnumerable<string> FilterPaths(IEnumerable<string> paths, string[] filter)
	{
		IEnumerable<string> result = new string[] { };

		foreach (var item in paths)
		{
			var folder = GetFinalDirectoryOfPath(item);

			if (!filter.Contains(folder))
			{
				result = result.Concat(new[] { item });
			}
		}

		return result;
	}

	public static string GetFinalDirectoryOfPath(string directoryPath)
	{
		string[] splitted = directoryPath.Split(System.IO.Path.DirectorySeparatorChar);
		Debug.Assert(splitted != null, "The String Split on: " + directoryPath + " -- didn't work properly, splitted returned null");

		return splitted[splitted.Length - 1];
	}

	public static IEnumerable<string> GetPathsOfChildDirectories(string directoryPath)
	{
		return System.IO.Directory.EnumerateDirectories(directoryPath);
	}

	public static string ConvertSystemToLocalPath(string systemPath, int assetFolderToTake = 1)
	{
		const string ASSET_FOLDER = "Assets";

		Debug.Assert(!string.IsNullOrEmpty(systemPath), "Given systemPath is empty or null");
		Debug.Assert(assetFolderToTake >= 1, "assetFolderToTake is far too small");

		var splittedPath = systemPath.Split(new[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar });
		Debug.Assert(splittedPath != null && splittedPath.Length > 0, "Splitting the systemPath resulted in an empty or null splittedPath");

		List<int> assetFolderIndexes = new List<int>();
		for (int i = 0; i < splittedPath.Length; ++i)
		{
			if (splittedPath[i] == ASSET_FOLDER)
			{
				assetFolderIndexes.Add(i);
			}
		}

		Debug.Assert(assetFolderIndexes.Count > 0, "Could not find an Assets folder in the splittedPath");

		int indexToStartLooping = 0;
		if (assetFolderIndexes.Count > 1)
		{
			Debug.Assert(assetFolderToTake > assetFolderIndexes.Count, "assetFolderToTake should not be bigger than assetFolderIndexes.Count");
			Debug.Log("We detected multiple Assets folders in the path, we found: " + assetFolderIndexes.Count + " -- we will take the " + assetFolderIndexes + " -entry");
			indexToStartLooping = assetFolderIndexes[assetFolderToTake - 1];
		}
		else
		{
			indexToStartLooping = assetFolderIndexes[0];
		}

		System.Text.StringBuilder finalPath = new System.Text.StringBuilder();
		for (int i = indexToStartLooping; i < splittedPath.Length; ++i)
		{
			if (i != indexToStartLooping)
			{
				finalPath.Append(System.IO.Path.DirectorySeparatorChar);
			}

			finalPath.Append(splittedPath[i]);
		}

		Debug.Assert(!string.IsNullOrEmpty(finalPath.ToString()), "StringBuilder finalPath has no content");
		return finalPath.ToString();
	}
}
