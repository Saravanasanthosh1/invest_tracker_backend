using System.Security.Cryptography;
using System.Text;

namespace investtracker.Helpers
{
    public static class PasswordHelper
    {
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public static string ComputeHash(string password, string salt)
        {
            using var sha = SHA256.Create();
            var saltedPassword = password + salt;
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(bytes);
        }
    }
}
