using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;

public enum SpritePlaceType
{
	Normal,
	Ground,
	Object,
}

public enum DefaultActiveState
{
	All_Active,
	All_Disabled,
	DisabledParent_ActiveSprite,
	ActiveParent_DisabledSprite,
}

public class SpritePlacerData
{
	public UnityEngine.Vector2 Position;
	public UnityEngine.Sprite Sprite;
	public int SortingOrder;
	public SpritePlaceType PlacementType;
	public byte GroundSpriteMask;
}

public class SpriteInstanceData
{
	public GameObject RendererHost;
	public Vector2 Position;
	//	public SpriteRenderer Renderer;
}

public class SpritePlacer : SingletonMonoBehaviour<SpritePlacer>
{
	// Note DK: This class will receive data to spawn SpriteRenderers
	// It will keep track of spawned objects through a class object that specifics a string identifier, and then a list of GameObjects.

	// TODO DK: Research on how to send over data thread safely.

	private class InternalJobRegistration
	{
		public string groupJobName; // Note DK: This way we register jobs within groups, whilst the job itself has a unique name
		public string jobName;
		public DefaultActiveState defaultActiveState; // Note DK: The default state a sprite will have.
		public List<SpritePlacerData> spritePlacerData;
	}

#pragma warning disable 0649
	[SerializeField]
	private GameObject _spritePrefab;
	[SerializeField] public SpritePrefabs spritePrefabs;
#pragma warning restore 0649

#if UNITY_EDITOR
	[SerializeField][Readonly] private int _currentJobRegister;
	[SerializeField][Readonly] private int _currentActiveSubJobs;
#endif

	private Dictionary<string, List<GUID>> _jobRegister;
	private Dictionary<GUID, Coroutine> _subJobs;
	private ConcurrentQueue<InternalJobRegistration> _registeredJobsQueue;
	private Dictionary<string, Transform> _jobParentItems;

	public static Dictionary<string, List<SpriteInstanceData>> JobResults { get; private set; }

	public static bool IsBusy(string jobName)
	{
		Debug.Assert(Instance != null, "Can't check for values on the instance of SpritePlacer, its null");


		bool registerIsBusy = false;
		var jobRegister = Instance._jobRegister;
		if (jobRegister != null && jobRegister.Count > 0)
		{
			registerIsBusy = jobRegister.ContainsKey(jobName);
		}

		if (registerIsBusy) { return true; }


		bool jobQueueIsBusy = false;
		var jobQueue = Instance._registeredJobsQueue;
		if (jobQueue != null && jobQueue.Count > 0)
		{
			var queueAsArray = new InternalJobRegistration[jobQueue.Count];
			jobQueue.CopyTo(queueAsArray, 0);

			for (int i = 0; i < queueAsArray.Length; ++i)
			{
				if (queueAsArray[i].jobName.Equals(jobName))
				{
					jobQueueIsBusy = true;
					break;
				}
			}
		}

		if (jobQueueIsBusy) { return true; }


		return false;
	}

	//public static bool SetBusy()
	//{

	//}


