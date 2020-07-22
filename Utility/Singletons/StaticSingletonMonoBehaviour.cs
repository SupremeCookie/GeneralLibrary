using UnityEngine;

public class StaticSingletonMonoBehaviour<T> : MonoBehaviour where T : StaticSingletonMonoBehaviour<T>
{
	public bool HasAwakened;

	private static T _instance;
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
					Debug.Log("<b>StaticSingletonMonoBehaviour</b> -- Found an existing instance whilst in EDIT mode.");
					_instance = existingInstances[0];
					return _instance;
				}
			}
#endif

			return _instance;
		}
	}

	public static bool HasInstance { get { return _instance != null; } }

	protected virtual void Awake()
	{
		if (!HasAwakened)
		{
			HasAwakened = true;
		}

		if (_instance == null)
		{
			_instance = (T)this;
			DontDestroyOnLoad(_instance);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
