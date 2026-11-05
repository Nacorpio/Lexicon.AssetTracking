namespace Lexicon.AssetTracking;

public static class InputHelper 
{
	public static string ReadNonEmptyString(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);

			var input = Console.ReadLine();

			if (!string.IsNullOrWhiteSpace(input))
				return input;
		}
	}

	public static decimal ReadDecimal(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);

			var input = Console.ReadLine();

			if (decimal.TryParse(input, out var value))
				return value;
		}
	}

	public static TEnum ReadEnum<TEnum>(string prompt) where TEnum : struct, Enum
	{
		while (true)
		{
			Console.Write(prompt);

			var input = Console.ReadLine();

			if (Enum.TryParse<TEnum>(input, true, out var value))
				return value;
		}
	}

	
}
