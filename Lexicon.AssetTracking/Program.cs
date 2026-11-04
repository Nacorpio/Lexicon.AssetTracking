using Lexicon.AssetTracking;

PrintAssets(GetSampleAssets(DateTime.Now));

static void PrintAssets(IReadOnlyList<Asset> assets)
{
	Console.WriteLine("ASSET LIST");

	string header = $"{"Type",-10} | {"Brand",-10} | {"Model",-20} | {"Office",-15} | {"Price Local",10} | {"Price USD",10} | {"Purchase Date"}";

	Console.WriteLine(new string('-', header.Length));
	Console.WriteLine(header);
	Console.WriteLine(new string('-', header.Length));

	foreach (var asset in assets)
	{
		var status = asset.GetStatus(DateTime.Now);
		
		Console.ForegroundColor = GetStatusColor(status);

		Console.WriteLine($"{asset.Type,-10} | {asset.Brand,-10} | {asset.Model,-20} | {asset.Office,-15} | " +
			$"{asset.PriceLocal,10} | {asset.PriceUsd,10} | {asset.PurchaseDate:yyyy-MM-dd}");

		Console.ResetColor();
	}
}

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