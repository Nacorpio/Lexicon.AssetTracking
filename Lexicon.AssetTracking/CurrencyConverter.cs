namespace Lexicon.AssetTracking;

public static class CurrencyConverter
{
	private static readonly Dictionary<string, decimal> _usdExchangeRates = new()
	{
		{ "SEK", 0.10m },
		{ "USD", 1.00m },
		{ "TRY", 0.053m }
	};

	private static readonly Dictionary<OfficeLocation, string> _officeLocationCurrencies = new()
	{
		{ OfficeLocation.Sweden, "SEK" },
		{ OfficeLocation.USA, "USD" },
		{ OfficeLocation.Turkey, "TRY" }
	};

	public static string GetCurrencyCode(OfficeLocation officeLocation)
	{
		if (!_officeLocationCurrencies.TryGetValue(officeLocation, out var currencyCode))
			throw new ArgumentException($"Unsupported office location: {officeLocation}", nameof(officeLocation));

		return currencyCode;
	}

	public static decimal GetExchangeRate(string currencyCode)
	{
		if (!_usdExchangeRates.TryGetValue(currencyCode, out var exchangeRate))
			throw new ArgumentException($"Unsupported currency code: {currencyCode}", nameof(currencyCode));

		return exchangeRate;
	}

	public static decimal ConvertFromUsd(decimal amountInUsd, OfficeLocation officeLocation)
	{
		var currencyCode = GetCurrencyCode(officeLocation);
		var exchangeRate = GetExchangeRate(currencyCode);

		if (exchangeRate == 0)
			throw new InvalidOperationException($"Exchange rate for currency code '{currencyCode}' is zero, cannot convert.");

		return amountInUsd / exchangeRate;
	}
}
