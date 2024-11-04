using Microsoft.Identity.Client;
using System.Security.Cryptography;

namespace fima_client_maui7.Auth
{
    /// <summary>
    /// Physically persists auth tokens so they can be reused across application restarts.
    /// </summary>
    public class TokenPersistence
    {
        /// <summary>
        /// Just a random encryption key to save the auth tokens.
        /// </summary>
        private static readonly byte[] _encryptionKey = { 0xCA, 0x22, 0x00, 0xBF, 0x7A, 0xEA, 0xE3, 0x69, 0x2B, 0x2D, 0x25, 0xFF, 0xF5, 0x47, 0xA2, 0x44,
            0xDF, 0xAA, 0x61, 0x95, 0x19, 0x7A, 0x87, 0xCD, 0x43, 0x92, 0x32, 0x29, 0x0E, 0x79, 0xAF, 0xE3 };

        private static readonly string CacheFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "fima\\token_cache.dat");


        /// <summary>
        /// Adds hooks to load and save the tokencache, so they are persisted across application restarts.
        /// </summary>
        public static void ConfigureOn(ITokenCache tokenCache)
        {
            tokenCache.SetBeforeAccessAsync(BeforeAccessNotification);
            tokenCache.SetAfterAccessAsync(AfterAccessNotification);
        }

        private static async Task BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            if (File.Exists(CacheFilePath))
            {
                var encryptedData = await File.ReadAllBytesAsync(CacheFilePath);
                var decryptedData = DecryptData(encryptedData);
                args.TokenCache.DeserializeMsalV3(decryptedData);
            }
        }

        private static async Task AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            if (args.HasStateChanged)
            {
                var data = args.TokenCache.SerializeMsalV3();
                var encryptedData = EncryptData(data);
                Directory.CreateDirectory(Path.GetDirectoryName(CacheFilePath));
                await File.WriteAllBytesAsync(CacheFilePath, encryptedData);
            }
        }

        private static byte[] EncryptData(byte[] data)
        {
            using var aes = Aes.Create();
            aes.Key = _encryptionKey;
            aes.GenerateIV();
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length); // Write IV to file first
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }

        private static byte[] DecryptData(byte[] data)
        {
            using var aes = Aes.Create();
            aes.Key = _encryptionKey;
            using var ms = new MemoryStream(data);
            var iv = new byte[aes.BlockSize / 8];
            ms.Read(iv, 0, iv.Length); // Read IV from the file
            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var resultStream = new MemoryStream();
            cs.CopyTo(resultStream);
            return resultStream.ToArray();
        }

        public static void Clear()
        {
            File.Delete(CacheFilePath);
        }
    }
}
