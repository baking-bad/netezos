namespace Netezos
{
    static class DateTimeExtension
    {
        public static int ToUnixTime(this DateTime datetime)
        {
            return (int)(datetime - DateTime.UnixEpoch).TotalSeconds;
        }

        public static DateTime FromUnixTime(long seconds)
        {
            return DateTime.UnixEpoch.AddSeconds(seconds);
        }
    }
}
