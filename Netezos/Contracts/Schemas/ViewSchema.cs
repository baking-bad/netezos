﻿using System;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class ViewSchema : Schema
    {
        public override PrimType Prim => PrimType.view;

        public ViewSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteProperty(Utf8JsonWriter writer, IMicheline value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        public override IMicheline MapObject(object obj, bool isValue = false)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");

        protected override IMicheline MapValue(object value)
            => throw new InvalidOperationException($"Value of type {Prim} is not allowed");
    }
}
