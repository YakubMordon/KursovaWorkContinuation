using FluentAssertions;
using KursovaWork.Infrastructure.Services.DB;

namespace KursovaWork.Infrastructure.Unit.Tests.Services.DB;

public class EncrypterTests
{
    [Fact]
    public void Encrypt_Decrypt_ValueIsSame_ShouldReturnOriginalValue()
    {
        // Arrange
        string originalValue = "1234567890123456";

        // Act
        string encryptedValue = Encrypter.Encrypt(originalValue);
        string decryptedValue = Encrypter.Decrypt(encryptedValue);

        // Assert
        decryptedValue.Substring(decryptedValue.Length - 16).Should().Be(originalValue);
    }    
    
    [Fact]
    public void Encrypt_ValuesAreDifferent_ShouldHaveDifferentHashes()
    {
        // Arrange
        string firstValue = "1234567890123456";
        string secondValue = "1234561234567890";

        // Act
        string encryptedFirstValue = Encrypter.Encrypt(firstValue);
        string encryptedSecondValue = Encrypter.Encrypt(secondValue);

        // Assert
        encryptedFirstValue.Should().NotBe(encryptedSecondValue);
    }

    [Fact]
    public void HashPassword_ShouldReturnDifferentHashForDifferentValues()
    {
        // Arrange
        string password1 = "Password123";
        string password2 = "Password456";

        // Act
        string hashedPassword1 = Encrypter.HashPassword(password1);
        string hashedPassword2 = Encrypter.HashPassword(password2);

        // Assert
        hashedPassword1.Should().NotBe(hashedPassword2);
    }

    [Fact]
    public void EncryptMonth_DecryptMonth_ValueIsSame_ShouldReturnOriginalValue()
    {
        // Arrange
        string originalValue = "12";

        // Act
        string encryptedValue = Encrypter.EncryptMonth(originalValue);
        string decryptedValue = Encrypter.DecryptMonth(encryptedValue);

        // Assert
        decryptedValue.Should().Be(originalValue);
    }        
    
    [Fact]
    public void EncryptYear_DecryptYear_ValueIsSame_ShouldReturnOriginalValue()
    {
        // Arrange
        string originalValue = "24";

        // Act
        string encryptedValue = Encrypter.EncryptYear(originalValue);
        string decryptedValue = Encrypter.DecryptYear(encryptedValue);

        // Assert
        decryptedValue.Should().Be(originalValue);
    }    
}
