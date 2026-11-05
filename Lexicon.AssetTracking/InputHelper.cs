using System.Globalization;

namespace Lexicon.AssetTracking;

public static class InputHelper 
{
	public static bool TryHandleInput<T>(string prompt, Func<string, (bool isValid, T? value)> parseFunc, out T? result)
	{
		result = default;
		Console.Write(prompt);

		var input = Console.ReadLine() ?? throw new EndOfStreamException("Input cannot be null.");

		var (isValid, value) = parseFunc(input);

		if (isValid)
			result = value;

		return isValid;
	}

	public static bool TryReadNonEmptyString(string prompt, out string? result)
	{
		return TryHandleInput<string>(prompt, input =>
		{
			var isValid = !string.IsNullOrWhiteSpace(input);
			return (isValid, isValid ? input : default);
		},
		out result);
	}

	public static bool TryReadDecimal(string prompt, out decimal result)
	{
		return TryHandleInput<decimal>(prompt, input =>
		{
			var isValid = decimal.TryParse(input, CultureInfo.InvariantCulture, out var value) && value >= 0;

			return (isValid, isValid ? value : default);
		},
		out result);
	}

	public static bool TryReadEnum<TEnum>(string prompt, out TEnum result) where TEnum : struct, Enum
	{
		return TryHandleInput<TEnum>(prompt, input =>
		{
			var isValid = Enum.TryParse<TEnum>(input, true, out var value) && Enum.IsDefined<TEnum>(value);

			return (isValid, isValid ? value : default);
		}, 
		out result);
	}
}
