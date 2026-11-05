namespace Lexicon.AssetTracking;

public class Computer : Asset
{
	public Computer(
		AssetType type, 
		string brand,
		string model, 
		OfficeLocation office, 
		decimal priceUsd, 
		DateTime purchaseDate) :
		base(type, brand, model, office, priceUsd, purchaseDate)
	{
		if (type is not (AssetType.Laptop or AssetType.Desktop))
			throw new ArgumentException("Invalid asset type for `Computer`.", nameof(type));
	}

	public override string GetDescription() => $"Computer: {Brand} {Model}";
}
