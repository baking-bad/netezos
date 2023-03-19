namespace Netezos
{
    static class DateTimeExtension
    {
        static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, 0);

        public static int ToUnixTime(this DateTime datetime)
        {
            return (int)(datetime - Epoch).TotalSeconds;
        }

        public static DateTime FromUnixTime(long seconds)
        {
            return Epoch.AddSeconds(seconds);
        }
    }
}
