namespace Lexicon.AssetTracking;

public class Tablet : Asset
{
	public Tablet(
		int id,
		string brand,
		string model,
		OfficeLocation office,
		decimal priceUsd,
		DateTime purchaseDate) :
		base(id, AssetType.Tablet, brand, model, office, priceUsd, purchaseDate)
	{
	}

	public override string GetDescription() => $"Tablet: {Brand} {Model}";
}