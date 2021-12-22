using System;

[System.Serializable]
public class ComparableClass<T> : IComparable where T : ComparableClass<T>
{
	public int Index;

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}

		var otherData = obj as T;
		if (otherData == null)
		{
			throw new System.ArgumentException($"Object is not a {typeof(T).ToString()}");
		}

		return this.Index.CompareTo(otherData.Index);
	}
}
