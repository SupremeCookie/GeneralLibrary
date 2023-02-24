namespace Triangulator
{
	public class VertexTriangle
	{
		private Vertex[] vertices;

		public Vertex this[int i]
		{
			get { return vertices[i]; }
			set { vertices[i] = value; }
		}


		public VertexTriangle()
		{
			vertices = new Vertex[3];
		}

		public VertexTriangle(Vertex first, Vertex second, Vertex third)
		{
			vertices = new Vertex[3];
			vertices[0] = first;
			vertices[1] = second;
			vertices[2] = third;
		}

		public void Clear()
		{
			//vertices[0] = default(Vertex);
			//vertices[1] = default(Vertex);
			//vertices[2] = default(Vertex);
		}
	}
}