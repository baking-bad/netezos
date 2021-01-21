using System;
using System.IO;

namespace Netezos
{
    static class BinaryReaderExtension
    {
        public static int Read7BitInt(this BinaryReader reader)
        {
            var res = 0;
            var bits = 0;
            byte b = 0;

            while (bits < 28)
            {
                b = reader.ReadByte();
                res |= (b & 0x7F) << bits;
                bits += 7;

                if (b < 0x80) return res;
            }

            b = reader.ReadByte();
            if (b > 0x0F) throw new FormatException("Int32 overflow");

            res |= b << 28;
            return res;
        }
    }
}
