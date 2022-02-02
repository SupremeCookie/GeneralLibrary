using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Coordinates))]
public class CoordinatesPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		float width = 100;
		var xRect = new Rect(position.x, position.y, width, position.height);
		var yRect = new Rect(position.x + width, position.y, width, position.height);

		EditorGUI.PropertyField(xRect, property.FindPropertyRelative("x"), GUIContent.none);
		EditorGUI.PropertyField(yRect, property.FindPropertyRelative("y"), GUIContent.none);

		EditorGUI.EndProperty();
	}
}
