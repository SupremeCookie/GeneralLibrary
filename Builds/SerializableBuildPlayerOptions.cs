
#if UNITY_EDITOR
using UnityEditor;

[System.Serializable]
public class SerializableBuildPlayerOptions
{
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