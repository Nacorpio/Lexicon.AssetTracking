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

	public static void PrintRow(
		List<ColumnDefinition> columnDefs,
		int[] widths,
		string separator,
		Func<ColumnDefinition, string> getText,
		ConsoleColor? color = null)
	{
		for (var i = 0; i < columnDefs.Count; i++)
		{
			if (i > 0)
				ConsoleHelper.Print(separator);

			var text = TableHelper.AlignText(getText(columnDefs[i]), widths[i], columnDefs[i].Alignment);
			ConsoleHelper.Print(text, color);
		}

		ConsoleHelper.PrintLine();
	}
}
