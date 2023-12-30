public static class StringExtensions
{
	public static string ReplaceNewLines(this string input)
	{
		return input.Replace("\\n", "\n");
	}
}