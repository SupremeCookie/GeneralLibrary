using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpriteAnimation_UI : SpriteAnimation
{
	[Header("UI Variant Settings")]
	[SerializeField] private Image animationTarget;

	protected override void Reset()
	{
		animationTarget = GetComponent<Image>();

		base.Reset();
	}


	protected override void SetSprite(Sprite sprite)
	{
		animationTarget.sprite = sprite;
	}

	protected override void SetScale(Vector3 scale)
	{
		animationTarget.transform.localScale = scale;
	}

	protected override void SetColor(Color color)
	{
		animationTarget.color = color;
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(SpriteAnimation_UI))]
public class SpriteAnimation_UIEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(15);

		if (GUILayout.Button($"Reset Animated Sprites To Default"))
		{
			(target as SpriteAnimation_UI).SetAnimatedSpritesToDefault();
		}

		if (GUILayout.Button($"Set dirty"))
		{
			UnityEditor.EditorUtility.SetDirty(target);
		}
	}
}
#endif
