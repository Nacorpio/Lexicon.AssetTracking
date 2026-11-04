namespace Lexicon.AssetTracking
{
	public class MobilePhone : Asset
	{
		public MobilePhone(
			string brand, 
			string model,
			string office, 
			decimal priceLocal, 
			decimal priceUsd, 
			DateTime purchaseDate) :
			base(AssetType.Phone, brand, model, office, priceLocal, priceUsd, purchaseDate)
		{
		}
	}
}
