using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Netezos.Keys
{
    static class StringExtension
    {
        public static SecureString ToSecureString(this string plainString)
        {
            if (plainString == null)
                return null;
 
            var secureString = new SecureString();
            foreach (var c in plainString.ToCharArray())
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }
        
        public static string SecureStringToString(this SecureString value) {
            var valuePtr = IntPtr.Zero;
            try {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            } finally {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}