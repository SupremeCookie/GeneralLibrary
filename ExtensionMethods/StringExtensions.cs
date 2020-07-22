using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StringExtensions
{
	public static string ReplaceNewLines(this string input)
	{
		return input.Replace("\\n", "\n");
	}
}