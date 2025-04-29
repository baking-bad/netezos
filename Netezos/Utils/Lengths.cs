#pragma warning disable IDE1006
namespace Netezos
{
    static class Lengths
    {
        /// <summary>
        /// Block hash
        /// </summary>
        public static class B
        {
            public const int Encoded = 51;
            public const int Decoded = 32;
        }

        /// <summary>
        /// Operation list list hash
        /// </summary>
        public static class LLo
        {
            public const int Encoded = 53;
            public const int Decoded = 32;
        }

        /// <summary>
        /// Protocol hash
        /// </summary>
        public static class P
        {
            public const int Encoded = 51;
            public const int Decoded = 32;
        }

        /// <summary>
        /// Context hash
        /// </summary>
        public static class Co
        {
            public const int Encoded = 52;
            public const int Decoded = 32;
        }

        /// <summary>
        /// ed25519 public key hash
        /// </summary>
        public static class tz1
        {
            public const int Encoded = 36;
            public const int Decoded = 20;
        }

        /// <summary>
        /// Originated address
        /// </summary>
        public static class KT1
        {
            public const int Encoded = 36;
            public const int Decoded = 20;
        }

        /// <summary>
        /// Originated TX Rollup
        /// </summary>
        public static class txr1
        {
            public const int Encoded = 37;
            public const int Decoded = 20;
        }

        /// <summary>
        /// Originated smart rollup
        /// </summary>
        public static class sr1
        {
            public const int Encoded = 36;
            public const int Decoded = 20;
        }

        /// <summary>
        /// Smart rollup commitment
        /// </summary>
        public static class src1
        {
            public const int Encoded = 54;
            public const int Decoded = 32;
        }

        /// <summary>
        /// Compressed smart rollup state
        /// </summary>
        public static class srs1
        {
            public const int Encoded = 54;
            public const int Decoded = 32;
        }

        /// <summary>
        /// generic signature
        /// </summary>
        public static class sig
        {
            public const int Encoded = 96;
            public const int Decoded = 64;
        }

        /// <summary>
        /// generic signature
        /// </summary>
        public static class BLsig
        {
            // public const int Encoded = 96;
            public const int Decoded = 96;
        }

        /// <summary>
        /// nce
        /// </summary>
        public static class nce
        {
            public const int Encoded = 53;
            public const int Decoded = 32;
        }

        /// <summary>
        /// vh
        /// </summary>
        public static class vh
        {
            public const int Encoded = 52;
            public const int Decoded = 32;
        }
    }
}
#pragma warning restore IDE1006
