using System.Collections.Generic;

[System.Serializable]
public class IndexableComparableClass<T> where T : ComparableClass<T>
{
	[UnityEngine.SerializeField] protected List<T> _data;

	public T this[int index]
	{
		get => _data[index];
		set => _data[index] = value;
	}

	public void Sort()
	{
		_data.Sort();
	}

	public int Count
	{
		get => _data.Count;
	}


	public IndexableComparableClass()
	{
		_data = new List<T>();
	}

	public IndexableComparableClass(int capacity)
	{
		_data = new List<T>(capacity);
	}

	public void Add(T data)
	{
		_data.Add(data);
	}

	public void AddData(T data)
	{
		_data.Add(data);
	}

	public void AddRange(List<T> data)
	{
		_data.AddRange(data);
	}

	public void AddRange(T[] data)
	{
		_data.AddRange(data);
	}

	public List<T> GetData()
	{
		return _data;
	}
}

public static class IndexableComparableClassExtensions
{
	public static bool IsNullOrEmpty<T>(this IndexableComparableClass<T> coll) where T : ComparableClass<T>
	{
		return coll == null || coll.Count == 0;
	}

	public static T GetValue<T>(this IndexableComparableClass<T> coll, int indexValue) where T : ComparableClass<T>
	{
		var data = coll.GetData();
		if (data.IsNullOrEmpty())
		{
			return null;
		}

		for (int i = 0; i < data.Count; ++i)
		{
			if (data[i].Index == indexValue)
			{
				return data[i];
			}
		}

		return null;
	}
}
