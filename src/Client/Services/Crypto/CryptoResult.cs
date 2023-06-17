namespace CleanArchTemplate.Client.Services.Crypto;

public class CryptoResult
{
    public bool Status { get; set; }
    public string Origin { get; set; }
    public string Value { get; set; }
    public Secret Secret { get; set; }

    public static implicit operator string(CryptoResult result) => result.Status ? result.Value : string.Empty;

}
