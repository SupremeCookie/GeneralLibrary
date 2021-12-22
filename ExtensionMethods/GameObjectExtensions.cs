using UnityEngine;

public static class GameObjectExtensions
{
	public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
	{
		Debug.Assert(gameObject != null, "GameObject toa dd component to is null");

		T comp = gameObject.GetComponent<T>();
		if (comp == null)
		{
			comp = gameObject.AddComponent<T>();
		}

		return comp;
	}
}
