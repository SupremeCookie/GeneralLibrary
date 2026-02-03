using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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



	public InternalBuildSettings options;
}


[System.Serializable]
public class InternalBuildSettings
{
#if UNITY_EDITOR
	public string[] scenes;
	public string locationPathName;
	public string assetBundleManifestPath;
	public BuildTargetGroup targetGroup;
	public BuildTarget target;
	public BuildOptions options;

	public static implicit operator BuildPlayerOptions(InternalBuildSettings input)
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

	public static implicit operator InternalBuildSettings(BuildPlayerOptions input)
	{
		return new InternalBuildSettings
		{
			assetBundleManifestPath = input.assetBundleManifestPath,
			locationPathName = input.locationPathName,
			options = input.options,
			scenes = input.scenes,
			target = input.target,
			targetGroup = input.targetGroup,
		};
	}
#endif
}