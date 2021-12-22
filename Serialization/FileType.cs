using UnityEngine;

public enum FileType
{
	Json,
	Txt,
}

public static class FileTypeExtensions
{
	public static string ToContextualString(this FileType fileType)
	{
		switch (fileType)
		{
			case FileType.Json:
			{
				return "json";
			}

			case FileType.Txt:
			{
				return "txt";
			}
		}

		Debug.Assert(false, "Returning an empty string, as we haven't made a case for fileType: " + fileType.ToString());
		return "";
	}
}
