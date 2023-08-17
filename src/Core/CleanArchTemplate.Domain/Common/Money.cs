using System.Numerics;

namespace CleanArchTemplate.Domain.Common;

public record Money : IComparable<Money>
{
    public decimal Amount { get; }
    public Currency Currency { get; }
    public static Money Zero { get; } = new(0, Currency.Empty);
    private bool IsZero => Amount == 0 && Currency == Currency.Empty;

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0) throw new InvalidOperationException("Money amount cannot be negative");
        Amount = Math.Round(amount, 2);
        Currency = currency;
    }

    public Money Add(Money other) =>
        other.IsZero ? this
        : IsZero ? other
        : Currency == other.Currency ? new Money(Amount + other.Amount, Currency)
        : throw new InvalidOperationException("Cannot add different currencies");
    
    public Money Subtract(Money other) =>
        other.IsZero ? this
        : IsZero ? throw new InvalidOperationException("Cannot subtract from zero")
        : other.Currency != Currency ? throw new InvalidOperationException("Cannot subtract different currencies")
        : other.Amount > Amount ? throw new InvalidOperationException("Not enough funds")
        : other.Amount == Amount ? Zero
        : new Money(Amount - other.Amount, Currency);

    public Money Scale(decimal factor) =>
        factor < 0 ? throw new InvalidOperationException("Cannot multiply by a negative factor")
        : new Money(Amount * factor, Currency);

    public Money Divide(decimal  factor) =>
        factor < 0 ? throw new InvalidOperationException("Cannot divide by a negative factor")
        : factor == 0 ? throw new InvalidOperationException("Cannot divide by zero")
        : new Money(Amount / factor, Currency);
    
    public int CompareTo(Money? other)
        => other is null ? 1
            : other.IsZero || IsZero ? Amount.CompareTo(other.Amount)
            : Currency == other.Currency ? Amount.CompareTo(other.Amount)
            : throw new InvalidOperationException("Cannot compare different currencies");

    public static Money operator +(Money left, Money right) => left.Add(right);
    public static Money operator -(Money left, Money right) => left.Subtract(right);
    public static Money operator *(Money left, decimal right) => left.Scale(right);
    public static Money operator /(decimal left, Money right) => right.Divide(left);
    
    public override string ToString() => IsZero ? "0.00": $"{Amount:0:00} {Currency.Symbol}";
    public static bool TryParse(string value, out Money money)
    {
        money = Zero;
        try
        {
            var arr = value.Split(" ");
            var amountStr = arr.First();
            if (!decimal.TryParse(amountStr, out var amount)) return false;
            money = new Money(amount, arr.Length == 2 ? new Currency(arr.Last()) : Currency.Empty);
            return true;
        }
        catch
        {
            return false;
        }
      
    }

    public static Money Parser(string value)
    {
        if (TryParse(value, out var money))
        {
            return money;
        }
        throw new ArgumentException("Invalid Money format");
    }
}