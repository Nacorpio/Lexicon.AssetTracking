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
	Console.WriteLine("ASSET LIST");

	var assets = assetManager.GetSortedAssets();

	if (assets.Count is 0)
	{
		Console.WriteLine("No assets found.");
		return;
	}

	var maxTypeLength = GetMaxLength(assets, "Type", a => a.Type.ToString());
	var maxBrandLength = GetMaxLength(assets, "Brand", a => a.Brand);
	var maxModelLength = GetMaxLength(assets, "Model", a => a.Model);
	var maxOfficeLength = GetMaxLength(assets, "Office", a => a.Office.ToString());
	var maxStatusLength = GetMaxLength(assets, "Status", a => GetStatusString(a, today));

	var maxPriceLocalLength = GetMaxLength(assets, "Price Local", a => $"{a.PriceLocal:F2} {CurrencyConverter.GetCurrencyCode(a.Office)}");
	var maxPriceUsdLength = GetMaxLength(assets, "Price USD", a => $"{a.PriceUsd:F2} USD");

	var maxPurchaseDateLength = GetMaxLength(assets, "Purchase Date", a => a.PurchaseDate.ToString("yyyy-MM-dd"));

	string header =
		$"{"Office".PadRight(maxOfficeLength)} | " +
		$"{"Type".PadRight(maxTypeLength)} | " +
		$"{"Brand".PadRight(maxBrandLength)} | " +
		$"{"Model".PadRight(maxModelLength)} | " +
		$"{"Purchase Date".PadRight(maxPurchaseDateLength)} | " +
		$"{"Price Local".PadRight(maxPriceLocalLength)} | " +
		$"{"Price USD".PadRight(maxPriceUsdLength)} | " +
		$"{"Status".PadRight(maxStatusLength)}";

	Console.WriteLine(new string('-', header.Length));
	Console.WriteLine(header);
	Console.WriteLine(new string('-', header.Length));

	foreach (var asset in assets)
	{
		var status = asset.GetStatus(today);
		var color = GetStatusColor(status);

		Print($"{asset.Office.ToString().PadRight(maxOfficeLength)}", color);
		Print(" | ");

		Print($"{asset.Type.ToString().PadRight(maxTypeLength)}", color);
		Print(" | ");

		Print($"{asset.Brand.PadRight(maxBrandLength)}", color);
		Print(" | ");

		Print($"{asset.Model.PadRight(maxModelLength)}", color);
		Print(" | ");

		PrintLine($"{asset.PurchaseDate.ToString("yyyy-MM-dd").PadRight(maxPurchaseDateLength)}", color);
		Print(" | ");

		Print($"{($"{asset.PriceLocal:F2} {CurrencyConverter.GetCurrencyCode(asset.Office)}").PadRight(maxPriceLocalLength)}", color);
		Print(" | ");

		Print($"{($"{asset.PriceUsd:F2} USD").PadRight(maxPriceUsdLength)}", color);
		Print(" | ");

		PrintLine($"{GetStatusString(asset, today).PadRight(maxStatusLength)}", color);
	}
}

static void Print(string text, ConsoleColor? color = null)
{
	if (color.HasValue)
	{
		Console.ForegroundColor = color.Value;
	}

	Console.Write(text);
	Console.ResetColor();
}

static void PrintLine(string text, ConsoleColor? color = null)
{
	if (color.HasValue)
	{
		Console.ForegroundColor = color.Value;
	}

	Console.WriteLine(text);
	Console.ResetColor();
}

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