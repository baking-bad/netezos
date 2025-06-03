using System.Numerics;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class SaplingStateSchema : Schema
    {
        public override PrimType Prim => PrimType.sapling_state;

        public override string Signature => $"sapling_state:{MemoSize}";

        public BigInteger MemoSize { get; }

        public SaplingStateSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || micheline.Args[0] is not MichelineInt micheInt)
                throw new FormatException($"Invalid {Prim} schema format");

            MemoSize = micheInt.Value;
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is MichelineInt micheInt)
            {
                writer.WriteNumberValue((long)micheInt.Value);
            }
            else if (value is MichelineArray micheArray)
            {
                if (micheArray.Count > 0)
                    throw new NotImplementedException("At the time of creation there was no any documentation on possible values of sapling_state :(");

                writer.WriteStartArray();
                writer.WriteEndArray();
            }
            else
            {
                throw FormatException(value);
            }
        }

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            writer.WriteStartArray("oneOf");
            {
                writer.WriteStartObject();
                writer.WriteString("type", "integer");
                writer.WriteString("$comment", "int");
                writer.WriteEndObject();

                writer.WriteStartObject();
                writer.WriteString("type", "array");
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteString("$comment", Prim.ToString());
        }

        protected override List<IMicheline> GetArgs()
        {
            return [new MichelineInt(MemoSize)];
        }
    }
}