	private void Update()
	{
		if (_registeredJobsQueue != null && _registeredJobsQueue.Count > 0)
		{
			if (_jobParentItems == null)
			{
				_jobParentItems = new Dictionary<string, Transform>();
			}

			for (int i = 0; i < _registeredJobsQueue.Count; ++i)
			{
				_registeredJobsQueue.TryDequeue(out var data);

				if (!_jobRegister.ContainsKey(data.jobName))
				{
					_registeredJobsQueue.Enqueue(data);
					continue;
				}


				//Debug.Assert(_jobRegister.ContainsKey(data.jobName), $"No Job with name: ({data.jobName}) registered, please fix");
				if (_jobRegister[data.jobName] == null)
				{
					_jobRegister[data.jobName] = new List<GUID>();
				}

				GUID guid = new GUID();
				_jobRegister[data.jobName].Add(guid);



				bool activeState = data.defaultActiveState == DefaultActiveState.All_Active || data.defaultActiveState == DefaultActiveState.ActiveParent_DisabledSprite;

				// TODO DK: There's 2 spaces where we rely on a RogueLike specific thing, please refactor.
				// TODO DK: Also, we really shouldn't access the placementType like this.
				var placementType = data.spritePlacerData[0].PlacementType;

#if RogueLike
				var containerObjectForParent = placementType == SpritePlaceType.Ground ? RogueLike.LevelSpriteObjectCreator.Instance.GroundTileContainer : null;
				Debug.Assert(RogueLike.LevelSpriteObjectCreator.Instance.GroundTileContainer != null, $"RogueLike.LevelSpriteObjectCreator.Instance.GroundTileContainer is null, {RogueLike.LevelSpriteObjectCreator.Instance?.name ?? "emptyName"}", RogueLike.LevelSpriteObjectCreator.Instance);
#else
				Transform containerObjectForParent = null;
#endif

				var parentObject = GetParentObject(data.jobName, containerObjectForParent);
				parentObject.gameObject.SetActive(activeState);

				if (JobResults == null)
				{
					JobResults = new Dictionary<string, List<SpriteInstanceData>>();
				}

				if (!JobResults.ContainsKey(data.jobName))
				{
					JobResults.Add(data.jobName, new List<SpriteInstanceData>());
				}

				var subParentObj = GetParentObject(guid.ToString(), parentObject);
				var coroutine = Instance.StartCoroutine(ExecuteJob(data.spritePlacerData, guid, subParentObj, data.jobName, data.defaultActiveState));
				_subJobs.Add(guid, coroutine);
			}
		}

		if (_jobRegister != null && (_registeredJobsQueue == null || _registeredJobsQueue.IsEmpty)) // In case we got subjobs waiting for registration, we can't remove the jobs registration.
		{
			CleanupJobRegister();
		}
	}


	public static void RegisterJob(string groupJobName, List<string> jobNames, List<List<SpritePlacerData>> jobData, DefaultActiveState spriteDefaultActiveState)
	{
		Debug.Assert(jobNames.Count == jobData.Count, $"Given list of jobNames doesn't comply with list of jobDatas, countNames:({jobNames.Count}), countData:({jobData.Count})");

		if (Instance._jobRegister == null)
		{
			Instance._jobRegister = new Dictionary<string, List<GUID>>();
		}

		if (Instance._subJobs == null)
		{
			Instance._subJobs = new Dictionary<GUID, Coroutine>();
		}

		if (Instance._registeredJobsQueue == null)
		{
			Instance._registeredJobsQueue = new ConcurrentQueue<InternalJobRegistration>();
		}

		for (int i = 0; i < jobData.Count; ++i)
		{
			var currentJobName = jobNames[i];

			Instance._jobRegister.Add(currentJobName, null);
			Instance._registeredJobsQueue.Enqueue(new InternalJobRegistration
			{
				spritePlacerData = jobData[i],
				jobName = currentJobName,
				defaultActiveState = spriteDefaultActiveState,
				groupJobName = groupJobName,
			});
		}
	}


	private static IEnumerator ExecuteJob(List<SpritePlacerData> jobData, GUID guid, Transform parentObject, string jobName, DefaultActiveState defaultActiveState)
	{
		float timeoutTime = 15;
		float currentRunoutTime = 0f;

		int currentIndex = 0;
		int countPerYield = 50;

		bool shouldRun = true;
		while (shouldRun)
		{
			if (currentRunoutTime > timeoutTime)
			{
				Debug.LogError($"Oy, we ran out, not good mate, job data size:({jobData.Count}), currentIndex:({currentIndex})");
				yield break;
			}

			int count = 0;
			for (int i = currentIndex; i < jobData.Count && count < countPerYield; ++i, ++count)
			{
				var currentData = jobData[i];
				var spriteObj = Instance.CreateSprite(currentData, in parentObject, defaultActiveState);

				JobResults[jobName].Add(new SpriteInstanceData { RendererHost = spriteObj, Position = currentData.Position, /*Renderer = spriteObj.GetComponent<SpriteRenderer>(), */});
			}

			currentIndex += count;

			if (currentIndex >= jobData.Count - 1)
			{
				shouldRun = false;
			}

			currentRunoutTime += Time.deltaTime;
			yield return null;
		}



		if (Instance._subJobs.ContainsKey(guid))
		{
			Instance._subJobs.Remove(guid);
		}
		else
		{
			Debug.LogError($"Can't remove job, identified by identifier: ({guid.ToString()})");
		}
	}


