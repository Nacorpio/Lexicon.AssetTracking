using Lexicon.AssetTracking;

PrintAssets(GetSampleAssets());

static void PrintAssets(IReadOnlyList<Asset> assets)
{
	Console.WriteLine("ASSET LIST");

	string header = $"{"Type",-10} | {"Brand",-10} | {"Model",-20} | {"Office",-15} | {"Price Local",10} | {"Price USD",10} | {"Purchase Date"}";

	Console.WriteLine(new string('-', header.Length));
	Console.WriteLine(header);
	Console.WriteLine(new string('-', header.Length));

	foreach (var asset in assets)
	{
		Console.WriteLine($"{asset.Type,-10} | {asset.Brand,-10} | {asset.Model,-20} | {asset.Office,-15} | " +
			$"{asset.PriceLocal,10} | {asset.PriceUsd,10} | {asset.PurchaseDate:yyyy-MM-dd}");
	}
}

static IReadOnlyList<Asset> GetSampleAssets()
{
	return
	[
		new Computer(
			AssetType.Laptop,
			"Lenovo",
			"ThinkPad X1 Carbon",
			"Sweden",
			25000m,
			2500m,
			new DateTime(2023, 1, 15)),
		new Computer(
			AssetType.Desktop,
			"Dell",
			"OptiPlex 7080",
			"USA",
			15000m,
			1500m,
			new DateTime(2022, 11, 5)),
		new MobilePhone(
			"Apple",
			"iPhone 14 Pro",
			"Turkey",
			12000m,
			1200m,
			new DateTime(2023, 2, 20)),
		new MobilePhone(
			"Samsung",
			"Galaxy S23",
			"Sweden",
			10000m,
			1000m,
			new DateTime(2023, 3, 10))
	];
}