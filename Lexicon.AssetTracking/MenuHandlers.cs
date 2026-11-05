namespace Lexicon.AssetTracking;

public static class MenuHandlers
{
	public static void HandleViewAssets(AssetManager assetManager, DateTime today)
	{
		const string Separator = " | ";

		var columnDefs = new List<ColumnDefinition>
		{
			new("Office", ColumnAlignment.Left, a => a.Office.ToString()),
			new("Type", ColumnAlignment.Left, a => a.Type.ToString()),
			new("Brand", ColumnAlignment.Left, a => a.Brand),
			new("Model", ColumnAlignment.Left, a => a.Model),
			new("Purchase Date", ColumnAlignment.Left, a => a.PurchaseDate.ToString("yyyy-MM-dd")),
			new("Price Local", ColumnAlignment.Right, a => FormatPriceLocal(a)),
			new("Price USD", ColumnAlignment.Right, a => $"{a.PriceUsd:F2} USD"),
			new("Status", ColumnAlignment.Left, a => GetStatusString(a, today))
		};

		ConsoleHelper.Print("ASSET LIST");
		ConsoleHelper.PrintLine();

		var assets = assetManager.GetSortedAssets();

		if (assets.Count is 0)
		{
			Console.WriteLine("No assets found.");
			return;
		}

		int[] widths = [.. columnDefs.Select(cd =>
		TableHelper.GetMaxLength(assets, cd.Header, cd.ValueSelector))];

		var lineLength = widths.Sum() + (columnDefs.Count - 1) * Separator.Length;

		ConsoleHelper.PrintLine(new string('-', lineLength));
		ConsoleHelper.PrintRow(columnDefs, widths, Separator, cd => cd.Header);
		ConsoleHelper.PrintLine(new string('-', lineLength));

		foreach (var asset in assets)
		{
			var color = GetStatusColor(asset.GetStatus(today));

			ConsoleHelper.PrintRow(columnDefs, widths, Separator, cd => cd.ValueSelector(asset), color);
		}

		Console.ReadLine();
	}

	static string FormatPriceLocal(Asset asset) =>
	$"{asset.PriceLocal:F2} {CurrencyConverter.GetCurrencyCode(asset.Office)}";

	static string GetStatusString(Asset asset, DateTime today) =>
		asset.GetStatus(today) switch
		{
			AssetStatus.Red => "RED",
			AssetStatus.Yellow => "YELLOW",

			_ => string.Empty
		};

	public static ConsoleColor GetStatusColor(AssetStatus status) =>
		status switch
		{
			AssetStatus.Red => ConsoleColor.Red,
			AssetStatus.Yellow => ConsoleColor.Yellow,

			_ => ConsoleColor.White
		};
}
