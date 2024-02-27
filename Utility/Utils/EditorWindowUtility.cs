using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class EditorWindowUtility
{
	public static GUIStyle CreateHeaderStyle()
	{
		var result = new GUIStyle(GUI.skin.label);

		result.normal.background = CreateHeaderBackground(new Color(0.2f, 0.6f, 1f), 300, 20);
		result.alignment = TextAnchor.MiddleCenter;

		result.fontSize += 5;

		return result;
	}

	public static Texture2D CreateHeaderBackground(Color color, in int width, in int height)
	{
		Texture2D newBackground = new Texture2D(width, height);

		Color[] pixels = CreateColoredPixels(color, width, height);

		newBackground.SetPixels(pixels);
		newBackground.Apply();

		return newBackground;
	}

	public static Color[] CreateColoredPixels(Color pixelColor, int width, int height)
	{
		Color[] pixels = new Color[width * height];
		for (int i = 0; i < pixels.Length; ++i)
		{
			if ((i % width) == 0 || (i % width) == (width - 1)
				|| i < width || i >= (width * (height - 1)))
			{
				pixels[i] = Color.black;
				continue;
			}

			pixels[i] = pixelColor;
		}

		return pixels;
	}

	public static void DrawButton(string label, float availableWidth, System.Action onButtonCallback)
	{
		GUILayout.BeginHorizontal();
		float buttonWidth = availableWidth * 0.65f;
		GUILayout.Space(buttonWidth * 0.25f);

		if (GUILayout.Button(label, GUILayout.MaxWidth(buttonWidth)))
		{
			onButtonCallback.Invoke();
		}

		GUILayout.EndHorizontal();
	}
}