using System.Security.Cryptography;
using System.Text;

namespace YeahBuddy.Application.Services.Cryptography;

public class PasswordHasher(string additionKey)
{
    public string Encrypt(string password)
    {
        var newPassword = password + additionKey;

        var pwdBytes = Encoding.UTF8.GetBytes(newPassword);
        var hashBytes = SHA512.HashData(pwdBytes);

        return StringBytes(hashBytes);
    }

    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}