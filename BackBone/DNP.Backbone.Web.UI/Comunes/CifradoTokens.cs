namespace DNP.Backbone.Web.UI.Comunes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    [ExcludeFromCodeCoverage]
    public class CifradoTokens
    {
        private static readonly TimeZoneInfo CustomTimeZone = TimeZoneInfo.CreateCustomTimeZone("Custom Time",
            new TimeSpan(-5, 0, 0), "Custom Timezone",
            "Custom Time");

        public static byte[] EncryptionKey = Encoding.ASCII.GetBytes("FBKmKC0sLb0IX5^796*6jNoYBqOtz0Q0");

        public static string EncryptStringWithTimeBasedSaltUrl(string toEncrypt)
        {
            return Base64ForUrlEncode(EncryptStringWithTimeBasedSalt(toEncrypt, DateTime.UtcNow));
        }

        public static string EncryptStringWithTimeBasedSalt(string toEncrypt, DateTime currentDate)
        {
            try
            {
                byte[] toEncryptBytes = Encoding.UTF8.GetBytes(toEncrypt);
                using (var provider = new AesCryptoServiceProvider())
                {
                    provider.Key = AddSalt(EncryptionKey, currentDate);
                    provider.Mode = CipherMode.CBC;
                    provider.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform encryptor = provider.CreateEncryptor(provider.Key, provider.IV))
                    {
                        var ms = new MemoryStream();
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(toEncryptBytes, 0, toEncryptBytes.Length);
                            cs.FlushFinalBlock();
                            var retVal = new byte[16 + ms.Length];
                            provider.IV.CopyTo(retVal, 0);
                            ms.ToArray().CopyTo(retVal, 16);
                            return Convert.ToBase64String(retVal);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public static string DecryptStringWithTimeBasedSalt(string encryptedString, DateTime currentDate)
        {
            using (var provider = new AesCryptoServiceProvider())
            {
                byte[] encriptedStringBytes = Convert.FromBase64String(encryptedString);
                provider.Key = AddSalt(EncryptionKey, currentDate);
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                provider.IV = encriptedStringBytes.Take(16).ToArray();
                var ms = new MemoryStream(encriptedStringBytes, 16, encriptedStringBytes.Length - 16);
                using (ICryptoTransform decryptor = provider.CreateDecryptor(provider.Key, provider.IV))
                {
                    try
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            var decrypted = new byte[encriptedStringBytes.Length];
                            int byteCount = cs.Read(decrypted, 0, encriptedStringBytes.Length);
                            return Encoding.UTF8.GetString(decrypted, 0, byteCount);
                        }
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }
                }

            }
        }

        public static string Base64ForUrlEncode(string str)
        {
            var encbuff = Encoding.UTF8.GetBytes(str);
            return HttpServerUtility.UrlTokenEncode(encbuff);
        }

        public static string Base64ForUrlDecode(string str)
        {
            var decbuff = HttpServerUtility.UrlTokenDecode(str);
            return decbuff != null ? Encoding.UTF8.GetString(decbuff) : null;
        }

        private static byte[] AddSalt(byte[] lowSodiumKey, DateTime currentDate)
        {
            int salt = TimeZoneInfo.ConvertTime(currentDate, CustomTimeZone).DayOfYear;
            if (salt > 255)
            {
                lowSodiumKey[0] = 0xFE;
                lowSodiumKey[7] = (byte)(salt - 255);
            }
            else
            {
                lowSodiumKey[15] = (byte)(salt);
            }
            return lowSodiumKey;
        }

        public static string DecryptStringWithTimeBasedSaltUrl(string encryptedString)
        {
            return DecryptStringWithTimeBasedSalt(Base64ForUrlDecode(encryptedString), DateTime.UtcNow);
        }
    }
}
