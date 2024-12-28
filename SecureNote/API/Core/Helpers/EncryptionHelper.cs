namespace API.Core.Helpers;

using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper {
    public static string Encrypt(string plaintext, string key) {
        var aesKey = NormalizeKey(key);
        using var aes = Aes.Create();
        aes.Key = aesKey;
        aes.GenerateIV();
        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
            using var sw = new StreamWriter(cryptoStream);
            sw.Write(plaintext);
        }
        var iv = Convert.ToBase64String(aes.IV);
        var encrypted = Convert.ToBase64String(ms.ToArray());
        return $"{iv}:{encrypted}";
    }

    public static string Decrypt(string ciphertext, string key) {
        var parts = ciphertext.Split(':');
        if (parts.Length != 2) throw new FormatException("Invalid ciphertext format.");
        var iv = Convert.FromBase64String(parts[0]);
        var encrypted = Convert.FromBase64String(parts[1]);

        var aesKey = NormalizeKey(key);
        using var aes = Aes.Create();
        aes.Key = aesKey;
        aes.IV = iv;
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(encrypted);
        using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cryptoStream);
        return sr.ReadToEnd();
    }

    private static byte[] NormalizeKey(string key) {
        return Encoding.UTF8.GetBytes(key.PadRight(32, ' ').Substring(0, 32));
    }
}
