using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Netezos.Utils
{
    internal static partial class Blst
    {
        static Blst()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), LoadLibrary);
        }

        static nint LoadLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == "blst")
            {
                var (os, prefix, ext) = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? ("win", "", "dll")
                    : RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                        ? ("osx", "lib", "dylib")
                        : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                            ? ("linux", "lib", "so")
                            : throw new PlatformNotSupportedException();

                var arch = RuntimeInformation.OSArchitecture switch
                {
                    Architecture.Arm64 => "arm64",
                    Architecture.X64 => "x64",
                    _ => throw new PlatformNotSupportedException()
                };

                libraryName = Path.Combine("runtimes", $"{os}-{arch}", "native", $"{prefix}{libraryName}.{ext}");
            }
            return NativeLibrary.Load(libraryName, assembly, searchPath);
        }

        public enum ERROR
        {
            SUCCESS = 0,
            BAD_ENCODING,
            POINT_NOT_ON_CURVE,
            POINT_NOT_IN_GROUP,
            AGGR_TYPE_MISMATCH,
            VERIFY_FAIL,
            PK_IS_INFINITY,
            BAD_SCALAR,
        }

        public class Exception(ERROR code) : ApplicationException
        {
            public override string Message => code switch
            {
                ERROR.BAD_ENCODING => "bad encoding",
                ERROR.POINT_NOT_ON_CURVE => "point not on curve",
                ERROR.POINT_NOT_IN_GROUP => "point not in group",
                ERROR.AGGR_TYPE_MISMATCH => "aggregate type mismatch",
                ERROR.VERIFY_FAIL => "verify failure",
                ERROR.PK_IS_INFINITY => "public key is infinity",
                ERROR.BAD_SCALAR => "bad scalar",
                _ => "unknown error",
            };
        }

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void blst_keygen(
            Span<byte> key,
            ReadOnlySpan<byte> IKM, nuint IKM_len,
            ReadOnlySpan<byte> info, nuint info_len);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void blst_sk_to_pk_in_g1(
            Span<long> ret,
            ReadOnlySpan<byte> SK);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void blst_sign_pk_in_g1(
            Span<long> ret,
            ReadOnlySpan<long> hash,
            ReadOnlySpan<byte> SK);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void blst_hash_to_g2(
            Span<long> ret,
            ReadOnlySpan<byte> msg, nuint msg_len,
            ReadOnlySpan<byte> dst, nuint dst_len,
            ReadOnlySpan<byte> aug, nuint aug_len);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial nuint blst_p1_sizeof();

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void blst_p1_compress(
            Span<byte> ret,
            ReadOnlySpan<long> inp);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial ERROR blst_p1_uncompress(
            Span<long> ret,
            ReadOnlySpan<byte> inp);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial nuint blst_p2_sizeof();

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial nuint blst_p1_affine_sizeof();

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial nuint blst_p2_affine_sizeof();

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial nuint blst_pairing_sizeof();

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void blst_p2_affine_compress(
            Span<byte> ret,
            ReadOnlySpan<long> inp);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial ERROR blst_p2_uncompress(
            Span<long> ret,
            ReadOnlySpan<byte> inp);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void blst_pairing_init(
            Span<long> ctx,
            [MarshalAs(UnmanagedType.Bool)] bool hash_or_encode,
            ref long dst, nuint dst_len);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void blst_pairing_commit(
            Span<long> ctx);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool blst_pairing_finalverify(
            ReadOnlySpan<long> ctx,
            ReadOnlySpan<long> sig);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial ERROR blst_pairing_chk_n_aggr_pk_in_g1(
            Span<long> ctx,
            ReadOnlySpan<long> pk, [MarshalAs(UnmanagedType.Bool)] bool pk_grpchk,
            ReadOnlySpan<long> sig, [MarshalAs(UnmanagedType.Bool)] bool sig_grpchk,
            ReadOnlySpan<byte> msg, nuint msg_len,
            ReadOnlySpan<byte> aug, nuint aug_len);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void blst_derive_master_eip2333(
            Span<byte> key,
            ReadOnlySpan<byte> IKM, nuint IKM_len);

        [LibraryImport("blst")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        public static partial void blst_derive_child_eip2333(
            Span<byte> key,
            ReadOnlySpan<byte> master,
            uint child_index);
    }
}
