namespace Lexicon.AssetTracking;

public static class MenuHandlers
{
	public static void HandleAddAsset(AssetManager assetManager)
	{
		ConsoleHelper.Print("ADD ASSET");
		ConsoleHelper.PrintLine();

		var officeNames = Enum.GetNames<OfficeLocation>();

		var assetType = InputHelper.ReadEnum<AssetType>("Enter asset type (Laptop, Phone, Desktop, Tablet): ", "Invalid asset type. Please try again.");
		var brand = InputHelper.ReadNonEmptyString("Enter brand: ", "Brand cannot be empty. Please try again.");
		var model = InputHelper.ReadNonEmptyString("Enter model: ", "Model cannot be empty. Please try again.");
		var office = InputHelper.ReadEnum<OfficeLocation>($"Enter office location ({string.Join(", ", officeNames)}): ", "Invalid office location. Please try again.");
		var priceUsd = InputHelper.ReadDecimal("Enter price in USD: ", "Invalid price. Please enter a valid decimal number.");
		var purchaseDate = InputHelper.ReadDate("Enter purchase date (yyyy-MM-dd): ", "Invalid date format. Please enter the date in yyyy-MM-dd format.");

		var asset = assetManager.AddAsset(assetType, brand!, model!, office, priceUsd, purchaseDate);

		ConsoleHelper.PrintLine($"Asset added: {asset.GetDescription()}", ConsoleColor.Green);
	}

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
	}

	private static string FormatPriceLocal(Asset asset) =>
		$"{asset.PriceLocal:F2} {CurrencyConverter.GetCurrencyCode(asset.Office)}";

	private static string GetStatusString(Asset asset, DateTime today) =>
		asset.GetStatus(today) switch
		{
			AssetStatus.Red => "RED",
			AssetStatus.Yellow => "YELLOW",

			_ => string.Empty
		};

	private static ConsoleColor GetStatusColor(AssetStatus status) =>
		status switch
		{
			AssetStatus.Red => ConsoleColor.Red,
			AssetStatus.Yellow => ConsoleColor.Yellow,

			_ => ConsoleColor.White
		};
}
