using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class TicketSchema : Schema
    {
        public override PrimType Prim => PrimType.ticket;

        public PairSchema Ticket { get; }

        public TicketSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || micheline.Args[0] is not MichelinePrim contentType)
                throw new FormatException($"Invalid {Prim} schema format");

            contentType.Annots ??= new List<IAnnotation>(1);
            if (contentType.Annots.Count == 0)
                contentType.Annots.Add(new FieldAnnotation("content"));

            var ticket = new MichelinePrim
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
                            contentType,
                            new MichelinePrim
                            {
                                Prim = PrimType.nat,
                                Annots = new List<IAnnotation>(1) { new FieldAnnotation("amount") }
                            }
                        }
                    }
                }
            };

            Ticket = new PairSchema(ticket);
        }

        internal override TreeView GetTreeView(TreeView? parent, IMicheline value, string? name = null, Schema? schema = null)
        {
            if (value is MichelinePrim { Prim: PrimType.Pair })
            {
                return Ticket.GetTreeView(parent, value, name ?? Name, this);
            }
            else if (value is MichelinePrim { Prim: PrimType.Ticket, Args.Count: 4 } ticket)
            {
                return Ticket.GetTreeView(parent, new MichelinePrim
                {
                    Prim = PrimType.Pair,
                    Args = [ticket.Args[0], ticket.Args[2], ticket.Args[3]]
                }, name ?? Name, this);
            }
            else
            {
                throw FormatException(value);
            }
        }

        internal override void WriteValue(Utf8JsonWriter writer)
        {
            Ticket.WriteValue(writer);
        }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            if (value is MichelinePrim { Prim: PrimType.Pair })
            {
                Ticket.WriteValue(writer, value);
            }
            else if (value is MichelinePrim { Prim: PrimType.Ticket, Args.Count: 4 } ticket)
            {
                Ticket.WriteValue(writer, new MichelinePrim
                {
                    Prim = PrimType.Pair,
                    Args = [ticket.Args[0], ticket.Args[2], ticket.Args[3]]
                });
            }
            else
            {
                throw FormatException(value);
            }
        }

        internal override void WriteJsonSchema(Utf8JsonWriter writer)
        {
            Ticket.WriteJsonSchema(writer, Prim.ToString());
        }

        protected override List<IMicheline> GetArgs()
        {
            var type = (Ticket.Right as PairSchema)!.Left;
            return new List<IMicheline>(1) { type.ToMicheline() };
        }

        protected override IMicheline MapValue(object? value)
        {
            return Ticket.MapObject(value, true);
        }

        public override IMicheline Optimize(IMicheline value)
        {
            if (value is MichelinePrim { Prim: PrimType.Ticket, Args.Count: 4 } ticket)
            {
                return Ticket.Optimize(new MichelinePrim
                {
                    Prim = PrimType.Pair,
                    Args = [ticket.Args[0], ticket.Args[2], ticket.Args[3]]
                });
            }
            return Ticket.Optimize(value);
        }
    }
}
