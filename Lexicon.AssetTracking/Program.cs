using Lexicon.AssetTracking;

var assetManager = new AssetManager();
var today = DateTime.Today;

AddSampleAssets(assetManager, today);
PrintAssets(assetManager, today);

static void AddSampleAssets(AssetManager assetManager, DateTime today)
{
	// Purchased exactly 3 years ago + N months => end of life is N months from today.
	DateTime EndsIn(int months) => today.AddYears(-3).AddMonths(months);

	assetManager.AddAsset(new Computer(AssetType.Laptop, "Apple", "MacBook Pro", OfficeLocation.Sweden,
		2500m, EndsIn(-2)));                 // expired        -> RED

	assetManager.AddAsset(new MobilePhone("Nokia", "XR20", OfficeLocation.Turkey,
		800m, EndsIn(2)));                   // 2 months left  -> RED

	assetManager.AddAsset(new Computer(AssetType.Laptop, "Asus", "ZenBook", OfficeLocation.USA,
		1500m, EndsIn(4)));                   // 4 months left  -> YELLOW

	assetManager.AddAsset(new Computer(AssetType.Desktop, "Dell", "OptiPlex 7080", OfficeLocation.Sweden,
		1500m, today.AddYears(-1)));         // 2 years left   -> blank

	assetManager.AddAsset(new MobilePhone("Samsung", "Galaxy S23", OfficeLocation.USA,
		1000m, today.AddMonths(-6)));         // 2.5 years left -> blank
}

static void PrintAssets(AssetManager assetManager, DateTime today)
{
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

	foreach (var columnDef in columnDefs)
	{
		var maxLength = GetMaxLength(assets, columnDef.Header, columnDef.ValueSelector);

		var header = columnDef.Alignment switch
		{
			ColumnAlignment.Left => columnDef.Header.PadRight(maxLength),
			ColumnAlignment.Right => columnDef.Header.PadLeft(maxLength),

			_ => throw new InvalidOperationException($"Unknown alignment: {columnDef.Alignment}")
		};

		ConsoleHelper.Print(header);
		ConsoleHelper.Print(" | ");
	}

	ConsoleHelper.PrintLine();
	ConsoleHelper.PrintLine(new string('-', columnDefs.Sum(cd => GetMaxLength(assets, cd.Header, cd.ValueSelector) + 3)));

	foreach (var asset in assets)
	{
		foreach (var columnDef in columnDefs)
		{
			var maxLength = GetMaxLength(assets, columnDef.Header, columnDef.ValueSelector);
			var value = columnDef.ValueSelector(asset);

			var formattedValue = columnDef.Alignment switch
			{
				ColumnAlignment.Left => value.PadRight(maxLength),
				ColumnAlignment.Right => value.PadLeft(maxLength),
				_ => throw new InvalidOperationException($"Unknown alignment: {columnDef.Alignment}")
			};

			var status = asset.GetStatus(today);
			var color = GetStatusColor(status);

			ConsoleHelper.Print(formattedValue, color);
			ConsoleHelper.Print(" | ");
		}

		ConsoleHelper.PrintLine();
	}
}


static string FormatPriceLocal(Asset asset) =>
	$"{asset.PriceLocal:F2} {CurrencyConverter.GetCurrencyCode(asset.Office)}";


static int GetMaxLength<T>(IEnumerable<T> items, string column, Func<T, string> selector) =>
	Math.Max(column.Length, items.Max(item => selector(item).Length));

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
