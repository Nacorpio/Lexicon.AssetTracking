namespace Lexicon.AssetTracking;

public class MobilePhone : Asset
{
	public MobilePhone(
		string brand, 
		string model,
		OfficeLocation office, 
		decimal priceUsd, 
		DateTime purchaseDate) :
		base(AssetType.Phone, brand, model, office, priceUsd, purchaseDate)
	{
	}

	public override string GetDescription() => $"Phone: {Brand} {Model}";
}
