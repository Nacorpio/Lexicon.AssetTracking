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

	public bool RemoveAsset(int id)
	{
		var asset = _assets.FirstOrDefault(a => a.Id == id);

		if (asset is null)
			return false;

		return _assets.Remove(asset);
	}

	public bool ContainsAsset(int id) => _assets.Any(a => a.Id == id);

	public IReadOnlyList<Asset> GetAssets() => _assets;


	public IReadOnlyList<Asset> SearchAssetsByBrand(string brand) => [.. _assets
		.Where(a => a.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase))];

	public IReadOnlyList<Asset> SearchAssetsByModel(string model) => [.. _assets
		.Where(a => a.Model.Contains(model, StringComparison.OrdinalIgnoreCase))];


	public IReadOnlyList<Asset> GetAssetsSorted(SortMenuOption sortOption) => sortOption switch
	{
		SortMenuOption.ByOfficeThenByDate => _assets
			.OrderBy(a => a.Office.ToString())
			.ThenBy(a => a.PurchaseDate)
			.ToList(),

		SortMenuOption.ByTypeThenByDate => [.. _assets
			.OrderBy(a => a.Type)
			.ThenBy(a => a.PurchaseDate)],

		SortMenuOption.ByPurchaseDate => [.. _assets
			.OrderBy(a => a.PurchaseDate)],

		_ => throw new ArgumentException("Invalid sort option.", nameof(sortOption))
	};

	public IReadOnlyList<Asset> GetSortedAssets() => [.. _assets
		.OrderBy(a => a.Office.ToString())
		.ThenBy(a => a.PurchaseDate)];
}
