using Lexicon.AssetTracking;

var assetManager = new AssetManager();
var today = DateTime.Today;

AddSampleAssets(assetManager, today);

try 
{
	while (true)
	{
		Console.Clear();

		ConsoleHelper.PrintLine("ASSET TRACKING SYSTEM", ConsoleColor.Green);
		ConsoleHelper.PrintLine();

		ConsoleHelper.PrintMenu();
		ConsoleHelper.PrintLine();

		if (!InputHelper.TryReadEnum<MenuOption>("Select an option: ", out var option))
		{
			ConsoleHelper.PrintLine("Invalid option. Please try again.", ConsoleColor.Red);
			Console.ReadLine();
			
			continue;
		}

		Console.Clear();

		switch (option)
		{
			case MenuOption.ViewAssets:
				PrintAssets(assetManager, DateTime.Today);
				break;

			case MenuOption.Exit:
				ConsoleHelper.PrintLine("Exiting the application. Goodbye!");
				return;

			default:
				ConsoleHelper.PrintLine("Invalid option. Please try again.", ConsoleColor.Red);
				break;
		}

		ConsoleHelper.PrintLine("Press ENTER to return to the main menu...");
		Console.ReadLine();
	}
} 
catch (EndOfStreamException)
{
	ConsoleHelper.PrintLine("Input stream closed. Exiting the application.", ConsoleColor.Red);
}
catch (Exception ex)
{
	ConsoleHelper.PrintLine($"An unexpected error occurred: {ex.Message}", ConsoleColor.Red);
}


static void AddSampleAssets(AssetManager assetManager, DateTime today)
{
	// Purchased exactly 3 years ago + N months => end of life is N months from today.
	DateTime EndsIn(int months) => today.AddYears(-3).AddMonths(months);

	assetManager.AddAsset(AssetType.Laptop, "Dell", "XPS 13", OfficeLocation.Sweden, 1200m, EndsIn(0));
	assetManager.AddAsset(AssetType.Phone, "Apple", "iPhone 12", OfficeLocation.Turkey, 800m, EndsIn(6));
	assetManager.AddAsset(AssetType.Tablet, "Samsung", "Galaxy Tab S7", OfficeLocation.USA, 650m, EndsIn(3));
}

static void PrintAssets(AssetManager assetManager, DateTime today)
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

static ConsoleColor GetStatusColor(AssetStatus status) =>
	status switch
	{
		AssetStatus.Red => ConsoleColor.Red,
		AssetStatus.Yellow => ConsoleColor.Yellow,

		_ => ConsoleColor.White
	};
