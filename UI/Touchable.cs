using UnityEngine.UI;
using UnityEngine;

// https://stackoverflow.com/questions/36888780/how-to-make-an-invisible-transparent-button-work/36892803#36892803

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Touchable))]
public class Touchable_Editor : Editor
{ public override void OnInspectorGUI() { } }
#endif

[RequireComponent(typeof(CanvasRenderer))]
public class Touchable : Graphic
{
	protected override void UpdateGeometry() { }
}