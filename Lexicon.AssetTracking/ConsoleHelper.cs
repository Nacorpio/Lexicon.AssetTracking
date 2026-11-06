namespace Lexicon.AssetTracking;

public static class ConsoleHelper
{
	private readonly static Dictionary<MenuOption, string> MenuOptionTexts = new()
	{
		{ MenuOption.AddAsset, "Add a new asset" },
		{ MenuOption.ViewAssets, "List all assets" },
		{ MenuOption.SortAssets, "Sort assets"  },
		{ MenuOption.SearchAssets, "Search assets" },
		{ MenuOption.RemoveAsset, "Remove an asset" },

		{ MenuOption.Exit, "Exit" }
	};

	public static void PrintMenu()
	{
		ConsoleHelper.PrintLine("MENU OPTIONS", ConsoleColor.Cyan);
		
		foreach (var option in Enum.GetValues<MenuOption>())
		{
			ConsoleHelper.PrintLine($"{(int)option}. {MenuOptionTexts[option]}");
		}
	}

	public static void Print(string text, ConsoleColor? color = null)
	{
		if (color.HasValue)
			Console.ForegroundColor = color.Value;

		Console.Write(text);
		Console.ResetColor();
	}

	public static void PrintLine(string text, ConsoleColor? color = null)
	{
		if (color.HasValue)
			Console.ForegroundColor = color.Value;

		Console.WriteLine(text);
		Console.ResetColor();
	}

	public static void PrintLine() => 
		Console.WriteLine();


	public static void PrintDivider(int length, char character = '-') =>
		ConsoleHelper.PrintLine(new string(character, length));

	public static void PrintMenuHeader(string header, int length)
	{
		ConsoleHelper.PrintDivider(length);
		ConsoleHelper.PrintLine(header, ConsoleColor.Cyan);
		ConsoleHelper.PrintDivider(length);
	}


	public static void PrintRow(
		IReadOnlyList<ColumnDefinition> columnDefs,
		int[] widths,
		string separator,
		Func<ColumnDefinition, string> getText,
		ConsoleColor? color = null)
	{
		for (var i = 0; i < columnDefs.Count; i++)
		{
			if (i > 0)
				ConsoleHelper.Print(separator);

			ConsoleHelper.Print(TableHelper.AlignText(getText(columnDefs[i]), widths[i], columnDefs[i].Alignment), color);
		}

		ConsoleHelper.PrintLine();
	}

	public static void PrintTable(
		IReadOnlyList<ColumnDefinition> columnDefs,
		IReadOnlyList<Asset> assets,
		string separator,
		DateTime today)
	{
		if (assets.Count is 0)
		{
			ConsoleHelper.PrintLine("No assets found.", ConsoleColor.Yellow);
			return;
		}

		int[] widths = [.. columnDefs
			.Select(cd => TableHelper
			.GetMaxLength(assets, cd.Header, cd.ValueSelector))];

		var dividerLength = widths.Sum() + (columnDefs.Count - 1) * separator.Length;

		ConsoleHelper.PrintDivider(dividerLength);
		ConsoleHelper.PrintRow(columnDefs, widths, separator, cd => cd.Header);
		ConsoleHelper.PrintDivider(dividerLength);

		foreach (var asset in assets)
		{
			var color = GetStatusColor(asset.GetStatus(today));
			ConsoleHelper.PrintRow(columnDefs, widths, separator, cd => cd.ValueSelector(asset), color);
		}
	}

	private static ConsoleColor GetStatusColor(AssetStatus status) =>
		status switch
		{
			AssetStatus.Red => ConsoleColor.Red,
			AssetStatus.Yellow => ConsoleColor.Yellow,

			_ => ConsoleColor.White
		};
}
