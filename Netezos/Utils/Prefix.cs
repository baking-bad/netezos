namespace Netezos
{
    static class Prefix
    {
        /// <summary>
        /// Block hash prefix
        /// </summary>
        public static readonly byte[] B = [1, 52];

        /// <summary>
        /// Operation hash prefix
        /// </summary>
        public static readonly byte[] o = [5, 116];

        /// <summary>
        /// Operation list hash prefix
        /// </summary>
        public static readonly byte[] Lo = [133, 233];

        /// <summary>
        /// Operation list hash prefix
        /// </summary>
        public static readonly byte[] LLo = [29, 159, 109];

        /// <summary>
        /// Protocol hash prefix
        /// </summary>
        public static readonly byte[] P = [2, 170];

        /// <summary>
        /// Context hash prefix
        /// </summary>
        public static readonly byte[] Co = [79, 199];

        /// <summary>
        /// Address prefix for Ed25519
        /// </summary>
        public static readonly byte[] tz1 = [6, 161, 159];

        /// <summary>
        /// Address prefix for Secp256k1
        /// </summary>
        public static readonly byte[] tz2 = [6, 161, 161];

        /// <summary>
        /// Address prefix for Nistp256
        /// </summary>
        public static readonly byte[] tz3 = [6, 161, 164];

        /// <summary>
        /// Address prefix for BLS12-381
        /// </summary>
        public static readonly byte[] tz4 = [6, 161, 166];

        /// <summary>
        /// Address prefix for originated contract
        /// </summary>
        public static readonly byte[] KT1 = [2, 90, 121];

        /// <summary>
        /// Address prefix for originated tx rollup
        /// </summary>
        public static readonly byte[] txr1 = [1, 128, 120, 31];

        /// <summary>
        /// Address prefix for originated smart rollup
        /// </summary>
        public static readonly byte[] sr1 = [6, 124, 117];

        /// <summary>
        /// Smart rollup state hash prefix
        /// </summary>
        public static readonly byte[] srs1 = [17, 165, 235, 240];

        /// <summary>
        /// Smart rollup commitment hash prefix
        /// </summary>
        public static readonly byte[] src1 = [17, 165, 134, 138];

        /// <summary>
        /// Public key prefix for Ed25519 (tz1)
        /// </summary>
        public static readonly byte[] edpk = [13, 15, 37, 217];

        /// <summary>
        /// Public key prefix for Secp256k1 (tz2)
        /// </summary>
        public static readonly byte[] sppk = [3, 254, 226, 86];

        /// <summary>
        /// Public key prefix for Nistp256 (tz3)
        /// </summary>
        public static readonly byte[] p2pk = [3, 178, 139, 127];

        /// <summary>
        /// Public key prefix for BLS12-381 (tz4)
        /// </summary>
        public static readonly byte[] BLpk = [6, 149, 135, 204];

        /// <summary>
        /// Private key prefix for Ed25519 (tz1)
        /// </summary>
        public static readonly byte[] edsk = [13, 15, 58, 7];

        /// <summary>
        /// Private key prefix for Secp256k1 (tz2)
        /// </summary>
        public static readonly byte[] spsk = [17, 162, 224, 201];

        /// <summary>
        /// Private key prefix for Nistp256 (tz3)
        /// </summary>
        public static readonly byte[] p2sk = [16, 81, 238, 189];

        /// <summary>
        /// Private key prefix for BLS12-381 (tz4)
        /// </summary>
        public static readonly byte[] BLsk = [3, 150, 192, 40];

        /// <summary>
        /// Signature prefix for Ed25519 (tz1)
        /// </summary>
        public static readonly byte[] edsig = [9, 245, 205, 134, 18];

        /// <summary>
        /// Signature prefix for Secp256k1 (tz2)
        /// </summary>
        public static readonly byte[] spsig = [13, 115, 101, 19, 63];

        /// <summary>
        /// Signature prefix for Nistp256 (tz3)
        /// </summary>
        public static readonly byte[] p2sig = [54, 240, 44, 52];

        /// <summary>
        /// Signature prefix for BLS12-381 (tz4)
        /// </summary>
        public static readonly byte[] BLsig = [40, 171, 64, 207];

        /// <summary>
        /// Signature prefix
        /// </summary>
        public static readonly byte[] sig = [4, 130, 043];

        /// <summary>
        /// Chain id prefix
        /// </summary>
        public static readonly byte[] Net = [87, 82, 0];

        /// <summary>
        /// Seed nonce hash prefix
        /// </summary>
        public static readonly byte[] nce = [69, 220, 169];

        /// <summary>
        /// Script expression
        /// </summary>
        public static readonly byte[] expr = [13, 44, 64, 27];

        /// <summary>
        /// Consensus value hash (e.g. block payload hash)
        /// </summary>
        public static readonly byte[] vh = [1, 106, 242];

        /// <summary>
        /// Tx Rollup inbox hash
        /// </summary>
        public static readonly byte[] txi = [79, 148, 196];

        /// <summary>
        /// Tx Rollup message hash
        /// </summary>
        public static readonly byte[] txm = [79, 149, 30];

        /// <summary>
        /// Tx Rollup commitment hash
        /// </summary>
        public static readonly byte[] txc = [79, 148, 17];

        /// <summary>
        /// Tx Rollup message result hash
        /// </summary>
        public static readonly byte[] txmr = [18, 7, 206, 87];

        /// <summary>
        /// Tx Rollup message result list hash
        /// </summary>
        public static readonly byte[] txM = [79, 146, 82];

        /// <summary>
        /// Tx Rollup withdraw list hash
        /// </summary>
        public static readonly byte[] txw = [79, 150, 72];

        /// <summary>
        /// DAL slot header
        /// </summary>
        public static readonly byte[] sh = [2, 116, 180];
    }
}
