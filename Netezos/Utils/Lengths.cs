namespace Netezos
{
    static class Lengths
    {
        /// <summary>
        /// Block hash
        /// </summary>
        public static class B
        {
            public const int Encdoed = 51;
            public const int Decoded = 32;
        }

        /// <summary>
        /// Operation list list hash
        /// </summary>
        public static class LLo
        {
            public const int Encdoed = 53;
            public const int Decoded = 32;
        }

        /// <summary>
        /// Protocol hash
        /// </summary>
        public static class P
        {
            public const int Encdoed = 51;
            public const int Decoded = 32;
        }

        /// <summary>
        /// Context hash
        /// </summary>
        public static class Co
        {
            public const int Encdoed = 52;
            public const int Decoded = 32;
        }

        /// <summary>
        /// ed25519 public key hash
        /// </summary>
        public static class tz1
        {
            public const int Encdoed = 36;
            public const int Decoded = 20;
        }

        /// <summary>
        /// Originated address
        /// </summary>
        public static class KT1
        {
            public const int Encdoed = 36;
            public const int Decoded = 20;
        }

        /// <summary>
        /// Originated TX Rollup
        /// </summary>
        public static class txr1
        {
            public const int Encdoed = 37;
            public const int Decoded = 20;
        }

        /// <summary>
        /// generic signature
        /// </summary>
        public static class sig
        {
            public const int Encdoed = 96;
            public const int Decoded = 64;
        }

        /// <summary>
        /// nce
        /// </summary>
        public static class nce
        {
            public const int Encdoed = 53;
            public const int Decoded = 32;
        }

        /// <summary>
        /// vh
        /// </summary>
        public static class vh
        {
            public const int Encdoed = 52;
            public const int Decoded = 32;
        }
    }
}
