namespace ClamJam.Domain.DomainModels;

public readonly record struct Percentage
{
    public decimal Value { get; }
    public decimal Factor => Value / 100m;

    public Percentage(decimal value)
    {
        if (value < 0 || value > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "Percentage must be between 0 and 100");
        
        Value = value;
    }

    public static implicit operator decimal(Percentage percentage) => percentage.Value;
    public static implicit operator Percentage(decimal value) => new(value);
    
    public override string ToString() => $"{Value}%";
}