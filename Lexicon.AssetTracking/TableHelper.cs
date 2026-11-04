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

	
}
