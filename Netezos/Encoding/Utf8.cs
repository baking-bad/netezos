namespace Netezos.Encoding
{
    public static class Utf8
    {
        public static byte[] Parse(string text)
        {
            return System.Text.Encoding.UTF8.GetBytes(text);
        }

        public static bool TryParse(string? text, out byte[] bytes)
        {
            if (text != null)
            {
                try
                {
                    bytes = System.Text.Encoding.UTF8.GetBytes(text);
                    return true;
                }
                catch { }
            }
            bytes = null!;
            return false;
        }

        public static string Convert(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}