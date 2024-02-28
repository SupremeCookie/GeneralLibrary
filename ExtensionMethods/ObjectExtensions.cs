using System;
using System.Collections.Generic;

public static class ObjectExtensions
{
	public static bool IsPrimitive(this Type type)
	{
		if (type == typeof(String))
			return true;

		return (type.IsValueType & type.IsPrimitive);
	}
}
