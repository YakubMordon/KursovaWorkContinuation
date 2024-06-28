using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace KursovaWorkDAL.Entity.Service
{
    /// <summary>
    /// Class for encryption/decryption of data.
    /// </summary>
    public class Encrypter
    {
        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole())
            .CreateLogger(typeof(Encrypter));

        private static readonly byte[] Key = Encoding.UTF8.GetBytes("ojvafou1najfvsiu84IvnA42vhiOsv3M");

        /// <summary>
        /// Method for encrypting Bank card number.
        /// </summary>
        /// <param name="value">Bank card number.</param>
        /// <returns>Encrypted Bank card number.</returns>
        public static string Encrypt(string value)
        {
            _logger.LogInformation("Generating random encryption key.");
            byte[] key = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
            }

            _logger.LogInformation("Generating random initialization vector.");
            byte[] iv = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            }

            _logger.LogInformation("Converting string to byte array.");
            byte[] plaintext = Encoding.UTF8.GetBytes(value);

            _logger.LogInformation("Encrypting data using AES encryption with random key and initialization vector.");
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

                    _logger.LogInformation("Combining encrypted bytes with key and initialization vector into one byte array.");
                    byte[] encrypted = ms.ToArray();
                    byte[] result = new byte[key.Length + iv.Length + encrypted.Length];
                    Buffer.BlockCopy(key, 0, result, 0, key.Length);
                    Buffer.BlockCopy(iv, 0, result, key.Length, iv.Length);
                    Buffer.BlockCopy(encrypted, 0, result, key.Length + iv.Length, encrypted.Length);

                    _logger.LogInformation("Converting result to base64 string and returning it.");
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
            _logger.LogInformation("Splitting received value into key, initialization vector, and encrypted text.");
            byte[] result = Convert.FromBase64String(encryptedValue);
            byte[] key = new byte[32];
            byte[] iv = new byte[16];
            byte[] encrypted = new byte[result.Length - key.Length - iv.Length];
            Buffer.BlockCopy(result, 0, key, 0, key.Length);
            Buffer.BlockCopy(result, key.Length, iv, 0, iv.Length);
            Buffer.BlockCopy(result, key.Length + iv.Length, encrypted, 0, encrypted.Length);
            _logger.LogInformation("Decrypting encrypted text with key and initialization vector.");
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(encrypted))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs, Encoding.UTF8))
                {
                    _logger.LogInformation("Returning decrypted text.");
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
            _logger.LogInformation("Hashing password.");
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                _logger.LogInformation("Returning hashed password.");
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
            _logger.LogInformation("Encrypting month.");
            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.GenerateIV();

                byte[] encrypted;

                using (var encryptor = aes.CreateEncryptor())
                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(aes.IV, 0, aes.IV.Length);

                    byte[] monthBytes = Encoding.UTF8.GetBytes(month);

                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(monthBytes, 0, monthBytes.Length);
                    }

                    encrypted = memoryStream.ToArray();
                }
                _logger.LogInformation("Returning encrypted month.");
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
            _logger.LogInformation("Decrypting month.");
            byte[] encrypted = Convert.FromBase64String(encryptedMonth);

            using (var aes = Aes.Create())
            {
                aes.Key = Key;

                byte[] iv = new byte[aes.IV.Length];
                Buffer.BlockCopy(encrypted, 0, iv, 0, iv.Length);

                byte[] monthBytes;

                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                using (var memoryStream = new MemoryStream(encrypted, iv.Length, encrypted.Length - iv.Length))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    monthBytes = new byte[memoryStream.Length];
                    cryptoStream.Read(monthBytes, 0, monthBytes.Length);
                }
                _logger.LogInformation("Returning decrypted month.");
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
            _logger.LogInformation("Encrypting year.");
            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.GenerateIV();

                byte[] encrypted;

                using (var encryptor = aes.CreateEncryptor())
                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(aes.IV, 0, aes.IV.Length);

                    byte[] yearBytes = Encoding.UTF8.GetBytes(year);

                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(yearBytes, 0, yearBytes.Length);
                    }

                    encrypted = memoryStream.ToArray();
                }

                _logger.LogInformation("Returning encrypted year.");
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
            _logger.LogInformation("Decrypting year.");
            byte[] encrypted = Convert.FromBase64String(encryptedYear);

            using (var aes = Aes.Create())
            {
                aes.Key = Key;

                byte[] iv = new byte[aes.IV.Length];
                Buffer.BlockCopy(encrypted, 0, iv, 0, iv.Length);

                byte[] yearBytes;

                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                using (var memoryStream = new MemoryStream(encrypted, iv.Length, encrypted.Length - iv.Length))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    yearBytes = new byte[memoryStream.Length];
                    cryptoStream.Read(yearBytes, 0, yearBytes.Length);
                }

                _logger.LogInformation("Returning decrypted year.");
                return Encoding.UTF8.GetString(yearBytes).Substring(0, 2);
            }
        }

        /// <summary>
        /// Method for encrypting CVV.
        /// </summary>
        /// <param name="cvv">CVV to encrypt.</param>
        /// <returns>Encrypted CVV.</returns>
        public static string EncryptCVV(string cvv)
        {
            _logger.LogInformation("Encrypting CVV.");
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

                _logger.LogInformation("Returning encrypted CVV.");
                return Convert.ToBase64String(encrypted);
            }
        }

        /// <summary>
        /// Method for decrypting CVV.
        /// </summary>
        /// <param name="encryptedCVV">Encrypted CVV.</param>
        /// <returns>Decrypted CVV.</returns>
        public static string DecryptCVV(string encryptedCVV)
        {
            _logger.LogInformation("Decrypting CVV.");
            byte[] encrypted = Convert.FromBase64String(encryptedCVV);

            using (var aes = Aes.Create())
            {
                aes.Key = Key;

                byte[] iv = new byte[aes.IV.Length];
                Buffer.BlockCopy(encrypted, 0, iv, 0, iv.Length);

                byte[] cvvBytes;

                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                using (var memoryStream = new MemoryStream(encrypted, iv.Length, encrypted.Length - iv.Length))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    string cvv = streamReader.ReadToEnd();
                    cvvBytes = Encoding.UTF8.GetBytes(cvv);
                }

                _logger.LogInformation("Returning decrypted CVV.");
                return Encoding.UTF8.GetString(cvvBytes);
            }
        }
    }
}
