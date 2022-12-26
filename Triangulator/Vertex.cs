using UnityEngine;

namespace Triangulator
{
	public struct Vertex
	{
		public int id;
		public Vector2 position;

		public Vertex(int id, Vector2 position) { this.id = id; this.position = position; }


		public override bool Equals(object obj)
		{
			// Only useful if Vertex turns into a class
			//if (ReferenceEquals(obj, null))
			//{
			//	return false;
			//}

			if (obj is Vertex)
			{
				var casted = (Vertex)obj;
				return id == casted.id && position == casted.position;
			}

			return false;   // Types won't match, never equal
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return $"(id: {id}, position: {position})";
		}
	}
}
