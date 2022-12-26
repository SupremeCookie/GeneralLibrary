using System.Collections.Generic;
using UnityEngine;

namespace Triangulator
{
	public static class VertexExtensions
	{
		public static int[] GetIDs(this Vertex[] vertices)
		{
			Debug.Assert(!vertices.IsNullOrEmpty(), $"No vertices passed to method, can't return ids");

			return GetIDs(new List<Vertex>(vertices));
		}

		public static int[] GetIDs(this List<Vertex> vertices)
		{
			Debug.Assert(!vertices.IsNullOrEmpty(), $"No vertices passed to method, can't return ids");

			int[] result = new int[vertices.Count];
			for (int i = 0; i < vertices.Count; ++i)
			{
				result[i] = vertices[i].id;
			}

			return result;
		}

		public static bool ContainsCloseEnough(this List<Vertex> input, in Vertex containingPoint)
		{
			if (input.IsNullOrEmpty())
			{
				return false;
			}

			for (int i = 0; i < input.Count; ++i)
			{
				var vertexPositionsCloseEnough = input[i].position.IsCloseTo(containingPoint.position, 0.001f);
				if (vertexPositionsCloseEnough)
				{
					return true;
				}
			}

			return false;
		}


		public static bool ContainsCloseEnough(this Vertex[] input, in Vertex containingPoint)
		{
			if (input.IsNullOrEmpty())
			{
				return false;
			}

			for (int i = 0; i < input.Length; ++i)
			{
				var vertexPositionsCloseEnough = input[i].position.IsCloseTo(containingPoint.position, 0.001f);
				if (vertexPositionsCloseEnough)
				{
					return true;
				}
			}

			return false;
		}
	}
}
