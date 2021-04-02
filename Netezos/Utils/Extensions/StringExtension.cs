using System;
using System.Security;

namespace Netezos
{
    static class StringExtension
    {
        static readonly char[] AlphaNumeric = new char[123]
        {
            '_','_','_','_','_','_','_','_','_','_','_','_','_','_','_','_',
            '_','_','_','_','_','_','_','_','_','_','_','_','_','_','_','_',
            '_','_','_','_','_','_','_','_','_','_','_','_','_','_','_','_',
            '0','1','2','3','4','5','6','7','8','9','_','_','_','_','_','_',
            '_','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O',
            'P','Q','R','S','T','U','V','W','X','Y','Z','_','_','_','_','_',
            '_','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o',
            'p','q','r','s','t','u','v','w','x','y','z'
        };

        public static string ToAlphaNumeric(this string str)
        {
            var buf = new char[str.Length];
            var changed = false;

            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = str[i] <= 'z' ? AlphaNumeric[str[i]] : '_';
                if (buf[i] != str[i]) changed = true;
            }

            return changed ? new string(buf) : str;
        }

        public static SecureString Secure(this string plainString)
        {
            var secureString = new SecureString();

            foreach (var c in plainString ?? throw new NullReferenceException())
                secureString.AppendChar(c);

            return secureString;
        }
    }
}