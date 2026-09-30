namespace SoccerManager.Importer.Dataset;

/// <summary>
/// One in-scope player's latest recorded market valuation.
/// </summary>
/// <param name="PlayerTransfermarktId">The Transfermarkt player id.</param>
/// <param name="MarketValueInEur">The market value in EUR recorded on the latest dated valuation row.</param>
public sealed record PlayerValuation(int PlayerTransfermarktId, decimal MarketValueInEur);
