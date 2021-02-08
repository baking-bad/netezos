using System;

namespace Netezos.Tests
{
    static class ArrayExtension
    {
        public static Span<T> GetSpan<T>(this T[] src, int start)
        {
            return GetSpan(src, start, (src?.Length ?? 0) - start);
        }

        public static Span<T> GetSpan<T>(this T[] src, int start, int length)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            else if (src.Length == 0)
                throw new ArgumentException(nameof(src));
            else if (start > src.Length)
                throw new ArgumentOutOfRangeException(nameof(start));
            else if (length < 0 || length > src.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new Span<T>(src, start, length);
        }

        public static ReadOnlySpan<T> GetReadOnlySpan<T>(this T[] src, int start)
        {
            return GetReadOnlySpan(src, start, (src?.Length ?? 0) - start);
        }

        public static ReadOnlySpan<T> GetReadOnlySpan<T>(this T[] src, int start, int length)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            else if (src.Length == 0)
                throw new ArgumentException(nameof(src));
            else if (start > src.Length)
                throw new ArgumentOutOfRangeException(nameof(start));
            else if (length < 0 || length > src.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new ReadOnlySpan<T>(src, start, length);
        }
    }
}