namespace CleanArchitecture.Application.Core.Abstraction.Services;

public interface IEncryptionService
{
    string Encrypt<T>(T item) where T : class;

    T? Descrypt<T>(string encryptText) where T : class;
}