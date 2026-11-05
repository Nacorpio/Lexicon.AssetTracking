namespace Lexicon.AssetTracking;

public class MobilePhone : Asset
{
	public MobilePhone(
		int id,
		string brand, 
		string model,
		OfficeLocation office, 
		decimal priceUsd, 
		DateTime purchaseDate) :
		base(id, AssetType.Phone, brand, model, office, priceUsd, purchaseDate)
	{
	}

	public override string GetDescription() => $"Phone: {Brand} {Model}";
}
