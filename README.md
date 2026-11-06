# Lexicon.AssetTracking

A C# console application that keeps track of a company's assets (laptops, desktops, mobile phones and tablets) across offices in different countries. It shows a table of all assets with their price in the office's local currency and their age, and warns when an asset is close to the end of its lifespan.

This is the weekly mini project "Asset Tracking System #1" on the Lexicon Full Stack C# .NET course (Levels 1 to 4, plus the optional search and remove features).

## Features

- Menu-driven console application: add, view, sort, search and remove assets
- Abstract `Asset` base class with `Computer` (laptop or desktop), `MobilePhone` and `Tablet`; each overrides `GetDescription()` (polymorphism)
- Every asset has a unique ID, assigned automatically
- Age of every asset, shown in the table
- Assets are stored in an `AssetManager` (a `List<Asset>` internally)
- End-of-life status for every asset, shown as text and in colour
- Offices in Sweden, USA and Turkey, each with its own currency
- Prices are entered in USD and converted to the office's local currency
- Input validation: every prompt repeats until the input is valid; constructors also reject invalid objects (empty brand or model, negative price)

## Technologies

- C#
- .NET 10
- Console application

## How to run

From the repository root:

```
dotnet run --project Lexicon.AssetTracking
```

Or open `Lexicon.AssetTracking.slnx` in Visual Studio and press F5.

## Menu

| Option | What it does |
|---|---|
| 1. Add asset | Asks for type, brand, model, office, price (USD) and purchase date, then adds the asset with a new ID |
| 2. View assets | Shows all assets sorted by office, then purchase date |
| 3. Sort assets | Shows all assets sorted by office then date, type then date, or purchase date only |
| 4. Search assets | Finds assets whose brand or model contains the search text (not case sensitive) |
| 5. Remove asset | Removes an asset by ID after a yes/no confirmation |
| 6. Exit | Closes the program |

Ctrl+Z (end of input) at any prompt closes the program cleanly.

## Input rules

- Brand and model cannot be empty.
- Price must be a number of 0 or more (dot as decimal separator).
- Purchase date must be written `yyyy-MM-dd` and cannot be in the future.
- Type, office and sort/search options must be one of the listed values.

## Business rules

- **Lifespan:** every asset lasts 3 years from its purchase date.
- **Age:** shown in years and months, calculated from the purchase date and today.
- **Status colours:**
  - **RED:** less than 3 months remaining, and also assets that are already past end of life.
  - **YELLOW:** less than 6 months remaining (and 3 months or more).
  - No status: 6 months or more remaining.

  The assignment text says YELLOW for under 3 months and RED for under 6 months. Those two ranges overlap, so the colours are swapped here (RED is the more urgent one). The course allows this inversion.
- **Sorting:** the default view sorts by office name (alphabetical, as in the assignment's example output), then by purchase date, oldest first. The Sort menu also offers type then date, and date only.
- **IDs:** a new asset gets the highest existing ID plus one (the first one gets 1). If the asset with the highest ID is removed, that ID is given to the next asset that is added. IDs of the remaining assets never change.
- **"Today":** the current date is read once in `Program.cs` and passed to everything that needs it, so every status, age and date check in one run uses the same date.

## Currencies

| Office | Currency |
|---|---|
| Sweden | SEK |
| USA | USD |
| Turkey | TRY |

Exchange rates are hardcoded in `CurrencyConverter`, stored as the value of one unit of the currency in USD:

| Currency | USD per 1 unit |
|---|---|
| SEK | 0.10 |
| USD | 1.00 |
| TRY | 0.053 |

The local price is calculated from the USD price (`PriceUsd / rate`), so the two prices can never disagree. The rates are approximate sample values, not live rates.

## Design notes

- `Asset` is abstract; `AssetManager` creates the right subclass from the chosen `AssetType`.
- `InputHelper.ReadInput<T>` is the one generic read-and-validate loop. The other `Read*` helpers only supply a parse function.
- The table is described once as a list of `ColumnDefinition`s (header, alignment, value selector) and printed by one `PrintTable` method that is shared by View, Sort and Search.

## Data

Nothing is saved. Four sample assets are created in code every time the program starts. Their purchase dates are relative to today, so RED and YELLOW statuses always appear. Saving to a file is Level 5 (optional) and is not implemented.
