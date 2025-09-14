namespace ClamJam.Domain.DomainModels
{
    public record struct Money
    {
        public decimal Amount { get; set; }
        public EnumCurrency Currency { get; set; }

        public Money(decimal amount, EnumCurrency currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));

            Amount = Math.Round(amount, 2);
            Currency = currency;
        }

        /// <summary>
        ///  Add money
        /// </summary>
        /// <param name="other">Money to add to the current amount</param>
        /// <returns>The addition of two sums of money</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException($"Cannot add different currencies: {Currency} and {other.Currency}");

            return new Money(Amount + other.Amount, Currency);
        }


        /// <summary>
        ///  Subtract money
        /// </summary>
        /// <param name="other">Money to subtract from the current amount</param>
        /// <returns>The subtraction of two sums of money</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException($"Cannot subtract different currencies: {Currency} and {other.Currency}");

            return new Money(Amount - other.Amount, Currency);
        }

        /// <summary>
        /// Multiply money
        /// </summary>
        /// <param name="factor">Factor applied to a base value to proportionally scale it</param>
        /// <returns>Multiplied money</returns>
        /// <exception cref="ArgumentException"></exception>
        public Money Multiply(decimal factor)
        {
            if (factor < 0)
                throw new ArgumentException("Factor cannot be negative", nameof(factor));

            return new Money(Amount * factor, Currency);
        }

        public static implicit operator decimal(Money money) => money.Amount;

    }
}
