public class Tuple<T, Y>
{
	public T First;
	public Y Second;

	public Tuple() { }
	public Tuple(T first, Y second) { First = first; Second = second; }

	public override string ToString()
	{
		var firstVal = First != null ? First.ToString() : GeneralConstants.NULL_STRING;
		var secondVal = Second != null ? Second.ToString() : GeneralConstants.NULL_STRING;

		return string.Format("{0}, {1}", firstVal, secondVal);
	}
}

public class Tuple<T, Y, U>
{
	public T First;
	public Y Second;
	public U Third;

	public Tuple() { }
	public Tuple(T first, Y second, U third) { First = first; Second = second; Third = third; }

	public override string ToString()
	{
		var firstVal = First != null ? First.ToString() : GeneralConstants.NULL_STRING;
		var secondVal = Second != null ? Second.ToString() : GeneralConstants.NULL_STRING;
		var thirdVal = Third != null ? Third.ToString() : GeneralConstants.NULL_STRING;

		return string.Format("{0}, {1}, {2}", firstVal, secondVal, thirdVal);
	}
}
