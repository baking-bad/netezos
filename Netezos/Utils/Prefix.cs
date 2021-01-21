namespace Netezos
{
    static class Prefix
    {
        /// <summary>
        /// Address prefix for Ed25519
        /// </summary>
        public static readonly byte[] tz1 = new byte[] { 6, 161, 159 };

        /// <summary>
        /// Address prefix for Secp256k1
        /// </summary>
        public static readonly byte[] tz2 = new byte[] { 6, 161, 161 };

        /// <summary>
        /// Address prefix for Nistp256
        /// </summary>
        public static readonly byte[] tz3 = new byte[] { 6, 161, 164 };

        /// <summary>
        /// Address prefix for originated contract
        /// </summary>
        public static readonly byte[] KT1 = new byte[] { 2, 90, 121 };

        /// <summary>
        /// Public key prefix for Ed25519 (tz1)
        /// </summary>
        public static readonly byte[] edpk = new byte[] { 13, 15, 37, 217 };

        /// <summary>
        /// Public key prefix for Secp256k1 (tz2)
        /// </summary>
        public static readonly byte[] sppk = new byte[] { 3, 254, 226, 86 };

        /// <summary>
        /// Public key prefix for Nistp256 (tz3)
        /// </summary>
        public static readonly byte[] p2pk = new byte[] { 3, 178, 139, 127 };

        /// <summary>
        /// Private key prefix for Ed25519 (tz1)
        /// </summary>
        public static readonly byte[] edsk = { 13, 15, 58, 7 };

        /// <summary>
        /// Private key prefix for Secp256k1 (tz2)
        /// </summary>
        public static readonly byte[] spsk = { 17, 162, 224, 201 };

        /// <summary>
        /// Private key prefix for Nistp256 (tz3)
        /// </summary>
        public static readonly byte[] p2sk = { 16, 81, 238, 189 };

        /// <summary>
        /// Signature prefix for Ed25519 (tz1)
        /// </summary>
        public static readonly byte[] edsig = { 9, 245, 205, 134, 18 };

        /// <summary>
        /// Signature prefix for Secp256k1 (tz2)
        /// </summary>
        public static readonly byte[] spsig = { 13, 115, 101, 19, 63 };

        /// <summary>
        /// Signature prefix for Nistp256 (tz3)
        /// </summary>
        public static readonly byte[] p2sig = { 54, 240, 44, 52 };

        /// <summary>
        /// Signature prefix
        /// </summary>
        public static readonly byte[] sig = new byte[] { 4, 130, 043 };

        /// <summary>
        /// Chain id prefix
        /// </summary>
        public static readonly byte[] Net = new byte[] { 87, 82, 0 };

    }
}
