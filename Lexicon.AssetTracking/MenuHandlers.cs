namespace Lexicon.AssetTracking;

public static class MenuHandlers
{
	public static void HandleAddAsset(AssetManager assetManager)
	{
		ConsoleHelper.PrintMenuHeader("ADD ASSET", 50);

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

	public static void HandleRemoveAsset(AssetManager assetManager)
	{
		ConsoleHelper.PrintMenuHeader("REMOVE ASSET", 50);

		var assetId = InputHelper.ReadInput<int>("Enter asset ID to remove: ", input =>
		{
			var isValid = int.TryParse(input, out var value) && value > 0;
			return (isValid, value);
		}, 
		"Invalid asset ID. Please enter a positive integer.");

		if (!assetManager.ContainsAsset(assetId))
		{
			ConsoleHelper.PrintLine($"Asset with ID {assetId} not found.", ConsoleColor.Red);
			return;
		}

		bool confirmation = InputHelper.ReadConfirmation(
			prompt: $"Are you sure you want to remove the asset with ID {assetId}? (y/n): ",
			errorMessage: "Invalid input. Please enter 'y' for yes or 'n' for no.");

		if (!confirmation)
		{
			ConsoleHelper.PrintLine("Asset removal canceled.", ConsoleColor.Yellow);
			return;
		}

		assetManager.RemoveAsset(assetId);

		ConsoleHelper.PrintLine($"Asset with ID {assetId} has been removed.", ConsoleColor.Green);
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

		ConsoleHelper.PrintMenuHeader("ASSET LIST", 50);

		var assets = assetManager.GetSortedAssets();

		if (assets.Count is 0)
		{
			ConsoleHelper.PrintLine("No assets found.", ConsoleColor.Yellow);
			return;
		}

		int[] widths = [.. columnDefs
			.Select(cd => TableHelper
			.GetMaxLength(assets, cd.Header, cd.ValueSelector))];

		var dividerLength = widths.Sum() + (columnDefs.Count - 1) * Separator.Length;

		ConsoleHelper.PrintDivider(dividerLength);
		ConsoleHelper.PrintRow(columnDefs, widths, Separator, cd => cd.Header);
		ConsoleHelper.PrintDivider(dividerLength);

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
