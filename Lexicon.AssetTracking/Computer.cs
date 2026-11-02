namespace Lexicon.AssetTracking
{
	internal class Computer : Asset
	{
		public Computer(
			AssetType type, 
			string brand,
			string model, 
			string office, 
			decimal priceLocal,
			decimal priceUsd, 
			DateTime purchaseDate) :
			base(type, brand, model, office, priceLocal, priceUsd, purchaseDate)
		{
			if (type is not (AssetType.Laptop or AssetType.Desktop))
				throw new ArgumentException("Invalid asset type for `Computer`.", nameof(type));
		}
	}
}
