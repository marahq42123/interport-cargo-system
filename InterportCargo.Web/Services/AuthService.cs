namespace InterportCargo.Web.Services
{
    public static class AuthService
    {
        public static string Hash(string plain) => BCrypt.Net.BCrypt.HashPassword(plain);
        public static bool Verify(string plain, string hash) => BCrypt.Net.BCrypt.Verify(plain, hash);
    }
}
