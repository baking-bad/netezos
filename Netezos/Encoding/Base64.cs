namespace Netezos.Encoding
{
    public static class Base64
    {
        public static byte[] Parse(string base64)
        {
            return System.Convert.FromBase64String(base64);
        }

        public static bool TryParse(string base64, out byte[] bytes)
        {
            try
            {
                bytes = System.Convert.FromBase64String(base64);
                return true;
            }
            catch
            {
                bytes = null;
                return false;
            }
        }

        public static string Convert(byte[] bytes)
        {
            return System.Convert.ToBase64String(bytes);
        }
    }
}