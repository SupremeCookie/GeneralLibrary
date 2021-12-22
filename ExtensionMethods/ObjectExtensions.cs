using System;
using System.Collections.Generic;

public static class ObjectExtensions
{
	public static bool IsPrimitive(this Type type)
	{
		if (type == typeof(String)) return true;
		return (type.IsValueType & type.IsPrimitive);
	}
}

public class ReferenceEqualityComparer : EqualityComparer<Object>
{
	public override bool Equals(object x, object y)
	{
		return ReferenceEquals(x, y);
	}
	public override int GetHashCode(object obj)
	{
		if (obj == null) return 0;
		return obj.GetHashCode();
	}
}
