using System.Collections;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public abstract class Schema
    {
        #region static
        public static Schema Create(MichelinePrim micheline) => micheline.Prim switch
        {
            PrimType.address => new AddressSchema(micheline),
            PrimType.big_map => new BigMapSchema(micheline),
            PrimType.bls12_381_fr => new Bls12381FrSchema(micheline),
            PrimType.bls12_381_g1 => new Bls12381G1Schema(micheline),
            PrimType.bls12_381_g2 => new Bls12381G2Schema(micheline),
            PrimType.@bool => new BoolSchema(micheline),
            PrimType.bytes => new BytesSchema(micheline),
            PrimType.chain_id => new ChainIdSchema(micheline),
            PrimType.chest_key => new ChestKeySchema(micheline),
            PrimType.chest => new ChestSchema(micheline),
            PrimType.constant => new ConstantSchema(micheline),
            PrimType.contract => new ContractSchema(micheline),
            PrimType.@int => new IntSchema(micheline),
            PrimType.key_hash => new KeyHashSchema(micheline),
            PrimType.key => new KeySchema(micheline),
            PrimType.lambda => new LambdaSchema(micheline),
            PrimType.list => new ListSchema(micheline),
            PrimType.map => new MapSchema(micheline),
            PrimType.mutez => new MutezSchema(micheline),
            PrimType.nat => new NatSchema(micheline),
            PrimType.never => new NeverSchema(micheline),
            PrimType.operation=> new OperationSchema(micheline),
            PrimType.option => new OptionSchema(micheline),
            PrimType.or => new OrSchema(micheline),
            PrimType.pair => new PairSchema(micheline),
            PrimType.parameter => new ParameterSchema(micheline),
            PrimType.sapling_state => new SaplingStateSchema(micheline),
            PrimType.sapling_transaction_deprecated => new SaplingTransactionDeprecatedSchema(micheline),
            PrimType.sapling_transaction => new SaplingTransactionSchema(micheline),
            PrimType.set => new SetSchema(micheline),
            PrimType.signature => new SignatureSchema(micheline),
            PrimType.storage => new StorageSchema(micheline),
            PrimType.@string => new StringSchema(micheline),
            PrimType.ticket => new TicketSchema(micheline),
            PrimType.timestamp => new TimestampSchema(micheline),
            PrimType.tx_rollup_l2_address => new TxRollupL2AddressSchema(micheline),
            PrimType.unit => new UnitSchema(micheline),
            PrimType.view => new ViewSchema(micheline),
            _ => throw new NotImplementedException($"Schema for prim {micheline.Prim} is not implemented")
        };
        #endregion

        public abstract PrimType Prim { get; }

        public string? Field { get; }
        public string? Type { get; }

        internal int Index = -1;
        internal string? Annot = null;

        internal string Suffix => Index > -1 ? $"_{Index}" : string.Empty;

        public virtual string? Name => (Annot ?? Prim.ToString()) + Suffix;
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
            using var mem = new MemoryStream();
            using var writer = new Utf8JsonWriter(mem, options);
            
            writer.WriteStartObject();
            writer.WritePropertyName($"schema:{Signature}");
            WriteValue(writer);
            writer.WriteEndObject();
            writer.Flush();

            return Utf8.Convert(mem.ToArray());
        }

        public string Humanize(IMicheline value, JsonWriterOptions options = default)
        {
            using var mem = new MemoryStream();
            using var writer = new Utf8JsonWriter(mem, options);
            
            WriteValue(writer, value);
            writer.Flush();

            return Utf8.Convert(mem.ToArray());
        }

        public string GetJsonSchema(JsonWriterOptions options = default)
        {
            using var mem = new MemoryStream();
            using var writer = new Utf8JsonWriter(mem, options);
            
            writer.WriteStartObject();
            writer.WriteString("$schema", "http://json-schema.org/draft/2019-09/schema#");
            WriteJsonSchema(writer);
            writer.WriteEndObject();
            writer.Flush();

            return Utf8.Convert(mem.ToArray());
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
                    if (!json.TryGetProperty(Name!, out var jsonProp))
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

        protected virtual List<IMicheline>? GetArgs()
        {
            return null;
        }

        protected List<IAnnotation>? GetAnnotations()
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
            writer.WritePropertyName(Name!);
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

        internal virtual TreeView GetTreeView(TreeView? parent, IMicheline value, string? name = null, Schema? schema = null)
        {
            return new TreeView
            {
                Name = name ?? Name!,
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
