using System.Security.Cryptography;

namespace ChloeOS.Core.Models.Misc;

public class Password {

    // Very important, KEEP SECRET!
    private const int HashSize = 32;
    private const int SaltSize = 16;
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public byte[] Hash { get; }
    public byte[] Salt { get; }

    public Password(byte[] hash, byte[] salt) {
        Hash = hash;
        Salt = salt;
    }

    public Password(string hash, string salt) : this(Convert.FromBase64String(hash), Convert.FromBase64String(salt)) {
    }

    // User --> Password.
    public static explicit operator Password(User user) => new (user.PasswordHash, user.PasswordSalt);

    public static Password GenerateHash(string? password) {
        if (string.IsNullOrEmpty(password)) {
            return null!;
        }

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = GetHash(password, salt);

        return new Password(hash, salt);
    }

    public bool VerifyHash(string password) {
        byte[] hash = GetHash(password, Salt);

        string hashString = Convert.ToBase64String(hash) + Convert.ToBase64String(Salt);
        return hash.SequenceEqual(Hash) && ToString() == hashString;
    }

    private static byte[] GetHash(string password, byte[] salt)
        => Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

    public override string ToString() => Convert.ToBase64String(Hash) + Convert.ToBase64String(Salt);

}