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
        /// Operation hash
        /// </summary>
        public static class o
        {
            public const int Encdoed = 51;
            public const int Decoded = 32;
        }

        /// <summary>
        /// Operation list hash
        /// </summary>
        public static class Lo
        {
            public const int Encdoed = 52;
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
        /// secp256k1 public key hash
        /// </summary>
        public static class tz2
        {
            public const int Encdoed = 36;
            public const int Decoded = 20;
        }

        /// <summary>
        /// p256 public key hash
        /// </summary>
        public static class tz3
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
        /// cryptobox public key hash
        /// </summary>
        public static class id
        {
            public const int Encdoed = 30;
            public const int Decoded = 16;
        }

        /// <summary>
        /// script expression
        /// </summary>
        public static class expr
        {
            public const int Encdoed = 54;
            public const int Decoded = 32;
        }

        /// <summary>
        /// ed25519 seed
        /// </summary>
        public static class eds
        {
            public const int Encdoed = 54;
            public const int Decoded = 32;
        }

        /// <summary>
        /// ed25519 public key
        /// </summary>
        public static class edpk
        {
            public const int Encdoed = 54;
            public const int Decoded = 32;
        }

        /// <summary>
        /// secp256k1 secret key
        /// </summary>
        public static class spsk
        {
            public const int Encdoed = 54;
            public const int Decoded = 32;
        }

        /// <summary>
        /// p256 secret key
        /// </summary>
        public static class p2sk
        {
            public const int Encdoed = 54;
            public const int Decoded = 32;
        }

        /// <summary>
        /// ed25519 encrypted seed
        /// </summary>
        public static class edesk
        {
            public const int Encdoed = 88;
            public const int Decoded = 56;
        }

        /// <summary>
        /// secp256k1 encrypted secret key
        /// </summary>
        public static class spesk
        {
            public const int Encdoed = 88;
            public const int Decoded = 56;
        }

        /// <summary>
        /// p256_encrypted_secret_key
        /// </summary>
        public static class p2esk
        {
            public const int Encdoed = 88;
            public const int Decoded = 56;
        }

        /// <summary>
        /// secp256k1 public key
        /// </summary>
        public static class sppk
        {
            public const int Encdoed = 55;
            public const int Decoded = 33;
        }

        /// <summary>
        /// p256 public key
        /// </summary>
        public static class p2pk
        {
            public const int Encdoed = 55;
            public const int Decoded = 33;
        }

        /// <summary>
        /// secp256k1 scalar
        /// </summary>
        public static class SSp
        {
            public const int Encdoed = 53;
            public const int Decoded = 33;
        }

        /// <summary>
        /// secp256k1 element
        /// </summary>
        public static class GSp
        {
            public const int Encdoed = 53;
            public const int Decoded = 33;
        }

        /// <summary>
        /// ed25519 secret key
        /// </summary>
        public static class edsk
        {
            public const int Encdoed = 98;
            public const int Decoded = 64;
        }

        /// <summary>
        /// ed25519 signature
        /// </summary>
        public static class edsig
        {
            public const int Encdoed = 99;
            public const int Decoded = 64;
        }

        /// <summary>
        /// secp256k1 signature
        /// </summary>
        public static class spsig
        {
            public const int Encdoed = 99;
            public const int Decoded = 64;
        }

        /// <summary>
        /// p256 signature
        /// </summary>
        public static class p2sig
        {
            public const int Encdoed = 98;
            public const int Decoded = 64;
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
        /// chain id
        /// </summary>
        public static class Net
        {
            public const int Encdoed = 15;
            public const int Decoded = 4;
        }
    }
}
