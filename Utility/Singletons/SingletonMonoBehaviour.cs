using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	[SerializeField, Readonly] private string stackTrace;

	[Space(15)]
	[SerializeField] private bool shouldNotDestroyOnLoad = true;

	protected bool pDestroyedObject = false;

	[Readonly]
	public bool HasAwakened;

	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				string instanceName = typeof(T) + "_instanced";
				GameObject newInstance = new GameObject(instanceName);
				newInstance.AddComponent<T>();
				newInstance.GetComponent<T>()?.OnGotCreated();

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

			if (shouldNotDestroyOnLoad)
			{
				DontDestroyOnLoad(_instance);
			}
		}
		else
		{
			pDestroyedObject = true;
			Destroy(gameObject);
		}
	}

	protected virtual void OnGotCreated()
	{
#if UNITY_EDITOR
		stackTrace = Environment.StackTrace;
#endif
	}
}
