using System.Security.Cryptography;
using System.Text;

public class PasswordService
{
    public string HashPassword(string plainPassword)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
            return Convert.ToBase64String(hashedBytes);
        }
    }


    public bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        string computedHash = HashPassword(plainPassword);
        return computedHash == hashedPassword;
    }
}
