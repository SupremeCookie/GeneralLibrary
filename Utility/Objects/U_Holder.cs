using UnityEngine;

public enum U_Type
{
	None,
	Int,
	Octa_Direction,
};

public class U_Holder : MonoBehaviour
{
	public string Note;
	[Space(15)]
	public U_Type Type;

	[HideInInspector]
	public int DefaultIntValue;

	private IU_Object _updatable;
	public IU_Object Updatable
	{
		get
		{
			if (_updatable == null)
			{
				ConstructObject();
			}

			return _updatable;
		}
	}

	private void ConstructObject()
	{
		switch (Type)
		{
			case U_Type.Int:
				_updatable = new U_Int(DefaultIntValue);
				break;

			case U_Type.Octa_Direction:
				_updatable = new U_OctaDirection();
				break;

			case U_Type.None:
				Debug.LogWarning("This U_Holder can't make UpdatableObjects of type <b>None</b>", gameObject);
				break;
			default:
				Debug.LogError($"This U_Type: <b>{Type}</b> doesn't have a case defined on <b>ConstructObject; U_Holder</b>", gameObject);
				break;
		}
	}
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(U_Holder))]
public class U_HolderEditor : UnityEditor.Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		serializedObject.Update();

		var casted = (U_Holder)target;

		switch (casted.Type)
		{
			case U_Type.Int:
				GUILayout.Space(15);

				UnityEditor.EditorGUILayout.BeginHorizontal();
				UnityEditor.EditorGUILayout.LabelField("Default Integer Value:  ");
				casted.DefaultIntValue = UnityEditor.EditorGUILayout.IntField(casted.DefaultIntValue);
				UnityEditor.EditorGUILayout.EndHorizontal();

				break;
		}

		serializedObject.ApplyModifiedProperties();
	}
}
#endif