	private Transform GetParentObject(string name, Transform objParent = null)
	{
		if (!_jobParentItems.ContainsKey(name))
		{
			var newGO = new GameObject(name);

			if (objParent != null)
			{
				newGO.transform.SetParent(objParent, false);
			}

			_jobParentItems.Add(name, newGO.transform);
		}

		return _jobParentItems[name];
	}

	private GameObject CreateSprite(SpritePlacerData data, in Transform parentObject, DefaultActiveState defaultActiveState)
	{
		GameObject spriteInstance = null;

#if RogueLike
		if (data.PlacementType == SpritePlaceType.Normal)
		{
#endif
			spriteInstance = GameObject.Instantiate(_spritePrefab);
			spriteInstance.transform.position = data.Position;
			spriteInstance.transform.SetParent(parentObject, true);

			return spriteInstance;

#if RogueLike
		}
		else
		{
			Debug.Assert(data.PlacementType == SpritePlaceType.Ground, $"Trying to spawn a non placementType Ground, make a case for this: ({data.PlacementType})");
			spriteInstance = RogueLike.LevelSpriteObjectCreator.GetGroundTileInstance(data.Position, data.GroundSpriteMask, addCollider: true, container: parentObject);
		}

		var spriteRenderer = spriteInstance.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = data.Sprite;
		spriteRenderer.sortingOrder = data.SortingOrder;

		// Note DK: The activeState bool is true in case of the two given states.
		bool activeState = defaultActiveState == DefaultActiveState.All_Active || defaultActiveState == DefaultActiveState.DisabledParent_ActiveSprite;
		spriteInstance.SetActive(activeState);

		return spriteInstance;
#endif
	}


	private void CleanupJobRegister()
	{
		// We clean up the job register by checking if any of the subjobs we stored aren't already removed
		// If so we remove it from the registration, if the count reaches 0, we remove the job all together.
		foreach (KeyValuePair<string, List<GUID>> registration in _jobRegister)
		{
			var subJobs = registration.Value;
			if (subJobs == null)
			{
				continue;   // Note DK: Initial state of registration is a Null list.
			}

			for (int i = subJobs.Count - 1; i >= 0; --i)
			{
				if (!_subJobs.ContainsKey(subJobs[i]))
				{
					subJobs.RemoveAt(i);
				}
			}

			if (subJobs.Count == 0)
			{
				//Debug.Log($"Cleanup Job: ({registration.Key})");
				CleanupRegisteredJob(registration.Key);
				break;
			}
		}
	}

	private void CleanupRegisteredJob(string jobName)
	{
		Debug.Assert(_jobRegister != null && _jobRegister.ContainsKey(jobName), $"There aren't any job registrations with the name: ({jobName}), don't call this method, or do fix the issue");
		_jobRegister.Remove(jobName);
	}

	public static void Cleanup()
	{
		SpritePlacer.Instance._jobRegister = null;
		SpritePlacer.Instance._subJobs = null;
		SpritePlacer.Instance._registeredJobsQueue = null;
		SpritePlacer.Instance._jobParentItems = null;
		JobResults = null;

	}


#if UNITY_EDITOR
	private void OnValidate()
	{
		_currentJobRegister = _jobRegister?.Count ?? 0;
		_currentActiveSubJobs = _subJobs?.Count ?? 0;
	}
#endif
}