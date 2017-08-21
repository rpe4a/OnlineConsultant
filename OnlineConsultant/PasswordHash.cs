using System;
using System.Security.Cryptography;
using System.Text;

namespace OnlineConsultant
{
    public static class PasswordHash
    {
        public static string Encoding(string password)
        {
            using (var hash = MD5.Create())
            {
                byte[] data = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                var sBuilder = new StringBuilder();
                foreach (var t in data)
                    sBuilder.Append(t.ToString("x2"));

                return sBuilder.ToString();
            }
        }

        public static bool IsEqual(string password, string hash)
        {
            var hashOfInput  = Encoding(password);
            var comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}