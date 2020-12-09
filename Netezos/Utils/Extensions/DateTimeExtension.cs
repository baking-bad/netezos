using System;

namespace Netezos
{
    static class DateTimeExtension
    {
        static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static int ToUnixTime(this DateTime datetime)
        {
            return (int)(datetime - Epoch).TotalSeconds;
        }
    }
}
