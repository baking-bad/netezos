using System.Diagnostics.CodeAnalysis;

namespace Netezos.Encoding
{
    public static class Base64
    {
        public static byte[] Parse(string base64)
        {
            return System.Convert.FromBase64String(base64);
        }

        public static bool TryParse(string? base64, [NotNullWhen(true)] out byte[]? bytes)
        {
            if (base64 != null)
            {
                try
                {
                    bytes = System.Convert.FromBase64String(base64);
                    return true;
                }
                catch
                {
                    // we don't care
                }
            }
            bytes = null;
            return false;
        }

        public static string Convert(byte[] bytes)
        {
            return System.Convert.ToBase64String(bytes);
        }
    }
}