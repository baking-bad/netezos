using System.IO;

namespace Netezos
{
    static class BinaryWriterExtension
    {
        public static void Write7BitInt(this BinaryWriter writer, int value)
        {
            while (value > 0x7F)
            {
                writer.Write((byte)(value | ~0x7F));
                value >>= 7;
            }

            writer.Write((byte)value);
        }
    }
}
