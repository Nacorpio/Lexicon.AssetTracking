namespace Lexicon.AssetTracking;

public static class TableHelper
{
	public static string AlignText(string text, int width, ColumnAlignment alignment) =>
		alignment switch
		{
			ColumnAlignment.Left => text.PadRight(width),
			ColumnAlignment.Right => text.PadLeft(width),
			_ => throw new InvalidOperationException($"Unknown alignment: {alignment}")
		};

	public static int GetMaxLength<T>(IEnumerable<T> items, string column, Func<T, string> selector) =>
		Math.Max(column.Length, items.Max(item => selector(item).Length));

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
