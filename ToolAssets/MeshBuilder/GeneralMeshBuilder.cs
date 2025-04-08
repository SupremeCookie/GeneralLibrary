using UnityEngine;
using MeshBuilder.Data;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class GeneralMeshBuilder : MonoBehaviour
{
	[Header("General")]
	[SerializeField] private Transform meshContainer;

	[Header("Shape Specific Configs")]
	[SerializeField] private CircleConfig circleConfig;

	public void CreateMesh_Circle()
	{
		Debug.Log($"Checking to see if it already exists");
		var existingMeshes = meshContainer.GetComponentsInChildren<GeneralMeshIdentifier>(includeInactive: true);
		if (!existingMeshes.IsNullOrEmpty())
		{
			for (int i = existingMeshes.Length - 1; i >= 0; --i)
			{
				var meshID = existingMeshes[i].id;
				if (meshID.Equals(circleConfig.GetID()))
				{
					Debug.Log($"-> Remove existing object");
					if (Application.isPlaying)
						Destroy(existingMeshes[i].gameObject);
					else
						DestroyImmediate(existingMeshes[i].gameObject);
				}
			}

		}

		Debug.Log($"Creating new GO");
		var newMeshHolder = new GameObject($"Mesh_Circle_{circleConfig.resolution}");
		newMeshHolder.transform.SetParent(meshContainer.transform, false);

		var meshIDComp = newMeshHolder.AddComponent<GeneralMeshIdentifier>();
		meshIDComp.id = circleConfig.GetID();

		Debug.Log($"Creating mesh data");
		var meshData = CircleMeshBuilder.Create(circleConfig);

		Debug.Log($"Add mesh to GO");
		var meshRenderer = newMeshHolder.AddComponent<MeshRenderer>();
		var meshFilterComp = newMeshHolder.AddComponent<MeshFilter>();
		meshFilterComp.mesh = meshData;

		{
			Debug.Log($"Store mesh asset");
			string objectName = newMeshHolder.name;
			meshData.name = objectName;

			string artPath = "Assets/Art/";
			string path = $"{artPath}GeneralMeshes/";
			string assetPath = System.IO.Path.Combine(path, $"{objectName}.asset");
			string prefabPath = System.IO.Path.Combine(path, $"{objectName}_prefab.prefab");

			if (!Directory.Exists(artPath))
				Directory.CreateDirectory(artPath);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			Mesh existingAsset = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
			if (existingAsset == null)
			{
				AssetDatabase.CreateAsset(meshData, assetPath);
				existingAsset = meshData;
			}
			else
			{
				existingAsset.Clear();
				EditorUtility.CopySerialized(meshData, existingAsset);
			}

			meshData = existingAsset;
			meshFilterComp.mesh = existingAsset;

			PrefabUtility.SaveAsPrefabAsset(newMeshHolder, prefabPath);

			AssetDatabase.SaveAssets();
			Debug.Log($"Saved asset at path: {assetPath}   and prefab at path: {prefabPath}");
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(GeneralMeshBuilder))]
public class GeneralMeshBuilderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(15);

		var castedTarget = target as GeneralMeshBuilder;

		if (GUILayout.Button($"Create Circle Mesh"))
			castedTarget.CreateMesh_Circle();
	}
}
#endif