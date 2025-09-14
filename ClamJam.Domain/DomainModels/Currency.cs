namespace ClamJam.Domain.DomainModels;

public enum EnumCurrency
{
    USD,
    EUR,
    GBP,
    CAD,
    AUD
}

public static class CurrencyExtensions
{
    public static string ToCode(this EnumCurrency currency) => currency switch
    {
        EnumCurrency.USD => "usd",
        EnumCurrency.EUR => "eur",
        EnumCurrency.GBP => "gbp",
        EnumCurrency.CAD => "cad",
        EnumCurrency.AUD => "aud",
        _ => throw new ArgumentOutOfRangeException(nameof(currency))
    };

    public static EnumCurrency FromCode(string code) => code.ToLowerInvariant() switch
    {
        "usd" => EnumCurrency.USD,
        "eur" => EnumCurrency.EUR,
        "gbp" => EnumCurrency.GBP,
        "cad" => EnumCurrency.CAD,
        "aud" => EnumCurrency.AUD,
        _ => throw new ArgumentException($"Unknown currency code: {code}")
    };
}