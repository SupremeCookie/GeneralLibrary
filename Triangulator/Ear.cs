using System.Collections.Generic;
using System.Linq;
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
				if (!hasArea)
				{
					// Note DK: Sometimes we can get ears that are actually lines, we can check this.
					var sortedVertices = vertices.OrderBy(s => s.position.x).ToArray();
					Vertex getLeftMostVertex()
					{
						return sortedVertices[0];
					}

					Vertex getRightMostVertex()
					{
						return sortedVertices[2];
					}

					Vertex getMiddleVertex()
					{
						return sortedVertices[1];
					}


					var farLeftVertex = getLeftMostVertex();
					var farRightVertex = getRightMostVertex();
					var middleVertex = getMiddleVertex();

					var leftToRight = farRightVertex.position - farLeftVertex.position;
					leftToRight.Normalize();

					var leftToMiddle = middleVertex.position - farLeftVertex.position;
					leftToMiddle.Normalize();

					float dot = Vector2.Dot(leftToRight, leftToMiddle);
					if (dot >= 0.98f)
					{
						// Note DK: This means the ear is a line.
						Debug.LogWarning($"We've encountered an ear that's basically a line, this means we have 0 area");
						return 0;
					}
				}

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
