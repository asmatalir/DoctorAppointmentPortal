using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace PurchaseOrderManagementSystem.Helper
{
    public static class CryptoEncryption
    {
        private static readonly string encryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
        private static readonly string salt = ConfigurationManager.AppSettings["Salt"];


        public static string Encrypt(string plainText)
        {
            byte[] key = new Rfc2898DeriveBytes(encryptionKey, Encoding.UTF8.GetBytes(salt)).GetBytes(32);
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
            byte[] iv = new byte[16];
            byte[] cipher = new byte[fullCipher.Length - 16];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, 16, cipher, 0, cipher.Length);

            byte[] key = new Rfc2898DeriveBytes(encryptionKey, Encoding.UTF8.GetBytes(salt)).GetBytes(32);
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(cipher))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
    public static class OtpHelper
    {
        public static string GenerateAlphanumericOtp(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
