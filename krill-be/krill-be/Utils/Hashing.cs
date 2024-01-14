using System.Security.Cryptography;
using System.Text;

namespace krill_be.Utils
{
    public class Hashing
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public Hashing() { }

        public string Hash(string toHash, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(toHash),
                salt,
                iterations,
                hashAlgorithm,
                keySize
                );

            return Convert.ToHexString(hash);
        }

        public bool VerifyHash(string toHash, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                toHash,
                salt,
                iterations,
                hashAlgorithm,
                keySize
                );

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}
