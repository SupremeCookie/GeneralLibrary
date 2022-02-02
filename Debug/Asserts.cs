using System.Collections.Generic;
using System.Diagnostics;

public static class Asserts
{
	[Conditional("DEBUG")]
	public static void NotNullCheck(object obj)
	{
		Debug.Assert(obj != null, $"Object is null, please fix");
	}

	//[Conditional("DEBUG")]
	//public static void NotNullOrEmptyCheck(object[] obj)
	//{
	//	Debug.Assert(!obj.IsNullOrEmpty(), $"Array is either null or empty, please fix");
	//}

	[Conditional("DEBUG")]
	public static void NotNullOrEmptyCheck<T>(IList<T> obj)
	{
		Debug.Assert(!obj.IsNullOrEmpty(), $"IList is either null or empty, please fix");
	}

	[Conditional("DEBUG")]
	public static void NotEmptyTransform(UnityEngine.Transform trans)
	{
		Debug.Assert(trans.childCount > 0, $"Transform with name: ({trans.gameObject.name}) has no children, please fix");
	}

	[Conditional("DEBUG")]
	public static void CountsMatch(IList<object> firstList, IList<object> secondList)
	{
		NotNullCheck(firstList);
		NotNullCheck(secondList);

		var firstCount = firstList?.Count ?? -1;
		var secondCount = secondList?.Count ?? -1;
		Debug.Assert(firstCount >= 0, $"First list's count isn't above 0, count check can't happen");
		Debug.Assert(secondCount >= 0, $"Second list's count isn't above 0, count check can't happen");

		Debug.Assert(firstCount != secondCount, $"Counts don't match up, firstCount: ({firstCount}),  secondCount: ({secondCount})");
	}
}
