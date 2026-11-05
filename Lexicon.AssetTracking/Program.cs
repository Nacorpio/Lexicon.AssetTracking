using Lexicon.AssetTracking;

var assetManager = new AssetManager();
var today = DateTime.Today;

AddSampleAssets(assetManager, today);
PrintAssets(assetManager, today);

static void AddSampleAssets(AssetManager assetManager, DateTime today)
{
	// Purchased exactly 3 years ago + N months => end of life is N months from today.
	DateTime EndsIn(int months) => today.AddYears(-3).AddMonths(months);

	assetManager.AddAsset(new Computer(1, AssetType.Laptop, "Apple", "MacBook Pro", OfficeLocation.Sweden,
		2500m, EndsIn(-2)));                 // expired        -> RED

	assetManager.AddAsset(new MobilePhone(2, "Nokia", "XR20", OfficeLocation.Turkey,
		800m, EndsIn(2)));                   // 2 months left  -> RED

	assetManager.AddAsset(new Computer(3, AssetType.Laptop, "Asus", "ZenBook", OfficeLocation.USA,
		1500m, EndsIn(4)));                   // 4 months left  -> YELLOW

	assetManager.AddAsset(new Computer(4, AssetType.Desktop, "Dell", "OptiPlex 7080", OfficeLocation.Sweden,
		1500m, today.AddYears(-1)));         // 2 years left   -> blank

	assetManager.AddAsset(new MobilePhone(5, "Samsung", "Galaxy S23", OfficeLocation.USA,
		1000m, today.AddMonths(-6)));         // 2.5 years left -> blank

	assetManager.AddAsset(new Tablet(6, "Apple", "iPad Pro", OfficeLocation.Turkey,
		1200m, today.AddYears(-2).AddMonths(-3))); // 9 months left  -> blank
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
