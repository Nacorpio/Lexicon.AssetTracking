# Lexicon.AssetTracking

A C# console application that keeps track of a company's assets (laptops, desktops and mobile phones) across offices in different countries. It shows a sorted table of all assets, their price in the office's local currency, and warns when an asset is close to the end of its lifespan.

This is the weekly mini project "Asset Tracking System #1" on the Lexicon Full Stack C# .NET course.

## Features

- Asset classes `Computer` (laptop or desktop) and `MobilePhone`, built on a shared `Asset` base class
- Assets are stored in an `AssetManager` (a `List<Asset>` internally)
- Table output sorted by office, then by purchase date
- End-of-life status for every asset, shown as text and in colour
- Offices in Sweden, USA and Turkey, each with its own currency
- Prices are entered in USD and converted to the office's local currency
- Input validation in the constructors (empty brand or model, negative price)

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

## Business rules

- **Lifespan:** every asset lasts 3 years from its purchase date.
- **Status colours:**
  - **RED:** less than 3 months remaining, and also assets that are already past end of life.
  - **YELLOW:** less than 6 months remaining (and 3 months or more).
  - No status: 6 months or more remaining.

  The assignment text says YELLOW for under 3 months and RED for under 6 months. Those two ranges overlap, so the colours are swapped here (RED is the more urgent one). The course allows this inversion.
- **Sorting:** by office name (alphabetical, as in the assignment's example output), then by purchase date, oldest first.
- **"Today":** the current date is read once with `DateTime.Today` and passed in, so every status in one run uses the same date.

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

## Data

Nothing is saved. The sample assets are created in code every time the program starts. The sample purchase dates are relative to today, so all statuses (RED, YELLOW and none) always appear.
