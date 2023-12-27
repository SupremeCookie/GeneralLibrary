using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueLike;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ResortSpriteRenderersUtil
{
	[MenuItem("Tools/Resort Sprite Renderers", priority = 70)]
	public static void OpenResortSpriteRenderers()
	{

		var window = EditorWindow.FindObjectOfType<ResortSpriteRenderers>();
		if (window == null)
			window = EditorWindow.CreateWindow<ResortSpriteRenderers>("Resort Sprite Renderers", typeof(ResortSpriteRenderers));

		window.Show();
		window.Focus();
	}
}

public class ResortSpriteRenderers : EditorWindow
{
	private SortingLayerType layer;
	private int order;

	public List<GameObject> gameObjects = new List<GameObject>();

	private void OnGUI()
	{
		if (gameObjects == null)
		{
			gameObjects = new List<GameObject>();
		}

		layer = (SortingLayerType)EditorGUILayout.EnumPopup("Sorting Layer", layer);
		order = EditorGUILayout.IntField("Sorting Order", order);

		GUILayout.Space(10);

		ScriptableObject target = this;
		SerializedObject so = new SerializedObject(target);
		SerializedProperty list = so.FindProperty($"gameObjects");

		EditorGUILayout.PropertyField(list, true);
		so.ApplyModifiedProperties();

		GUILayout.Space(10);

		if (GUILayout.Button("Set Layer and Order"))
		{
			if (!gameObjects.IsNullOrEmpty())
			{
				for (int i = 0; i < gameObjects.Count; ++i)
				{
					var go = gameObjects[i];
					var spriteRenderers = go.GetComponentsInChildren<SpriteRenderer>();

					for (int k = 0; k < spriteRenderers.Length; ++k)
					{
						spriteRenderers[k].sortingOrder = order;
						spriteRenderers[k].sortingLayerName = layer.ToSemanticString();
						EditorUtility.SetDirty(spriteRenderers[k]);
					}

					EditorUtility.SetDirty(go);
				}

			}
		}
	}

	private void OnInspectorUpdate()
	{
		Repaint();
	}
}
#endif