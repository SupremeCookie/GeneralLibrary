using System.Collections.Generic;
using UnityEngine;

namespace Triangulator
{
	public class Ear
	{
		public Vertex[] vertices = new Vertex[3];

		private float area;
		private bool areaIsDirty = true;

		public Ear() { areaIsDirty = true; }
		public Ear(Vertex first, Vertex second, Vertex third) { StoreVertices(first, second, third); }

		public bool ContainsVertex(Vertex vertex)
		{
			Debug.Assert(vertex.id >= 0, $"Vertex ID is below 0, that is an illegal vertex, {vertex}");
			return vertices.Contains(vertex);
		}

		public bool ContainsVertices(List<Vertex> vertices)
		{
			for (int i = 0; i < vertices.Count; ++i)
			{
				var containsVert = ContainsVertex(vertices[i]);
				if (containsVert)
				{
					return true;
				}
			}

			return false;
		}

		public bool ContainsVertices(Vertex[] vertices)
		{
			for (int i = 0; i < vertices.Length; ++i)
			{
				var containsVert = ContainsVertex(vertices[i]);
				if (containsVert)
				{
					return true;
				}
			}

			return false;
		}

		public float GetArea()
		{
			if (areaIsDirty)
			{
				area = EarUtility.CalculateArea(vertices);

				bool hasArea = area > 0;
				Debug.Assert(hasArea, $"Area is not above 0, this means CalculateArea failed, or was not correctly done, will set areaIsDirty to true so we can retry next time. Area: {area}");

				areaIsDirty = false;
				if (!hasArea)
				{
					areaIsDirty = true;
				}
			}

			return area;
		}

		private void StoreVertices(Vertex first, Vertex second, Vertex third)
		{
			vertices[0] = first;
			vertices[1] = second;
			vertices[2] = third;
			areaIsDirty = true;
		}


		public override string ToString()
		{
			return $"{vertices[0]}, {vertices[1]}, {vertices[2]}";
		}
	}
}
