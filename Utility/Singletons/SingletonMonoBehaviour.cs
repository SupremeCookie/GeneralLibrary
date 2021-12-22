using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	[Readonly]
	public bool HasAwakened;

	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
#if UNITY_EDITOR
				if (!Application.isPlaying)
				{
					var existingInstances = GameObject.FindObjectsOfType<T>();
					if (existingInstances != null && existingInstances.Length > 0)
					{
						Debug.Log($"<b><color=#25a0ff>SingletonMonoBehaviour</color></b> -- Found an existing instance ({typeof(T).ToString()}) whilst in EDIT mode.");
						_instance = existingInstances[0];
						return _instance;
					}
				}
#endif

				string instanceName = typeof(T) + "_instanced";
				GameObject newInstance = new GameObject(instanceName);
				newInstance.AddComponent<T>();

				Debug.LogFormat("<b><color=#25a0ff>SingletonMonoBehaviour</color></b> -- Created an instance of <b>{0}</b> its gameObject is called {1}", typeof(T), instanceName);
			}

			if (_instance == null)
				Debug.LogWarningFormat("b><color=#25a0ff>SingletonMonoBehaviour</color></b> -- Instance for type {0} is null", typeof(T));

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
