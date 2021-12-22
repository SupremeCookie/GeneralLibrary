using System.IO;
using UnityEngine;

public static class SerializationStorage
{
	public static string ConstructPath(string fileName, FileType fileType)
	{
		Debug.Assert(!string.IsNullOrEmpty(fileName), "fileName is null or empty, We won't be able to save to this.");

		var unityPath = Application.persistentDataPath;

		var filePath = Path.Combine(unityPath, fileName);
		var fileTypeString = fileType.ToContextualString();

		var result = string.Format("{0}.{1}", filePath, fileTypeString);
		Debug.Log("Constructing Path, Result: " + result);

		return result;
	}

	public static string ConstructPath(string folderName, string fileName, FileType fileType)
	{
		Debug.Assert(!string.IsNullOrEmpty(folderName), "folderName is null or empty, consider using ConstructPath with only the fileName overload");
		Debug.Assert(!string.IsNullOrEmpty(fileName), "fileName is null or empty, We won't be able to save to this.");

		var unityPath = Application.persistentDataPath;
		var folderPath = Path.Combine(unityPath, folderName);

		bool folderExists = Directory.Exists(folderPath);
		if (!folderExists)
		{
			Directory.CreateDirectory(folderPath);
		}

		var filePath = Path.Combine(folderPath, fileName);
		var fileTypeString = fileType.ToContextualString();

		var result = string.Format("{0}.{1}", filePath, fileTypeString);
		Debug.Log("Constructing Path, Result: " + result);

		return result;
	}



	// DK: Add Async variants
	public static void Write(string content, string path)
	{
		Debug.Assert(!string.IsNullOrEmpty(content), "No content to write");
		Debug.Assert(!string.IsNullOrEmpty(path), "No path to write to");

		File.WriteAllText(path, content);

		Debug.Log("Finished Writing To: " + path);
	}

	public static string Read(string path)
	{
		Debug.Assert(!string.IsNullOrEmpty(path), "No path to read from");

		bool fileExists = File.Exists(path);
		if (!fileExists)
		{
			return "";
		}

		var result = File.ReadAllText(path);
		Debug.Log("Finished Reading From: " + path);

		return result;
	}
}