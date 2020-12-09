namespace Netezos.Encoding
{
    public static class Utf8
    {
        public static byte[] Parse(string text)
        {
            return System.Text.Encoding.UTF8.GetBytes(text);
        }

        public static string Convert(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}