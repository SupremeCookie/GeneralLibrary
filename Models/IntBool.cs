/// <summary>
/// C++ has booleans which are interchangeable with ints, and vice versa. 
/// I sorta want that in c# too.
/// </summary>
public class IntBool
{
	private bool _isTrue;



	public bool BoolValue
	{
		get { return _isTrue; }
		set { _isTrue = value; }
	}

	public int IntValue
	{
		get { return _isTrue ? 1 : 0; }
	}

	public float FloatValue
	{
		get { return _isTrue ? 1.0f : 0.0f; }
	}




	public IntBool(bool value)
	{
		_isTrue = value;
	}

	public IntBool(int value)
	{
		_isTrue = value >= 1;
	}

	public IntBool(float value)
	{
		_isTrue = value >= 1.0f;
	}
}
