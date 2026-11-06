namespace Lexicon.AssetTracking;

public static class MenuHandlers
{
	private static IReadOnlyList<ColumnDefinition> GetColumnDefs(DateTime today) =>
	[
		new("Office", ColumnAlignment.Left, a => a.Office.ToString()),
		new("Type", ColumnAlignment.Left, a => a.Type.ToString()),
		new("Brand", ColumnAlignment.Left, a => a.Brand),
		new("Model", ColumnAlignment.Left, a => a.Model),
		new("Purchase Date", ColumnAlignment.Left, a => a.PurchaseDate.ToString("yyyy-MM-dd")),
		new("Price Local", ColumnAlignment.Right, a => FormatPriceLocal(a)),
		new("Price USD", ColumnAlignment.Right, a => $"{a.PriceUsd:F2} USD"),
		new("Age", ColumnAlignment.Right, a => FormatAge(a, today)),
		new("Status", ColumnAlignment.Left, a => GetStatusString(a, today))
	];

	private static readonly Dictionary<SortMenuOption, string> SortMenuOptionTexts = new()
	{
		{ SortMenuOption.ByOfficeThenByDate, "Sort by office then by purchase date" },
		{ SortMenuOption.ByTypeThenByDate, "Sort by type then by purchase date" },
		{ SortMenuOption.ByPurchaseDate, "Sort by purchase date" }
	};

	public static void HandleAddAsset(AssetManager assetManager, DateTime today)
	{
		ConsoleHelper.PrintMenuHeader("ADD ASSET", 50);

		var assetNames = Enum.GetNames<AssetType>();
		var officeNames = Enum.GetNames<OfficeLocation>();

		var assetType = InputHelper.ReadEnum<AssetType>($"Enter asset type ({string.Join(", ", assetNames)}): ", "Invalid asset type. Please try again.");
		var brand = InputHelper.ReadNonEmptyString("Enter brand: ", "Brand cannot be empty. Please try again.");
		var model = InputHelper.ReadNonEmptyString("Enter model: ", "Model cannot be empty. Please try again.");
		var office = InputHelper.ReadEnum<OfficeLocation>($"Enter office location ({string.Join(", ", officeNames)}): ", "Invalid office location. Please try again.");
		var priceUsd = InputHelper.ReadDecimal("Enter price in USD: ", "Invalid price. Please enter a valid decimal number.");
		var purchaseDate = InputHelper.ReadDate("Enter purchase date (yyyy-MM-dd): ", today, "Date must be yyyy-MM-dd and not in the future.");

		var asset = assetManager.AddAsset(assetType, brand, model, office, priceUsd, purchaseDate);

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
		ConsoleHelper.PrintMenuHeader("ASSET LIST", 50);

		const string Separator = " | ";

		var assets = assetManager.GetSortedAssets();

		ConsoleHelper.PrintTable(GetColumnDefs(today), assets, Separator, today);
	}

	public static void HandleSearchAssets(AssetManager assetManager, DateTime today)
	{
		ConsoleHelper.PrintMenuHeader("SEARCH ASSETS", 50);

		var options = Enum.GetNames<SearchMenuOption>();

		var searchOption = InputHelper.ReadEnum<SearchMenuOption>($"Search by ({string.Join(", ", options)}): ", "Invalid option. Please try again.");
		string searchTerm = InputHelper.ReadNonEmptyString("Enter search term: ", "Search term cannot be empty. Please try again.");

		IReadOnlyList<Asset> results = searchOption switch
		{
			SearchMenuOption.ByBrand => assetManager.SearchAssetsByBrand(searchTerm),
			SearchMenuOption.ByModel => assetManager.SearchAssetsByModel(searchTerm),

			_ => []
		};

		const string Separator = " | ";

		ConsoleHelper.PrintTable(GetColumnDefs(today), results, Separator, today);
	}

	public static void HandleSortAssets(AssetManager assetManager, DateTime today)
	{
		ConsoleHelper.PrintMenuHeader("SORT ASSETS", 50);

		foreach (var option in SortMenuOptionTexts)
		{
			ConsoleHelper.PrintLine($"{(int)option.Key}. {option.Value}");
		}

		var sortOption = InputHelper.ReadEnum<SortMenuOption>("Select a sorting option: ", "Invalid option. Please try again.");
		var sortedAssets = assetManager.GetAssetsSorted(sortOption);

		const string Separator = " | ";

		ConsoleHelper.PrintTable(GetColumnDefs(today), sortedAssets, Separator, today);
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

	private static string FormatAge(Asset asset, DateTime today)
	{
		var purchase = asset.PurchaseDate;

		var totalMonths = (today.Year - purchase.Year) * 12 + today.Month - purchase.Month;

		if (today.Day < purchase.Day)
			totalMonths--;

		totalMonths = Math.Max(totalMonths, 0);

		var (years, months) = Math.DivRem(totalMonths, 12);

		return 
			$"{years} {(years == 1 ? "year" : "years")}, " +
			$"{months} {(months == 1 ? "month" : "months")}";
	}
}
