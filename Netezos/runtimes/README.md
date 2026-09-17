# Native blst binaries

Prebuilt [blst](https://github.com/supranational/blst) libraries, loaded at runtime by
`Netezos.Utils.Blst` and shipped inside the NuGet package. They provide the BLS12-381
primitives behind tz4 keys and BLS signatures.

| RID | File | blst version | Built with | SHA-256 |
| --- | ---- | ------------ | ---------- | ------- |
| linux-arm64 | `libblst.so` | v0.3.14 | Ubuntu 22.04, gcc 11.4.0 | `3b9995fa313179df80070562c01db22918d99bf0c103544e0640c433a84cb5a2` |
| linux-x64 | `libblst.so` | unknown, v0.3.11–v0.3.14 | Ubuntu 22.04, GCC 11.4.0 | `b3a4c1594eb81a074eac32fec249772a8fd4964a35f70f826c7fd892f6f444d0` |
| osx-arm64 | `libblst.dylib` | unknown, v0.3.11–v0.3.14 | unknown | `fa5c19d740af1e3f0ed440267d08b3021700dda7673d8e8f0a4659ecf2a17fb0` |
| osx-x64 | `libblst.dylib` | unknown, v0.3.11–v0.3.14 | unknown | `0cad79621169ae5d0b1efa18f1433c5f8ecffd4db6ddbb855de2b89852b209c5` |
| win-x64 | `blst.dll` | unknown, v0.3.11–v0.3.14 | unknown | `19c7b0ec20678485f25dbbe4e240ffed76de477bac836c2af8f473a97b64631f` |

## On the unknown versions

Four of the five binaries were added in commit 78a7db5 (2025-06-03) with no record of where
they came from, and blst embeds no version string, so the exact version cannot be recovered
from the files themselves. What is established:

- They export 213 `blst_*` symbols, 37 of them from `blst_aux.h`. Four of those symbols do
  not exist before v0.3.11, which sets the lower bound.
- v0.3.15 was released 2025-06-06, three days after the files were committed, which sets the
  upper bound.
- `libblst.so` for linux-x64 carries a `GCC: (Ubuntu 11.4.0-1ubuntu1~22.04) 11.4.0` tag.

The range cannot be narrowed further: v0.3.12, v0.3.13 and v0.3.14 generate identical x86-64
code, differing only in armv8 assembly and `cpuid.c`.

Rebuilding the remaining four the same way as linux-arm64 would replace these unknowns with
recorded versions, and is worth doing.

## Rebuilding linux-arm64

Run from this directory; the result lands in `linux-arm64/native/libblst.so`. On a non-arm64
host this relies on Docker's QEMU emulation.

```sh
docker run --rm --platform linux/arm64 -v "$(pwd)/linux-arm64/native:/out" ubuntu:22.04 sh -c '
  set -e
  export DEBIAN_FRONTEND=noninteractive
  apt-get update -qq && apt-get install -y -qq gcc curl
  curl -sL https://github.com/supranational/blst/archive/refs/tags/v0.3.14.tar.gz | tar xz
  cd blst-0.3.14 && ./build.sh -shared
  cp libblst.so /out/
'
```

The source tarball is
`2d17ed3087bd37d2aff6fd37c83807831fcc62bcbbe71bb65d32d7ded5749faa`.

The rebuilt library is not expected to be byte-identical to the checked-in one, since neither
the toolchain packages nor blst's build are pinned to that level. Verify it by architecture,
by the exported symbol set, and by running the test suite on arm64 rather than by checksum.

## Verifying a binary

```sh
readelf -h linux-arm64/native/libblst.so | grep Machine          # AArch64
readelf --dyn-syms linux-arm64/native/libblst.so | grep -c blst_ # 213
```

The linux-arm64 binary shipped before v3.0.4 was in fact an x86-64 build, byte-identical to
the linux-x64 one, so every BLS operation threw `DllNotFoundException` on ARM64 Linux. Nothing
detected it because CI only ran on ubuntu-x64 and the file's name was the only thing saying
otherwise.
