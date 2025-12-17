using System.Security.Cryptography;
using System.Text;

namespace life_Book.Data.Utils;

public static class PasswordHasher
{
    private const int SaltSize = 16; // 128 bits
  private const int KeySize = 32; // 256 bits
    private const int Iterations = 10000; // PBKDF2 iterations

    /// <summary>
    /// Hash a password with a random salt using PBKDF2
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <returns>The hashed password with salt (Base64 encoded)</returns>
    public static string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        // Hash the password with the salt
   byte[] hash = HashPasswordWithSalt(password, salt);

        // Combine salt and hash
 byte[] hashBytes = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
     Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

   // Convert to Base64 for storage
 return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Verify a password against a hashed password
    /// </summary>
 /// <param name="password">The password to verify</param>
    /// <param name="hashedPassword">The hashed password (with salt)</param>
    /// <returns>True if the password matches, false otherwise</returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
    try
      {
      // Convert the hashed password from Base64
   byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extract the salt (first 16 bytes)
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

         // Extract the hash (remaining 32 bytes)
            byte[] storedHash = new byte[KeySize];
            Array.Copy(hashBytes, SaltSize, storedHash, 0, KeySize);

            // Hash the input password with the extracted salt
            byte[] hash = HashPasswordWithSalt(password, salt);

  // Compare the hashes
   return CompareHashes(hash, storedHash);
        }
        catch
        {
          return false;
        }
    }

 /// <summary>
    /// Hash a password with a specific salt using PBKDF2
    /// </summary>
    private static byte[] HashPasswordWithSalt(string password, byte[] salt)
    {
     using var pbkdf2 = new Rfc2898DeriveBytes(
      password: Encoding.UTF8.GetBytes(password),
            salt: salt,
        iterations: Iterations,
       hashAlgorithm: HashAlgorithmName.SHA256
    );

return pbkdf2.GetBytes(KeySize);
    }

    /// <summary>
    /// Securely compare two byte arrays (constant-time comparison to prevent timing attacks)
    /// </summary>
    private static bool CompareHashes(byte[] hash1, byte[] hash2)
    {
        if (hash1.Length != hash2.Length)
         return false;

        int result = 0;
     for (int i = 0; i < hash1.Length; i++)
 {
            result |= hash1[i] ^ hash2[i];
        }

        return result == 0;
    }
}
