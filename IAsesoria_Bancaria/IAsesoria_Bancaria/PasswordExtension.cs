using BCrypt.Net;

namespace IAsesoria_Bancaria
{
    public static class PasswordExtension
    {
        public static string ToBcryptHash(this string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }
    }
}
