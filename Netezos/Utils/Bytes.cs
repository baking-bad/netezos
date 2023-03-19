namespace Netezos.Utils
{
    static class Bytes
    {
        public static byte[] Concat(params byte[][] arrays)
        {
            var res = new byte[arrays.Sum(x => x.Length)];

            for (int s = 0, i = 0; i < arrays.Length; s += arrays[i].Length, i++)
                Buffer.BlockCopy(arrays[i], 0, res, s, arrays[i].Length);

            return res;
        }
    }
}
