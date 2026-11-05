namespace Lexicon.AssetTracking;

public class AssetManager 
{
	private readonly List<Asset> _assets = [];

	public int GetNextId() => _assets.Count == 0 ? 1 : _assets.Max(a => a.Id) + 1;

	public Asset AddAsset(
		AssetType type,
		string brand,
		string model,
		OfficeLocation office,
		decimal priceUsd,
		DateTime purchaseDate)
	{
		var id = GetNextId();

		Asset asset = type switch
		{
			AssetType.Laptop or AssetType.Desktop => new Computer(id, type, brand, model, office, priceUsd, purchaseDate),
			AssetType.Phone => new MobilePhone(id, brand, model, office, priceUsd, purchaseDate),
			AssetType.Tablet => new Tablet(id, brand, model, office, priceUsd, purchaseDate),

			_ => throw new ArgumentException("Invalid asset type.", nameof(type))
		};

		AddAsset(asset);

		return asset;
	}

	public void AddAsset(Asset asset)
	{
		_assets.Add(asset);
	}

	public IReadOnlyList<Asset> GetAssets() => _assets;


	public IReadOnlyList<Asset> GetAssetsByOffice(OfficeLocation office) =>
		[.. _assets.Where(a => a.Office == office)];

	public IReadOnlyList<Asset> GetAssetsByType(AssetType type) =>
		[.. _assets.Where(a => a.Type == type)];


	public IReadOnlyList<Asset> GetSortedAssets() => [.. _assets
		.OrderBy(a => a.Office.ToString())
		.ThenBy(a => a.PurchaseDate)];
}
