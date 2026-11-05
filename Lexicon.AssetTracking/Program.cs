using Lexicon.AssetTracking;

var assetManager = new AssetManager();
var today = DateTime.Today;

AddSampleAssets(assetManager, today);

try 
{
	while (true)
	{
		Console.Clear();

		ConsoleHelper.PrintLine("ASSET TRACKING SYSTEM", ConsoleColor.Green);
		ConsoleHelper.PrintLine();

		ConsoleHelper.PrintMenu();
		ConsoleHelper.PrintLine();

		if (!InputHelper.TryReadEnum<MenuOption>("Select an option: ", out var option))
		{
			ConsoleHelper.PrintLine("Invalid option. Please try again.", ConsoleColor.Red);
			Console.ReadLine();
			
			continue;
		}

		Console.Clear();

		switch (option)
		{
			case MenuOption.ViewAssets:
				MenuHandlers.HandleViewAssets(assetManager, DateTime.Today);
				break;

			case MenuOption.Exit:
				ConsoleHelper.PrintLine("Exiting the application. Goodbye!");
				return;

			default:
				ConsoleHelper.PrintLine("Invalid option. Please try again.", ConsoleColor.Red);
				break;
		}

		ConsoleHelper.PrintLine("Press ENTER to return to the main menu...");
		Console.ReadLine();
	}
} 
catch (EndOfStreamException)
{
	ConsoleHelper.PrintLine("Input stream closed. Exiting the application.", ConsoleColor.Red);
}
catch (Exception ex)
{
	ConsoleHelper.PrintLine($"An unexpected error occurred: {ex.Message}", ConsoleColor.Red);
}


static void AddSampleAssets(AssetManager assetManager, DateTime today)
{
	// Purchased exactly 3 years ago + N months => end of life is N months from today.
	DateTime EndsIn(int months) => today.AddYears(-3).AddMonths(months);

	assetManager.AddAsset(AssetType.Laptop, "Dell", "XPS 13", OfficeLocation.Sweden, 1200m, EndsIn(0));
	assetManager.AddAsset(AssetType.Phone, "Apple", "iPhone 12", OfficeLocation.Turkey, 800m, EndsIn(6));
	assetManager.AddAsset(AssetType.Tablet, "Samsung", "Galaxy Tab S7", OfficeLocation.USA, 650m, EndsIn(3));
}