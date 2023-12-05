using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.Options;
using CleanArchitecture.Application.Core.Abstraction.Services;

namespace CleanArchitecture.Infrastructure.Encryption;

public sealed class EncryptionService : IEncryptionService
{
    private readonly Option _option;
    private const int Iteration = 8;
    private readonly Lazy<Aes> _encryptor;
    public EncryptionService(IOptions<Option> options)
    {
        _option = options.Value;
        _encryptor = new Lazy<Aes>(() => CreateAes());
    }

    private Aes Encryptor => _encryptor.Value;

    public T? Descrypt<T>(string encryptText) where T : class
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(encryptText);

        byte[] encryptedBytes = Convert.FromBase64String(encryptText);

        using MemoryStream ms = new MemoryStream();
        using (CryptoStream cs = new CryptoStream(ms, Encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        {
            cs.Write(encryptedBytes, 0, encryptedBytes.Length);
        }

        var plainText = System.Text.Encoding.UTF8.GetString(ms.ToArray());

        return string.IsNullOrEmpty(plainText) ? default : JsonSerializer.Deserialize<T>(plainText);
    }

    public string Encrypt<T>(T item) where T : class
    {
        ArgumentNullException.ThrowIfNull(item);

        var plaintext = JsonSerializer.Serialize(item);
        byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);

        using MemoryStream ms = new MemoryStream();
        using (CryptoStream cs = new(ms, Encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(plaintextBytes, 0, plaintextBytes.Length);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    private Aes CreateAes()
    {
        var keyInBytes = System.Text.Encoding.UTF8.GetBytes(_option.Key!);
        var saltInBytes = System.Text.Encoding.UTF8.GetBytes(_option.Salt!);

        var passwordBytes = new Rfc2898DeriveBytes(keyInBytes,
        saltInBytes,
        Iteration,
        HashAlgorithmName.SHA256);

        var encryptor = Aes.Create();

        encryptor.Key = passwordBytes.GetBytes(32);
        encryptor.IV = passwordBytes.GetBytes(16);

        return encryptor;
    }
}