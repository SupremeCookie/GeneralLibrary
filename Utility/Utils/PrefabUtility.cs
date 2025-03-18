using UnityEngine;

public static partial class Utility
{
	public static bool IsInPrefabMode(this GameObject gameObject)
	{
		if (gameObject.scene.name == null || gameObject.scene.name == gameObject.name)
			return true;

		return false;
	}
}