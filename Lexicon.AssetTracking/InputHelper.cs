using System.Globalization;

namespace Lexicon.AssetTracking;

public static class InputHelper 
{
	public static T ReadInput<T>(string prompt, Func<string, (bool isValid, T value)> parseFunc, string? errorMessage = null)
	{
		while (true)
		{
			Console.Write(prompt);

			var input = Console.ReadLine() ?? throw new InvalidOperationException("Input cannot be null.");

			var (isValid, value) = parseFunc(input);

			if (isValid)
				return value;

			Console.WriteLine(errorMessage ?? "Invalid input. Please try again.");
		}
	}

	public static string ReadNonEmptyString(string prompt, string? errorMessage = null)
	{
		return ReadInput<string>(prompt, input =>
			{
				var isValid = !string.IsNullOrWhiteSpace(input);
				return (isValid, input);
			}, errorMessage);
	}

	public static DateTime ReadDate(string prompt, string? errorMessage = null)
	{
		return ReadInput<DateTime>(prompt, input =>
		{
			var isValid = DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var value);
			return (isValid, value);
		}, errorMessage);
	}

	public static decimal ReadDecimal(string prompt, string? errorMessage = null)
	{
		return ReadInput<decimal>(prompt, input =>
		{
			var isValid = decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var value);
			return (isValid, value);
		}, errorMessage);
	}

	public static TEnum ReadEnum<TEnum>(string prompt, string? errorMessage = null) where TEnum : struct, Enum
	{
		return ReadInput<TEnum>(prompt, input =>
		{
			var isValid = Enum.TryParse<TEnum>(input, true, out var value);
			return (isValid, value);
		}, errorMessage);
	}
}
