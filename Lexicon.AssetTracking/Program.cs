using Lexicon.AssetTracking;

PrintAssets(GetSampleAssets(DateTime.Now));

static void PrintAssets(IReadOnlyList<Asset> assets)
{
	Console.WriteLine("ASSET LIST");

	var maxTypeLength = GetMaxLength(assets, "Type", a => a.Type.ToString());
	var maxBrandLength = GetMaxLength(assets, "Brand", a => a.Brand);
	var maxModelLength = GetMaxLength(assets, "Model", a => a.Model);
	var maxOfficeLength = GetMaxLength(assets, "Office", a => a.Office);

	var maxPriceLocalLength = GetMaxLength(assets, "Price Local", a => a.PriceLocal.ToString());
	var maxPriceUsdLength = GetMaxLength(assets, "Price USD", a => a.PriceUsd.ToString());

	var maxPurchaseDateLength = GetMaxLength(assets, "Purchase Date", a => a.PurchaseDate.ToString("yyyy-MM-dd"));

	string header = 
		$"{"Type".PadRight(maxTypeLength)} | " +
		$"{"Brand".PadRight(maxBrandLength)} | " +
		$"{"Model".PadRight(maxModelLength)} | " +
		$"{"Office".PadRight(maxOfficeLength)} | " +
		$"{"Price Local".PadRight(maxPriceLocalLength)} | " +
		$"{"Price USD".PadRight(maxPriceUsdLength)} | " +
		$"{"Purchase Date".PadRight(maxPurchaseDateLength)}";

	Console.WriteLine(new string('-', header.Length));
	Console.WriteLine(header);
	Console.WriteLine(new string('-', header.Length));

	foreach (var asset in assets)
	{
		var status = asset.GetStatus(DateTime.Now);
		
		Console.ForegroundColor = GetStatusColor(status);

		Console.WriteLine(
			$"{asset.Type.ToString().PadRight(maxTypeLength)} | " +
			$"{asset.Brand.PadRight(maxBrandLength)} | " +
			$"{asset.Model.PadRight(maxModelLength)} | " +
			$"{asset.Office.PadRight(maxOfficeLength)} | " +
			$"{asset.PriceLocal.ToString().PadRight(maxPriceLocalLength)} | " +
			$"{asset.PriceUsd.ToString().PadRight(maxPriceUsdLength)} | " +
			$"{asset.PurchaseDate.ToString("yyyy-MM-dd").PadRight(maxPurchaseDateLength)}");

		Console.ResetColor();
	}
}

static int GetMaxLength<T>(IEnumerable<T> items, string column, Func<T, string> selector) =>
	Math.Max(column.Length, items.Max(item => selector(item).Length));

static ConsoleColor GetStatusColor(AssetStatus status) =>
	status switch
	{
		AssetStatus.Red => ConsoleColor.Red,
		AssetStatus.Yellow => ConsoleColor.Yellow,

		_ => ConsoleColor.White
	};

static IReadOnlyList<Asset> GetSampleAssets(DateTime today)
{
	// Purchased exactly 3 years ago + N months => end of life is N months from today.
	DateTime EndsIn(int months) => today.AddYears(-3).AddMonths(months);

	return
	[
		new Computer(AssetType.Laptop, "Apple", "MacBook Pro", "Sweden",
			25000m, 2500m, EndsIn(-2)),            // expired      -> RED

		new MobilePhone("Nokia", "XR20", "Turkey",
			9000m, 800m, EndsIn(2)),               // 2 months left -> RED

		new Computer(AssetType.Laptop, "Asus", "ZenBook", "USA",
			15000m, 1500m, EndsIn(4)),             // 4 months left -> YELLOW

		new Computer(AssetType.Desktop, "Dell", "OptiPlex 7080", "Sweden",
			15000m, 1500m, today.AddYears(-1)),    // 2 years left  -> blank

		new MobilePhone("Samsung", "Galaxy S23", "Sweden",
			10000m, 1000m, today.AddMonths(-6))    // 2.5 years left -> blank
	];
}