namespace CleanArchTemplate.Domain.Accounting;
public abstract record AccountType
{
    public const string Checking = nameof(Checking);
    public const string Savings = nameof(Savings);
    public const string MoneyMarket = nameof(MoneyMarket);
    public const string CertificateOfDeposit = nameof(CertificateOfDeposit);

    public string Type { get; }
    public AccountType(){}
    protected AccountType(string type)
    {
        
        Type = type;
    }

    public static IEnumerable<AccountType> Supported
    {
        get
        {
            yield return new CheckingAccountType();
            yield return new SavingsAccountType();
            yield return new MoneyMarketAccountType();
            yield return new CertificateOfDepositAccountType();
        }
    }

    public static AccountType Create(string type)
    {
        var found = Supported.SingleOrDefault(x => x.Type == type);
        if(found==null) throw new ArgumentException($"Invalid account type: {type}");
        return found;
    }
}

public record CheckingAccountType() : AccountType(AccountType.Checking);
public record SavingsAccountType() : AccountType(AccountType.Savings);
public record MoneyMarketAccountType() : AccountType(AccountType.MoneyMarket);
public record CertificateOfDepositAccountType() : AccountType(AccountType.CertificateOfDeposit);
