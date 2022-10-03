using Cookbook.Application.Services.Interfaces;
using Cookbook.Core.Entities;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Cookbook.Application.Services;

public class CryptographyService : ICryptographyService
{

    private readonly IConfiguration _config;

    public CryptographyService(IConfiguration config)
    {
        _config = config;
    }

    public string CryptographyPassword(string password)
    {
        var passwordWithKey = $"{password}{_config["CryptographyKey"]}";
        var bytes = Encoding.UTF8.GetBytes(passwordWithKey);
        var sha512 = SHA512.Create();
        byte[] hashBytes = sha512.ComputeHash(bytes);
        return StringBytes(hashBytes);
    }

    private string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }
        return sb.ToString();
    }
}
