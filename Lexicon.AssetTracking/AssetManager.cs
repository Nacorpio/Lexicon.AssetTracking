namespace Lexicon.AssetTracking;

public class AssetManager 
{
	private readonly List<Asset> _assets = [];

	public void AddAsset(Asset asset)
	{
		_assets.Add(asset);
	}

	public IReadOnlyList<Asset> GetAssets() => _assets;


	public IReadOnlyList<Asset> GetAssetsByOffice(OfficeLocation office) =>
		[.. _assets.Where(a => a.Office == office)];

	public IReadOnlyList<Asset> GetAssetsByType(AssetType type) =>
		[.. _assets.Where(a => a.Type == type)];


	public IReadOnlyList<Asset> GetSortedAssets() =>
		[.. _assets.OrderBy(a => a.Type).ThenBy(a => a.PurchaseDate)];
}
