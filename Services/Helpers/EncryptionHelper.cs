using System.Security.Cryptography;

namespace Services.Helpers
{
    public static class EncryptionHelper
    {
        public static string GetHash(string file)
        {
            using var sha1 = SHA1.Create();
            using var stream = File.OpenRead(file);
            var hash = sha1.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
