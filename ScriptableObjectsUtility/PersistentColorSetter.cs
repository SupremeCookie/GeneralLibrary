using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PersistentColorSetter : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private PersistentColor color;
	[SerializeField] private Renderer componentWithColor;
	[Space(15)]
	[SerializeField, Readonly] private Color resultingColor;

	// Had to update to support the new shader setup
	// Maybe update this with 2 editor buttons, 1 load from sprite rend, and 1 apply to sprite rend?
	private void Update()
	{
		if (componentWithColor == null || color == null)
			return;

		if (componentWithColor is SpriteRenderer spriteRend)
		{
			bool hasSetColor = false;

			var material = spriteRend.sharedMaterial;
			bool weHaveACustomShader = material.HasProperty("_Tint");
			if (weHaveACustomShader)
			{
				var chosenColor = material.GetColor("_Tint");
				color.SetValue(chosenColor);
				hasSetColor = true;
			}
			else
			{
				if (spriteRend.color != color.GetValue())
				{
					color.SetValue(spriteRend.color);
					hasSetColor = true;
				}
			}

			if (hasSetColor)
			{
				resultingColor = color.GetValue();

#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(color);
#endif
			}
		}
		else
		{
			Debug.LogWarning($"We don't have a case for this specific Renderer, please add");
		}
	}
}
