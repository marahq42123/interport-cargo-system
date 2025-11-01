namespace InterportCargo.Web.Services
{
    /// <summary>
    /// Provides hashing and verification methods for user authentication.
    /// </summary>
    public static class AuthService
    {
        /// <summary>
        /// Hashes a plain text password using BCrypt.
        /// </summary>
        public static string Hash(string plain) => BCrypt.Net.BCrypt.HashPassword(plain);

        /// <summary>
        /// Verifies a plain text password against a hashed password.
        /// </summary>
        public static bool Verify(string plain, string hash) => BCrypt.Net.BCrypt.Verify(plain, hash);
    }
}
