using System.Collections.Generic;

[System.Serializable]
public class IndexableEnum<T> where T : struct
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


	public IndexableEnum()
	{
		_data = new List<T>();
	}

	public IndexableEnum(int capacity)
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

public static class IndexableEnumExtensions
{
	public static bool IsNullOrEmpty<T>(this IndexableEnum<T> coll) where T : struct
	{
		return coll == null || coll.Count == 0;
	}
}
