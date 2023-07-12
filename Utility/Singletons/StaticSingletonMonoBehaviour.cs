using UnityEngine;

/// <summary>
/// Static Singletons need to exist manually, they aren't automatically created.
/// Next to that, they are destroyed on loading a different scene, this to ensure they're only existent in specific cases.
/// </summary>
public class StaticSingletonMonoBehaviour<T> : MonoBehaviour where T : StaticSingletonMonoBehaviour<T>
{
	[Readonly]
	public bool HasAwakened;

	protected static T pInstance;
	public static T Instance
	{
		get
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				var existingInstances = GameObject.FindObjectsOfType<T>();
				if (existingInstances != null && existingInstances.Length > 0)
				{
					//Debug.Log("<b>StaticSingletonMonoBehaviour</b> -- Found an existing instance whilst in EDIT mode.");
					return existingInstances[0];
				}
			}
#endif

			return pInstance;
		}
	}

	public static bool HasInstance { get { return pInstance != null; } }

	protected virtual void Awake()
	{
		if (!HasAwakened)
		{
			HasAwakened = true;
		}

		if (pInstance == null)
		{
			pInstance = (T)this;
			// DontDestroyOnLoad(_instance);
		}
		else
		{
			if (pInstance.gameObject == null)
			{
				Debug.Log($"Existing instance of type :({typeof(T)})'s gameobject is null, replacing pInstance with new instance");
				pInstance = (T)this;
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}
