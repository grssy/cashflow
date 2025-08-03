using BC = BCrypt.Net;
using CashFlow.Domain.Security.Cryptography;

namespace CashFlow.Infrastructure.Security;
internal class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        string passwordHash = BC.BCrypt.HashPassword(password);

        return passwordHash;
    }

    public bool Verify(string password, string passwordHash) => BC.BCrypt.Verify(password, passwordHash);
}
