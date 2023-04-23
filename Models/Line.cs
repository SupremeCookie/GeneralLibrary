using UnityEngine;

public struct Line
{
	private Vector2 _start;
	public Vector2 start { get { return _start; } set { _start = value; } }

	private Vector2 _end;
	public Vector2 end { get { return _end; } set { _end = value; } }


	public Line(Vector2 start, Vector2 end) { _start = start; _end = end; }
	public Line(Line copy) { _start = copy.start; _end = copy.end; }


	public Vector2 GetDelta()
	{
		return end - start;
	}
}