using NBitcoin;
using Netezos.Keys;

namespace Netezos.Ledger
{
    public static class Utils
    {
        static readonly byte[] _EdPublicKeyPrefix = [13, 15, 37, 217];
        static readonly byte[] _EdSignaturePrefix = [9, 245, 205, 134, 18];
        
        static readonly byte[] _NistPublicKeyPrefix = [3, 178, 139, 127];
        static readonly byte[] _NistSignaturePrefix = [54, 240, 44, 52];
        
        static readonly byte[] _SecpPublicKeyPrefix = [3, 254, 226, 86];
        static readonly byte[] _SecpSignaturePrefix = [13, 115, 101, 19, 63];

        internal static byte[] Serialize(KeyPath keyPath)
        {
            AssertKeyPath(keyPath);
            var memoryStream = new MemoryStream();
            memoryStream.WriteByte((byte) keyPath.Indexes.Length);
            for (int index = 0; index < keyPath.Indexes.Length; ++index)
            {
                byte[] bytes = ToBytes(keyPath.Indexes[index], false);
                memoryStream.Write(bytes, 0, bytes.Length);
            }
            return memoryStream.ToArray();
        }

        internal static List<byte[]> SplitArray(byte[] array, int nSize = 230)
        {
            var list = new List<byte[]>();
            for( var i = 0; i < array.Length; i += nSize)
            {
                list.Add(GetBytes(array, i, Math.Min(nSize, array.Length - i)));
            }

            return list;
        }
        
        static void AssertKeyPath(KeyPath keyPath)
        {
            if (keyPath.Indexes.Length > 10)
                throw new ArgumentOutOfRangeException("keypath", "The key path should have a maximum size of 10 derivations");
        }
        
        static byte[] ToBytes(uint value, bool littleEndian)
        {
            if (littleEndian)
                return
                [
                    (byte) value,
                    (byte) (value >> 8),
                    (byte) (value >> 16),
                    (byte) (value >> 24)
                ];
            return
            [
                (byte) (value >> 24),
                (byte) (value >> 16),
                (byte) (value >> 8),
                (byte) value
            ];
        }

        internal static byte[] GetBytes(byte[] src, int start, int length)
        {
            var res = new byte[length];
            Buffer.BlockCopy(src, start, res, 0, length);
            return res;
        }
        
        internal static byte[] GetSignaturePrefix(ECKind kind)
        {
            return kind == ECKind.Ed25519
                ? _EdSignaturePrefix
                : kind == ECKind.NistP256
                    ? _NistSignaturePrefix
                    : _SecpSignaturePrefix;
        }
        
        internal static byte[] GetPubKeyPrefix(ECKind kind)
        {
            return kind == ECKind.Ed25519
                ? _EdPublicKeyPrefix
                : kind == ECKind.NistP256
                    ? _NistPublicKeyPrefix
                    : _SecpPublicKeyPrefix;
        }
        
    }
}