using Application.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impl
{
    public class CryptographicService : ICryptographicService
    {
        private byte[] key = new byte[24];
        private byte[] iv = new byte[8];

        public CryptographicService(IOptions<CryptographyConfiguration> config)
        {
            key = Encoding.ASCII.GetBytes(config.Value.Key);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(config.Value.Key));
                Array.Copy(hash, iv, 8);
            }
        }

        public string Encrypt(string plainText)
        {
            byte[] encrypted;
            using (TripleDES tdes = TripleDES.Create())
            {
                // Create an encryptor
                ICryptoTransform encryptor = tdes.CreateEncryptor(key, iv);

                // Create a MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create a CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Convert the string to byte array
                        byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);

                        // Write the bytes to the CryptoStream
                        cs.Write(plainBytes, 0, plainBytes.Length);
                    }

                    // Get the encrypted data from the MemoryStream
                    encrypted = ms.ToArray();
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string encryptedText)
        {

            byte[] decrypted;

            // Decrypt the string
            using (TripleDES tdes = TripleDES.Create())
            {
                // Create a decryptor
                ICryptoTransform decryptor = tdes.CreateDecryptor(key, iv);

                // Create a MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create a CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        // Convert the string to byte array
                        byte[] cipherBytes = Convert.FromBase64String(encryptedText);

                        // Write the bytes to the CryptoStream
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }

                    // Get the decrypted data from the MemoryStream
                    decrypted = ms.ToArray();
                }
            }

            // Convert the decrypted data to UTF-8 string
            return  Encoding.UTF8.GetString(decrypted);
        }
    }
}
