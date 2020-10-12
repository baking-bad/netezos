using System;
using System.Security;

namespace Netezos
{
    static class StringExtension
    {
        public static SecureString Secure(this string plainString)
        {
            var secureString = new SecureString();

            foreach (var c in plainString ?? throw new NullReferenceException())
                secureString.AppendChar(c);

            return secureString;
        }
    }
}