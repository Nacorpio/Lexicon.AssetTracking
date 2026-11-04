using Lexicon.AssetTracking;

public record ColumnDefinition(
	string Header,
	ColumnAlignment Alignment,
	Func<Asset, string> ValueSelector
);