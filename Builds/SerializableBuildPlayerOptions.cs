#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[UnityEngine.Scripting.Preserve]
[System.Serializable]
[CreateAssetMenu(fileName = SCRIPTABLE_NAME, menuName = "_scriptables/" + SCRIPTABLE_NAME, order = 0)]
public class SerializableBuildPlayerOptions : CustomSO
{
	protected static SerializableBuildPlayerOptions pInstance;

	public const string SCRIPTABLE_NAME = "BuildOptions";

	public static SerializableBuildPlayerOptions Instance
	{
		get
		{
			if (pInstance == null)
			{
				pInstance = CustomSO.LoadScriptableObject<SerializableBuildPlayerOptions>(SCRIPTABLE_NAME);
				Debug.Assert(pInstance != null, "SerializableBuildPlayerOptions instance is null");
			}

			return pInstance as SerializableBuildPlayerOptions;
		}

		set
		{
			if (pInstance == null)
			{
				pInstance = CustomSO.LoadScriptableObject<SerializableBuildPlayerOptions>(SCRIPTABLE_NAME);
				Debug.Assert(pInstance != null, "SerializableBuildPlayerOptions instance is null");
			}

			pInstance = value;
		}
	}



	public string[] scenes;
	public string locationPathName;
	public string assetBundleManifestPath;
	public BuildTargetGroup targetGroup;
	public BuildTarget target;
	public BuildOptions options;

	public static implicit operator BuildPlayerOptions(SerializableBuildPlayerOptions input)
	{
		return new BuildPlayerOptions
		{
			assetBundleManifestPath = input.assetBundleManifestPath,
			locationPathName = input.locationPathName,
			options = input.options,
			scenes = input.scenes,
			target = input.target,
			targetGroup = input.targetGroup,
		};
	}

	public static implicit operator SerializableBuildPlayerOptions(BuildPlayerOptions input)
	{
		return new SerializableBuildPlayerOptions
		{
			assetBundleManifestPath = input.assetBundleManifestPath,
			locationPathName = input.locationPathName,
			options = input.options,
			scenes = input.scenes,
			target = input.target,
			targetGroup = input.targetGroup,
		};
	}
}
#endif