
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorScreenShot
{
	public bool CaptureEditorScreenshot(string _filename, bool _transparent)
	{
		SceneView sw = SceneView.lastActiveSceneView;

		if (sw == null)
		{
			Debug.LogError("Unable to capture editor screenshot, no scene view found");
			return false;
		}

		Camera cam = sw.camera;

		if (cam == null)
		{
			Debug.LogError("Unable to capture editor screenshot, no camera attached to current scene view");
			return false;
		}

		RenderTexture renderTexture = cam.targetTexture;

		if (renderTexture == null)
		{
			Debug.LogError("Unable to capture editor screenshot, camera has no render texture attached");
			return false;
		}

		int width = renderTexture.width;
		int height = renderTexture.height;

		var outputTexture = new Texture2D(width, height, _transparent ? TextureFormat.RGBA32 : TextureFormat.RGB24, false);

		RenderTexture.active = renderTexture;

		outputTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);

		byte[] pngData = outputTexture.EncodeToPNG();

		FileStream file = File.Create(_filename);

		if (!file.CanWrite)
		{
			Debug.LogError("Unable to capture editor screenshot, Failed to open file for writing");
			return false;
		}

		file.Write(pngData, 0, pngData.Length);

		file.Close();

		Object.DestroyImmediate(outputTexture);

		return true;
	}
}
#endif