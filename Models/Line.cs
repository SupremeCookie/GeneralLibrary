using UnityEngine;

public struct Line
{
	public Vector2 start;
	public Vector2 end;

	public Vector2 deltaVector
	{
		get
		{
			return end - start;
		}
	}

	public Line(Vector2 start, Vector2 end)
	{
		this.start = start;
		this.end = end;
	}

	public Line(Line copy)
	{
		this.start = copy.start;
		this.end = copy.end;
	}
}