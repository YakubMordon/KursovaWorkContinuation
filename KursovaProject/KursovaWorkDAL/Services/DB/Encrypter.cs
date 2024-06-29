using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace KursovaWork.Infrastructure.Services.DB;

/// <summary>
/// Class for encryption/decryption of data.
/// </summary>
public class Encrypter
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("ojvafou1najfvsiu84IvnA42vhiOsv3M");

    /// <summary>
    /// Method for encrypting Bank card number.
    /// </summary>
    /// <param name="value">Bank card number.</param>
    /// <returns>Encrypted Bank card number.</returns>
    public static string Encrypt(string value)
    {
        Log.Information("Generating random encryption key.");
        var key = new byte[32];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(key);
        }

        Log.Information("Generating random initialization vector.");
        var iv = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(iv);
        }

        Log.Information("Converting string to byte array.");
        var plaintext = Encoding.UTF8.GetBytes(value);

        Log.Information("Encrypting data using AES encryption with random key and initialization vector.");
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (var encryptor = aes.CreateEncryptor())
            using (var ms = new MemoryStream())
            {
                ms.Write(iv, 0, iv.Length);
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(plaintext, 0, plaintext.Length);
                }

                Log.Information("Combining encrypted bytes with key and initialization vector into one byte array.");

                var encrypted = ms.ToArray();
                var result = new byte[key.Length + iv.Length + encrypted.Length];

                Buffer.BlockCopy(key, 0, result, 0, key.Length);
                Buffer.BlockCopy(iv, 0, result, key.Length, iv.Length);
                Buffer.BlockCopy(encrypted, 0, result, key.Length + iv.Length, encrypted.Length);

                Log.Information("Converting result to base64 string and returning it.");
                return Convert.ToBase64String(result);
            }
        }
    }

    /// <summary>
    /// Method for decrypting Bank card number.
    /// </summary>
    /// <param name="encryptedValue">Encrypted Bank card number.</param>
    /// <returns>Decrypted Bank card number.</returns>
    public static string Decrypt(string encryptedValue)
    {
        Log.Information("Splitting received value into key, initialization vector, and encrypted text.");

        var result = Convert.FromBase64String(encryptedValue);
        var key = new byte[32];
        var iv = new byte[16];
        var encrypted = new byte[result.Length - key.Length - iv.Length];

        Buffer.BlockCopy(result, 0, key, 0, key.Length);
        Buffer.BlockCopy(result, key.Length, iv, 0, iv.Length);
        Buffer.BlockCopy(result, key.Length + iv.Length, encrypted, 0, encrypted.Length);

        Log.Information("Decrypting encrypted text with key and initialization vector.");
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor())
            using (var ms = new MemoryStream(encrypted))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var reader = new StreamReader(cs, Encoding.UTF8))
            {
                Log.Information("Returning decrypted text.");
                return reader.ReadToEnd();
            }
        }

    }

    /// <summary>
    /// Method for hashing password.
    /// </summary>
    /// <param name="password">Password to hash.</param>
    /// <returns>Hashed password value.</returns>
    public static string HashPassword(string password)
    {
        Log.Information("Hashing password.");

        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            Log.Information("Returning hashed password.");

            return Convert.ToBase64String(hashedBytes);
        }
    }

    /// <summary>
    /// Method for encrypting month.
    /// </summary>
    /// <param name="month">Month to encrypt.</param>
    /// <returns>Encrypted month.</returns>
    public static string EncryptMonth(string month)
    {
        Log.Information("Encrypting month.");
        using (var aes = Aes.Create())
        {
            aes.Key = Key;
            aes.GenerateIV();

            byte[] encrypted;

            using (var encryptor = aes.CreateEncryptor())
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(aes.IV, 0, aes.IV.Length);

                var monthBytes = Encoding.UTF8.GetBytes(month);

                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(monthBytes, 0, monthBytes.Length);
                }

                encrypted = memoryStream.ToArray();
            }
            Log.Information("Returning encrypted month.");
            return Convert.ToBase64String(encrypted);
        }
    }

    /// <summary>
    /// Method for decrypting month.
    /// </summary>
    /// <param name="encryptedMonth">Encrypted month.</param>
    /// <returns>Decrypted month.</returns>
    public static string DecryptMonth(string encryptedMonth)
    {
        Log.Information("Decrypting month.");
        var encrypted = Convert.FromBase64String(encryptedMonth);

        using (var aes = Aes.Create())
        {
            aes.Key = Key;

            var iv = new byte[aes.IV.Length];
            Buffer.BlockCopy(encrypted, 0, iv, 0, iv.Length);

            byte[] monthBytes;

            using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
            using (var memoryStream = new MemoryStream(encrypted, iv.Length, encrypted.Length - iv.Length))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                monthBytes = new byte[memoryStream.Length];
                cryptoStream.Read(monthBytes, 0, monthBytes.Length);
            }
            Log.Information("Returning decrypted month.");
            return Encoding.UTF8.GetString(monthBytes).Substring(0, 2);
        }
    }

    /// <summary>
    /// Method for encrypting year.
    /// </summary>
    /// <param name="year">Year to encrypt.</param>
    /// <returns>Encrypted year.</returns>
    public static string EncryptYear(string year)
    {
        Log.Information("Encrypting year.");
        using (var aes = Aes.Create())
        {
            aes.Key = Key;
            aes.GenerateIV();

            byte[] encrypted;

            using (var encryptor = aes.CreateEncryptor())
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(aes.IV, 0, aes.IV.Length);

                var yearBytes = Encoding.UTF8.GetBytes(year);

                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(yearBytes, 0, yearBytes.Length);
                }

                encrypted = memoryStream.ToArray();
            }

            Log.Information("Returning encrypted year.");
            return Convert.ToBase64String(encrypted);
        }
    }

    /// <summary>
    /// Method for decrypting year.
    /// </summary>
    /// <param name="encryptedYear">Encrypted year.</param>
    /// <returns>Decrypted year.</returns>
    public static string DecryptYear(string encryptedYear)
    {
        Log.Information("Decrypting year.");
        var encrypted = Convert.FromBase64String(encryptedYear);

        using (var aes = Aes.Create())
        {
            aes.Key = Key;

            var iv = new byte[aes.IV.Length];
            Buffer.BlockCopy(encrypted, 0, iv, 0, iv.Length);

            byte[] yearBytes;

            using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
            using (var memoryStream = new MemoryStream(encrypted, iv.Length, encrypted.Length - iv.Length))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                yearBytes = new byte[memoryStream.Length];
                cryptoStream.Read(yearBytes, 0, yearBytes.Length);
            }

            Log.Information("Returning decrypted year.");
            return Encoding.UTF8.GetString(yearBytes).Substring(0, 2);
        }
    }

    /// <summary>
    /// Method for encrypting CVV.
    /// </summary>
    /// <param name="cvv">CVV to encrypt.</param>
    /// <returns>Encrypted CVV.</returns>
    public static string EncryptCvv(string cvv)
    {
        Log.Information("Encrypting CVV.");
        using (var aes = Aes.Create())
        {
            aes.Key = Key;
            aes.GenerateIV();

            byte[] encrypted;

            using (var encryptor = aes.CreateEncryptor())
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(aes.IV, 0, aes.IV.Length);

                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(cvv);
                }

                encrypted = memoryStream.ToArray();
            }

            Log.Information("Returning encrypted CVV.");
            return Convert.ToBase64String(encrypted);
        }
    }

    /// <summary>
    /// Method for decrypting CVV.
    /// </summary>
    /// <param name="encryptedCvv">Encrypted CVV.</param>
    /// <returns>Decrypted CVV.</returns>
    public static string DecryptCvv(string encryptedCvv)
    {
        Log.Information("Decrypting CVV.");
        var encrypted = Convert.FromBase64String(encryptedCvv);

        using (var aes = Aes.Create())
        {
            aes.Key = Key;

            var iv = new byte[aes.IV.Length];
            Buffer.BlockCopy(encrypted, 0, iv, 0, iv.Length);

            byte[] cvvBytes;

            using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
            using (var memoryStream = new MemoryStream(encrypted, iv.Length, encrypted.Length - iv.Length))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                var cvv = streamReader.ReadToEnd();
                cvvBytes = Encoding.UTF8.GetBytes(cvv);
            }

            Log.Information("Returning decrypted CVV.");
            return Encoding.UTF8.GetString(cvvBytes);
        }
    }
}