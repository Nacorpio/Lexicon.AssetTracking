using Lexicon.AssetTracking;

var assetManager = new AssetManager();

assetManager.AddAsset(new Computer(
	AssetType.Laptop,
	"Dell", "XPS 13", "New York",
	1200.00m, 1500.00m,
	new DateTime(2021, 5, 15)));

assetManager.AddAsset(new MobilePhone(
	"Apple", "iPhone 13", "Los Angeles",
	999.00m, 1200.00m,
	new DateTime(2022, 3, 10)));

assetManager.AddAsset(new Computer(
	AssetType.Desktop,
	"HP", "Pavilion", "Chicago",
	800.00m, 1000.00m,
	new DateTime(2020, 11, 5)));

PrintAssets(assetManager, DateTime.Now);

static void PrintAssets(AssetManager assetManager, DateTime today)
{
	Console.WriteLine("ASSET LIST");

	var assets = assetManager.GetSortedAssets();

	var maxTypeLength = GetMaxLength(assets, "Type", a => a.Type.ToString());
	var maxBrandLength = GetMaxLength(assets, "Brand", a => a.Brand);
	var maxModelLength = GetMaxLength(assets, "Model", a => a.Model);
	var maxOfficeLength = GetMaxLength(assets, "Office", a => a.Office);
	var maxStatusLength = GetMaxLength(assets, "Status", a => a.GetStatus(today).ToString());

	var maxPriceLocalLength = GetMaxLength(assets, "Price Local", a => a.PriceLocal.ToString());
	var maxPriceUsdLength = GetMaxLength(assets, "Price USD", a => a.PriceUsd.ToString());

	var maxPurchaseDateLength = GetMaxLength(assets, "Purchase Date", a => a.PurchaseDate.ToString("yyyy-MM-dd"));

	string header = 
		$"{"Type".PadRight(maxTypeLength)} | " +
		$"{"Brand".PadRight(maxBrandLength)} | " +
		$"{"Model".PadRight(maxModelLength)} | " +
		$"{"Office".PadRight(maxOfficeLength)} | " +
		$"{"Status".PadRight(maxStatusLength)} | " +
		$"{"Price Local".PadRight(maxPriceLocalLength)} | " +
		$"{"Price USD".PadRight(maxPriceUsdLength)} | " +
		$"{"Purchase Date".PadRight(maxPurchaseDateLength)}";

	Console.WriteLine(new string('-', header.Length));
	Console.WriteLine(header);
	Console.WriteLine(new string('-', header.Length));

	foreach (var asset in assets)
	{
		var status = asset.GetStatus(today);
		
		Console.ForegroundColor = GetStatusColor(status);

		Console.WriteLine(
			$"{asset.Type.ToString().PadRight(maxTypeLength)} | " +
			$"{asset.Brand.PadRight(maxBrandLength)} | " +
			$"{asset.Model.PadRight(maxModelLength)} | " +
			$"{asset.Office.PadRight(maxOfficeLength)} | " +
			$"{status.ToString().PadRight(maxStatusLength)} | " +
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