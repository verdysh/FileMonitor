using System.Security.Cryptography;

namespace Services.Helpers
{
    public static class EncryptionHelper
    {
        public static string GetHash(string file)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(file);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
