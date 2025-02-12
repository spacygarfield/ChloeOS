using System.Security.Cryptography;

namespace ChloeOS.Core.Models.Misc;

public class Password {

    // Very important, KEEP SECRET!
    private const int HashSize = 32;
    private const int SaltSize = 16;
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    private byte[] Hash { get; set; }
    private byte[] Salt { get; set; }

    public Password(byte[] hash, byte[] salt) {
        Hash = hash;
        Salt = salt;
    }

    public Password(string hash, string salt) : this(Convert.FromBase64String(hash), Convert.FromBase64String(salt)) {
    }

    public static Password GenerateHash(string? password) {
        if (string.IsNullOrEmpty(password)) {
            return null!;
        }

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return new Password(hash, salt);
    }

    public bool Verify(Password password) => CryptographicOperations.FixedTimeEquals(Hash, password.Hash);

}