using System;
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
                case PrimType.big_map: return new BigMapSchema(micheline);
                case PrimType.@bool: return new BoolSchema(micheline);
                case PrimType.bytes: return new BytesSchema(micheline);
                case PrimType.chain_id: return new ChainIdSchema(micheline);
                case PrimType.contract: return new ContractSchema(micheline);
                case PrimType.@int: return new IntSchema(micheline);
                case PrimType.key: return new KeySchema(micheline);
                case PrimType.key_hash: return new KeyHashSchema(micheline);
                case PrimType.lambda: return new LambdaSchema(micheline);
                case PrimType.list: return new ListSchema(micheline);
                case PrimType.map: return new MapSchema(micheline);
                case PrimType.mutez: return new MutezSchema(micheline);
                case PrimType.nat: return new NatSchema(micheline);
                case PrimType.option: return new OptionSchema(micheline);
                case PrimType.or: return new OrSchema(micheline);
                case PrimType.pair: return new PairSchema(micheline);
                case PrimType.set: return new SetSchema(micheline);
                case PrimType.signature: return new SignatureSchema(micheline);
                case PrimType.@string: return new StringSchema(micheline);
                case PrimType.timestamp: return new TimestampSchema(micheline);
                case PrimType.unit: return new UnitSchema(micheline);
                default:
                    throw new NotImplementedException($"Schema for prim {micheline.Prim} is not implemented");
            }
        }
        #endregion

        public abstract PrimType Prim { get; }
        
        public string Field { get; }
        public string Type { get; }

        public virtual string Name => Field ?? Type ?? Prim.ToString();

        protected Schema(MichelinePrim micheline)
        {
            if (micheline.Annots?.Count > 0)
            {
                Field = micheline.Annots.FirstOrDefault(x => x.Type == AnnotationType.Field && x.Value.Length > 0)?.Value;
                Type = micheline.Annots.FirstOrDefault(x => x.Type == AnnotationType.Type && x.Value.Length > 0)?.Value;
            }
        }

        public string Humanize(JsonWriterOptions options = default)
        {
            using (var mem = new MemoryStream())
            using (var writer = new Utf8JsonWriter(mem, options))
            {
                WriteValue(writer);
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

        internal virtual void WriteProperty(Utf8JsonWriter writer)
        {
            writer.WritePropertyName(Name);
            WriteValue(writer);
        }

        internal virtual void WriteProperty(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WritePropertyName(Name);
            WriteValue(writer, value);
        }

        internal virtual void WriteValue(Utf8JsonWriter writer)
        {
            writer.WriteStringValue(Prim.ToString());
        }

        internal virtual void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }

        protected FormatException FormatException(IMicheline value)
        {
            var type = (value as MichelinePrim)?.Prim.ToString() ?? value.Type.ToString();
            return new FormatException($"Failed to map {type} into {Prim}");
        }
    }

    interface IFlat
    {
        string Flatten(IMicheline value);
    }
}
