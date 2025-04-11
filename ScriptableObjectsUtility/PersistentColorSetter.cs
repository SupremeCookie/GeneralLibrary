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

	private void Update()
	{
		if (componentWithColor == null || color == null)
			return;

		if (componentWithColor is SpriteRenderer spriteRend)
		{
			if (spriteRend.color != color.GetValue())
			{
				color.SetValue(spriteRend.color);
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
