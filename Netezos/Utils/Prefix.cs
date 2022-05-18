﻿namespace Netezos
{
    static class Prefix
    {
        /// <summary>
        /// Block hash prefix
        /// </summary>
        public static readonly byte[] B = new byte[] { 1, 52 };

        /// <summary>
        /// Operation hash prefix
        /// </summary>
        public static readonly byte[] o = new byte[] { 5, 116 };

        /// <summary>
        /// Operation list hash prefix
        /// </summary>
        public static readonly byte[] Lo = new byte[] { 133, 233 };

        /// <summary>
        /// Operation list list hash prefix
        /// </summary>
        public static readonly byte[] LLo = new byte[] { 29, 159, 109 };

        /// <summary>
        /// Protocol hash prefix
        /// </summary>
        public static readonly byte[] P = new byte[] { 2, 170 };

        /// <summary>
        /// Context hash prefix
        /// </summary>
        public static readonly byte[] Co = new byte[] { 79, 199 };

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
        /// Address prefix for TX Rollup L2 address
        /// </summary>
        public static readonly byte[] tz4 = new byte[] { 6, 161, 166 };

        /// <summary>
        /// Address prefix for originated contract
        /// </summary>
        public static readonly byte[] KT1 = new byte[] { 2, 90, 121 };

        /// <summary>
        /// Address prefix for originated tx rollup
        /// </summary>
        public static readonly byte[] txr1 = new byte[] { 1, 128, 120, 31 };

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

        /// <summary>
        /// Seed nonce hash prefix
        /// </summary>
        public static readonly byte[] nce = new byte[] { 69, 220, 169 };

        /// <summary>
        /// Script expression
        /// </summary>
        public static readonly byte[] expr = new byte[] { 13, 44, 64, 27 };

        /// <summary>
        /// Consensus value hash (e.g. block payload hash)
        /// </summary>
        public static readonly byte[] vh = new byte[] { 1, 106, 242 };
    }
}
