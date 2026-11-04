namespace Lexicon.AssetTracking;

internal class AssetManager 
{
	private readonly List<Asset> _assets = [];

	public void AddAsset(Asset asset)
	{
		_assets.Add(asset);
	}

	public IReadOnlyList<Asset> GetAssets() => _assets;


	public IReadOnlyList<Asset> GetAssetsByOffice(string office) =>
		[.. _assets.Where(a => a.Office.Equals(office, StringComparison.OrdinalIgnoreCase))];

	public IReadOnlyList<Asset> GetAssetsByType(AssetType type) =>
		[.. _assets.Where(a => a.Type == type)];


	public IReadOnlyList<Asset> GetSortedAssets() =>
		[.. _assets.OrderBy(a => a.Type).ThenBy(a => a.PurchaseDate)];
}
