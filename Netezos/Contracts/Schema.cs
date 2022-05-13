﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public abstract class Schema
    {
        #region static
        public static Schema Create(MichelinePrim micheline)
        {
            switch (micheline.Prim)
            {
                case PrimType.address: return new AddressSchema(micheline);
                case PrimType.bls12_381_fr: return new Bls12381FrSchema(micheline);
                case PrimType.bls12_381_g1: return new Bls12381G1Schema(micheline);
                case PrimType.bls12_381_g2: return new Bls12381G2Schema(micheline);
                case PrimType.big_map: return new BigMapSchema(micheline);
                case PrimType.@bool: return new BoolSchema(micheline);
                case PrimType.bytes: return new BytesSchema(micheline);
                case PrimType.chest: return new ChestSchema(micheline);
                case PrimType.chest_key: return new ChestKeySchema(micheline);
                case PrimType.chain_id: return new ChainIdSchema(micheline);
                case PrimType.constant: return new ConstantSchema(micheline);
                case PrimType.contract: return new ContractSchema(micheline);
                case PrimType.@int: return new IntSchema(micheline);
                case PrimType.key: return new KeySchema(micheline);
                case PrimType.key_hash: return new KeyHashSchema(micheline);
                case PrimType.lambda: return new LambdaSchema(micheline);
                case PrimType.list: return new ListSchema(micheline);
                case PrimType.map: return new MapSchema(micheline);
                case PrimType.mutez: return new MutezSchema(micheline);
                case PrimType.nat: return new NatSchema(micheline);
                case PrimType.never: return new NeverSchema(micheline);
                case PrimType.option: return new OptionSchema(micheline);
                case PrimType.or: return new OrSchema(micheline);
                case PrimType.pair: return new PairSchema(micheline);
                case PrimType.sapling_state: return new SaplingStateSchema(micheline);
                case PrimType.sapling_transaction: return new SaplingTransactionSchema(micheline);
                case PrimType.sapling_transaction_deprecated: return new SaplingTransactionDeprecatedSchema(micheline);
                case PrimType.set: return new SetSchema(micheline);
                case PrimType.signature: return new SignatureSchema(micheline);
                case PrimType.@string: return new StringSchema(micheline);
                case PrimType.ticket: return new TicketSchema(micheline);
                case PrimType.timestamp: return new TimestampSchema(micheline);
                case PrimType.tx_rollup_l2_address: return new TxRollupL2AddressSchema(micheline);
                case PrimType.unit: return new UnitSchema(micheline);
                case PrimType.operation: return new OperationSchema(micheline);
                case PrimType.view: return new ViewSchema(micheline);
                default:
                    throw new NotImplementedException($"Schema for prim {micheline.Prim} is not implemented");
            }
        }
        #endregion

        public abstract PrimType Prim { get; }

        public string Field { get; }
        public string Type { get; }

        internal int Index = -1;
        internal string Annot = null;

        internal string Suffix => Index > -1 ? $"_{Index}" : string.Empty;

        public virtual string Name => (Annot ?? Prim.ToString()) + Suffix;
        public virtual string Signature => Prim.ToString();

        protected Schema(MichelinePrim micheline)
        {
            if (micheline.Annots?.Count > 0)
            {
                Field = micheline.Annots.FirstOrDefault(x => x.Type == AnnotationType.Field && x.Value.Length > 0)?.Value;
                Type = micheline.Annots.FirstOrDefault(x => x.Type == AnnotationType.Type && x.Value.Length > 0)?.Value;
                Annot = (Field ?? Type)?.ToAlphaNumeric();
            }
        }

        public TreeView ToTreeView(IMicheline value)
        {
            return GetTreeView(null, value);
        }

        public string Humanize(JsonWriterOptions options = default)
        {
            using (var mem = new MemoryStream())
            using (var writer = new Utf8JsonWriter(mem, options))
            {
                writer.WriteStartObject();
                writer.WritePropertyName($"schema:{Signature}");
                WriteValue(writer);
                writer.WriteEndObject();
                writer.Flush();

                return Utf8.Convert(mem.ToArray());
            }
        }

        public string Humanize(IMicheline value, JsonWriterOptions options = default)
        {
            using (var mem = new MemoryStream())
            using (var writer = new Utf8JsonWriter(mem, options))
            {
                WriteValue(writer, value);
                writer.Flush();

                return Utf8.Convert(mem.ToArray());
            }
        }

        public string GetJsonSchema(JsonWriterOptions options = default)
        {
            using (var mem = new MemoryStream())
            using (var writer = new Utf8JsonWriter(mem, options))
            {
                writer.WriteStartObject();
                writer.WriteString("$schema", "http://json-schema.org/draft/2019-09/schema#");
                WriteJsonSchema(writer);
                writer.WriteEndObject();
                writer.Flush();

                return Utf8.Convert(mem.ToArray());
            }
        }

        public IMicheline ToMicheline()
        {
            return new MichelinePrim
            {
                Prim = Prim,
                Args = GetArgs(),
                Annots = GetAnnotations()
            };
        }

        public virtual IMicheline Optimize(IMicheline value)
        {
            return value;
        }

        public virtual IMicheline MapObject(object obj, bool isValue = false)
        {
            if (isValue)
                return MapValue(obj);

            switch (obj)
            {
                case IEnumerator enumerator:
                    if (!enumerator.MoveNext())
                        throw MapFailedException($"enumerable is over");
                    return MapValue(enumerator.Current);
                case IEnumerable enumerable:
                    var e = enumerable.GetEnumerator();
                    if (!e.MoveNext())
                        throw MapFailedException($"enumerable is empty");
                    return MapValue(e.Current);
                case JsonElement json:
                    if (!json.TryGetProperty(Name, out var jsonProp))
                        throw MapFailedException($"no such property");
                    return MapValue(jsonProp);
                default:
                    var prop = obj?.GetType()?.GetProperty(Name)
                        ?? throw MapFailedException($"no such property");
                    return MapValue(prop.GetValue(obj));
            }
        }

        protected virtual IMicheline MapValue(object value)
        {
            throw new NotImplementedException();
        }

        protected virtual List<IMicheline> GetArgs()
        {
            return null;
        }

        protected List<IAnnotation> GetAnnotations()
        {
            if (Type != null)
            {
                return Field != null
                    ? new List<IAnnotation>(2) { new TypeAnnotation(Type), new FieldAnnotation(Field) }
                    : new List<IAnnotation>(1) { new TypeAnnotation(Type) };
            }
            else
            {
                return Field != null
                    ? new List<IAnnotation>(1) { new FieldAnnotation(Field) }
                    : null;
            }
        }

        internal virtual void WriteProperty(Utf8JsonWriter writer)
        {
            writer.WritePropertyName($"{Name}:{Signature}");
            WriteValue(writer);
        }

        internal virtual void WriteValue(Utf8JsonWriter writer)
        {
            writer.WriteStringValue(Prim.ToString());
        }

        internal virtual void WriteProperty(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WritePropertyName(Name);
            WriteValue(writer, value);
        }

        internal virtual void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }

        internal virtual void WriteJsonSchema(Utf8JsonWriter writer)
        {
            writer.WriteString("type", "string");
            writer.WriteString("$comment", Prim.ToString());
        }

        internal virtual TreeView GetTreeView(TreeView parent, IMicheline value, string name = null, Schema schema = null)
        {
            return new TreeView
            {
                Name = name ?? Name,
                Schema = schema ?? this,
                Value = value,
                Parent = parent
            };
        }

        protected FormatException FormatException(IMicheline value)
        {
            var type = (value as MichelinePrim)?.Prim.ToString() ?? value.Type.ToString();
            return new FormatException($"Failed to map {type} into {Prim}");
        }

        protected FormatException MapFailedException(string message)
        {
            return new FormatException($"Failed to map {Name}: {message}");
        }
    }

    interface IFlat
    {
        string Flatten(IMicheline value);
    }
}
