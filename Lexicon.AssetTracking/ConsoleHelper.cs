using System;
using System.Collections.Generic;
using System.Text;

namespace Lexicon.AssetTracking;

public static class ConsoleHelper
{
	public static void Print(string text, ConsoleColor? color = null)
	{
		if (color.HasValue)
		{
			Console.ForegroundColor = color.Value;
		}

		Console.Write(text);
		Console.ResetColor();
	}

	public static void PrintLine(string text, ConsoleColor? color = null)
	{
		if (color.HasValue)
		{
			Console.ForegroundColor = color.Value;
		}

		Console.WriteLine(text);
		Console.ResetColor();
	}

	public static void PrintLine() => 
		Console.WriteLine();
}
