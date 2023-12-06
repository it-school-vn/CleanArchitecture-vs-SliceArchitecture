using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework.Internal;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Infrastructure.Encryption;
namespace CleanArchitecture.Infrastructure.UnitTest.Encryption;
public class EncryptionServiceTest
{
    private record FakeTestObject(string Name, int Age, string Email);
    private IEncryptionService _service;
    private Option _option;

    private Randomizer _randomizer = Randomizer.CreateRandomizer();

    [SetUp]
    public void Setup()
    {
        _option = new Option
        {
            Key = _randomizer.GetString(25),
            Salt = _randomizer.GetString(25)
        };

        var iOptions = Substitute.For<IOptions<Option>>();

        iOptions.Value.Returns(_option);

        _service = new EncryptionService(iOptions);
    }

    [Test]
    public void Encrypt_NullItem_ThrowNullArgumentException()
    {

#pragma warning disable CS8634
        // Arrange
        FakeTestObject? item = null;

        // Action

        // Assert

        _ = Assert.Throws<ArgumentNullException>(() => _service.Encrypt(item));

#pragma warning restore CS8634

    }

    [Test]
    public void Encrypt_ValidItem_ReturnEncryptedString()
    {
        // Arrange
        var data = new FakeTestObject(_randomizer.GetString(25),
        25, _randomizer.GetString(25));

        // Action

        var encryptedText = _service.Encrypt(data);

        // Assert

        Assert.That(encryptedText, Is.Not.Empty);
    }
    [Test]

    public void Descrypt_NullOrEmptyEncryptedText_ThrowNullArgumentException()
    {

        // Assert
        Assert.Throws<ArgumentException>(() => _service.Descrypt<FakeTestObject>(string.Empty));
    }

    [Test]
    public void Descrypt_ValidEncryptedText_ReturnObject()
    {
        // Arrange
        var data = new FakeTestObject(_randomizer.GetString(25),
        25, _randomizer.GetString(25));

        // Action

        var encryptedText = _service.Encrypt(data);

        var descryptedData = _service.Descrypt<FakeTestObject>(encryptedText);

        // Assert

        Assert.That(descryptedData, Is.Not.Null);

        Assert.That(descryptedData, Is.EqualTo(data));
    }

    public void Descrypt_InvalidEncryptedText_ReturnNull()
    {
        // Arrange

        // Action

        var descryptedData = _service.Descrypt<FakeTestObject>(_randomizer.GetString(200));

        // Assert
        Assert.That(descryptedData, Is.Null);
    }
}