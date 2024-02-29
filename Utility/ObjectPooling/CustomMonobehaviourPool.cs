using System.Collections.Generic;
using UnityEngine;

public class CustomMonobehaviourPool<T, U> where U : Component
{
	private const int BASE_SIZE = 10;

	public enum GetObjectReturnValue
	{
		PoolDoesNotExist,
		Success_CreatedNewItems,
		Success,
	}

	public enum CreatePoolReturnValue
	{
		PoolCreated,
		PoolNotCreated_KeyAlreadyPresent,
	}

	private class UWrapper
	{
		public bool isUsed = false;
		public U instance;
	}

	private Transform parentObject
	{
		get { if (_parentObject == null) { CreateParent(); } return _parentObject; }
	}

	private Transform _parentObject;
	private Dictionary<T, IList<UWrapper>> pool;    // Make custom editor to portray this? Gonna be difficult as it needs to be a property drawer :thinking_face:
	private Dictionary<T, GameObject> prefabs;      // In order to grow the pool further, we store the 'impression' of a pool (the prefab)

	public void CreatePool<P>(T key, int size, GameObject prefab, out CreatePoolReturnValue returnValue) where P : U
	{
		if (pool == null)
		{
			pool = new Dictionary<T, IList<UWrapper>>();
		}

		returnValue = CreatePoolReturnValue.PoolNotCreated_KeyAlreadyPresent;

		if (!pool.ContainsKey(key))
		{
			pool.Add(key, new List<UWrapper>(BASE_SIZE));
			returnValue = CreatePoolReturnValue.PoolCreated;
		}


		if (prefabs == null)
		{
			prefabs = new Dictionary<T, GameObject>();
		}

		if (!prefabs.ContainsKey(key))
		{
			prefabs.Add(key, prefab);
		}


		Debug.Log($"CMP - Create pool with: {size} items for: {key}");
		AddToPool<P>(key, size, prefab);
	}

	public void CleanupPool()
	{
		foreach (var collection in pool)
		{
			for (int i = 0; i < collection.Value.Count; ++i)
			{
				Debug.Assert(collection.Value[i] != null, $"No value for collection item in index: {i}");
				if (collection.Value[i].isUsed)
				{
					Debug.Assert(collection.Value[i].instance != null, $"No instance for the value at index: {i}");
					ReturnObject(collection.Key, collection.Value[i].instance);
				}
			}
		}
	}


	private void AddToPool<P>(T key, int size, GameObject prefab) where P : U
	{
		Debug.Assert(pool != null, $"No pool has been made yet, please make it before trying to assign anything");
		Debug.Assert(pool.ContainsKey(key), $"There's no entry with key: {key}, gotta add it first!");

		List<P> newItems = new List<P>(size);
		for (int i = 0; i < size; ++i)
		{
			GameObject newGO = GameObject.Instantiate(prefab);
			newGO.transform.SetParent(parentObject, worldPositionStays: false);

			P comp = newGO.GetComponent<P>();
			Debug.Assert(comp != null, $"No component of type: {typeof(P)} could be found on gameObject", newGO);
			newItems.Add(comp);
		}

		for (int i = 0; i < newItems.Count; ++i)
		{
			pool[key].Add(new UWrapper
			{
				instance = newItems[i]
			});
		}
	}

	public GetObjectReturnValue GetObject(T key, out U value, bool isRecursion = false)
	{
		bool containsKey = pool.ContainsKey(key);
		if (!containsKey)
		{
			value = default;
			return GetObjectReturnValue.PoolDoesNotExist;
		}

		IList<UWrapper> items = pool[key];
		for (int i = 0; i < items.Count; ++i)
		{
			if (!items[i].isUsed)
			{
				items[i].isUsed = true;
				value = items[i].instance;
				return GetObjectReturnValue.Success;
			}
		}

		value = default;
		if (!isRecursion)
		{
			AddToPool<U>(key, BASE_SIZE, prefabs[key]);
			GetObject(key, out value, isRecursion: true);
		}

		return GetObjectReturnValue.Success_CreatedNewItems;
	}

	public void ReturnObject(T key, U value)
	{
		Debug.Assert(value != null, $"The value we're trying to return is null");
		Debug.Assert(value.transform != null, $"The value we're trying to return doesn't have a transform associated");
		Debug.Assert(parentObject != null, $"Our parent object is present");
		Debug.Assert(pool != null, $"There is no object pool to return our items to");
		Debug.Assert(!pool[key].IsNullOrEmpty(), $"The pool we're trying to put our item ({key}) into is null or empty");


		value.transform.SetParent(parentObject, false);

		var poolItems = pool[key];
		for (int i = 0; i < poolItems.Count; ++i)
		{
			if (!poolItems[i].isUsed)
			{
				continue;
			}

			if (poolItems[i].instance == value)
			{
				poolItems[i].isUsed = false;
				break;
			}
		}
	}


	private void CreateParent()
	{
		GameObject newGO = new GameObject("CMP-ParentObject");
		Object.DontDestroyOnLoad(newGO);

		_parentObject = newGO.transform;
		newGO.SetActive(false); // Note DK: ObjectPool objects should always be invisible.
	}
}