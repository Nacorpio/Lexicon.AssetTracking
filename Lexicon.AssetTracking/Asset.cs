namespace Lexicon.AssetTracking;

public abstract class Asset
{
	public const int RedStatusMonths = 3;
	public const int YellowStatusMonths = 6;

	public Asset(
		AssetType type, 
		string brand, string model, 
		OfficeLocation office, 
		decimal priceUsd,
		DateTime purchaseDate )
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(brand);
		ArgumentException.ThrowIfNullOrWhiteSpace(model);

		ArgumentOutOfRangeException.ThrowIfNegative(priceUsd);

		Type = type;
		Brand = brand;
		Model = model;
		Office = office;
		PriceUsd = priceUsd;
		PurchaseDate = purchaseDate;
	}

	public AssetType Type { get; init; }

	public string Brand { get; }
	public string Model { get; }

	public DateTime PurchaseDate { get; init; }

	public decimal PriceLocal => CurrencyConverter
		.ConvertFromUsd(PriceUsd, Office);

	public decimal PriceUsd { get; init; }

	public OfficeLocation Office { get; }

	public DateTime EndOfLife => PurchaseDate.AddYears(3);

	public AssetStatus GetStatus(DateTime currentDate)
	{
		var redFrom = EndOfLife.AddMonths(-RedStatusMonths);
		var yellowFrom = EndOfLife.AddMonths(-YellowStatusMonths);

		if (currentDate >= EndOfLife || currentDate >= redFrom)
			return AssetStatus.Red;
		else if (currentDate >= yellowFrom)
			return AssetStatus.Yellow;
		else
			return AssetStatus.None;
	}
}
