using System.Collections.Generic;

[System.Serializable]
public class IndexableClass<T> where T : class
{
	[UnityEngine.SerializeField] protected List<T> _data;
	public List<T> Data { set { _data = value; } }

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


	public IndexableClass()
	{
		_data = new List<T>();
	}

	public IndexableClass(int capacity)
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

public static class IndexableClassExtensions
{
	public static bool IsNullOrEmpty<T>(this IndexableClass<T> coll) where T : class
	{
		return coll == null || coll.Count == 0;
	}
}

