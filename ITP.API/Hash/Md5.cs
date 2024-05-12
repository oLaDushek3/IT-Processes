using System;
using System.Security.Cryptography;
using System.Text;

namespace ITProcesses.Hash;

public class Md5
{
    public static string HashPassword(string password)
    {
        byte[] input = Encoding.UTF8.GetBytes(password);

        using var md5 = MD5.Create();
        var output = md5.ComputeHash(input);

        return Convert.ToBase64String(output);
    }
}