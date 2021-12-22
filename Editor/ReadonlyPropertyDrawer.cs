using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadonlyAttribute))]
public class ReadonlyPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginDisabledGroup(true);
		EditorGUI.PropertyField(position, property, label, true);
		EditorGUI.EndDisabledGroup();
	}


	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float totalHeight = EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.standardVerticalSpacing;

		return totalHeight;
	}
}
