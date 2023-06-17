namespace CleanArchTemplate.Client.Services.Crypto;

public interface ICryptoService
{
    /// <summary>
    /// Converts plaintext to a ciphertext.
    /// </summary>
    /// <param name="text"></param>
    /// <returns>An object with ciphertext, origin and secret data.</returns>
    public Task<CryptoResult> EncryptAsync(string text);
    /// <summary>
    /// Converts an object to a ciphertext.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>An object with ciphertext, origin and secret data.</returns>
    public Task<CryptoResult> EncryptAsync(object obj);
    /// <summary>
    /// Converts a list of plaintext to a list of ciphertext.
    /// </summary>
    /// <param name="list"></param>
    /// <returns>A list of object with ciphertext, origin and secret data.</returns>
    public Task<List<CryptoResult>> EncryptListAsync(List<string> list);
    /// <summary>
    /// Converts a list of object to a list of ciphertext.
    /// </summary>
    /// <param name="list"></param>
    /// <returns>A list of object with ciphertext, origin and secret data.</returns>
    public Task<List<CryptoResult>> EncryptListAsync<T>(List<T> list);
    /// <summary>
    /// Converts a ciphertext to plaintext. Note: require key in program.cs
    /// </summary>
    /// <param name="text"></param>
    /// <returns>A decoded plaintext.</returns>
    public Task<string> DecryptAsync(string text);
    /// <summary>
    /// Converts a ciphertext to object. Note: require key in program.cs
    /// </summary>
    /// <param name="text"></param>
    /// <returns>A decoded object.</returns>
    public Task<T> DecryptAsync<T>(string text);
    /// <summary>
    /// Converts a ciphertext with a specific key to plaintext.
    /// </summary>
    /// <param name="text"></param>
    /// <returns>A decoded plaintext.</returns>
    public Task<string> DecryptAsync(CryptoInput input);
    /// <summary>
    /// Converts a ciphertext with a specific key to object.
    /// </summary>
    /// <param name="text"></param>
    /// <returns>A decoded object.</returns>
    public Task<T> DecryptAsync<T>(CryptoInput input);
    /// <summary>
    /// Converts a list of ciphertext to a list of plaintext. Note: require key in program.cs
    /// </summary>
    /// <param name="text"></param>
    /// <returns>A list of decoded plaintext.</returns>
    public Task<List<string>> DecryptListAsync(List<string> list);
    /// <summary>
    /// Converts a list of ciphertext to a list of plaintext. Note: require key in program.cs
    /// </summary>
    /// <param name="text"></param>
    /// <returns>A list of decoded object.</returns>
    public Task<List<T>> DecryptListAsync<T>(List<string> list);
    /// <summary>
    /// Converts a list ciphertext with a specific key to a list of plaintext.
    /// </summary>
    /// <param name="text"></param>
    /// <returns>A list of decoded plaintext.</returns>
    public Task<List<string>> DecryptListAsync(List<CryptoInput> list);
    /// <summary>
    /// Converts a list ciphertext with a specific key to a list of object.
    /// </summary>
    /// <param name="text"></param>
    /// <returns>A list of decoded object.</returns>
    public Task<List<T>> DecryptListAsync<T>(List<CryptoInput> list);
}
