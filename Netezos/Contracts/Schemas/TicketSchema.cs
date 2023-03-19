using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class TicketSchema : Schema
    {
        public override PrimType Prim => PrimType.ticket;

        public PairSchema Data { get; }

        public TicketSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || micheline.Args[0] is not MichelinePrim type)
                throw new FormatException($"Invalid {Prim} schema format");

            type.Annots ??= new List<IAnnotation>(1);
            if (type.Annots.Count == 0)
                type.Annots.Add(new FieldAnnotation("data"));

            var data = new MichelinePrim
            {
                Prim = PrimType.pair,
                Args = new List<IMicheline>(2)
                {
                    new MichelinePrim
                    {
                        Prim = PrimType.address,
                        Annots = new List<IAnnotation>(1) { new FieldAnnotation("address") }
                    },
                    new MichelinePrim
                    {
                        Prim = PrimType.pair,
                        Args = new List<IMicheline>(2)
                        {
                            type,
                            new MichelinePrim
                            {
                                Prim = PrimType.nat,
                                Annots = new List<IAnnotation>(1) { new FieldAnnotation("amount") }
                            }
                        }
                    }
                }
            };

            Data = new PairSchema(data);
        }

        internal override TreeView GetTreeView(TreeView? parent, IMicheline value, string? name = null, Schema? schema = null)
        {
            if (value is not MichelinePrim { Prim: PrimType.Pair } prim)
                throw FormatException(value);

            return Data.GetTreeView(parent, value, name ?? Name, this);
        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            Data.WriteValue(writer);
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is not MichelinePrim { Prim: PrimType.Pair } prim)
                throw FormatException(value);

            Data.WriteValue(writer, prim);
        }

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            Data.WriteJsonSchema(writer, Prim.ToString());
        }

        protected override List<IMicheline> GetArgs()
        {
            var type = (Data.Right as PairSchema)!.Left;
            return new List<IMicheline>(1) { type.ToMicheline() };
        }

        protected override IMicheline MapValue(object value)
        {
            return Data.MapObject(value, true);
        }

        public override IMicheline Optimize(IMicheline value)
        {
            return Data.Optimize(value);
        }
    }
}
