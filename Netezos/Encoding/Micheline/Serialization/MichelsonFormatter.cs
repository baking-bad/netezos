using System;
using System.Collections.Generic;
using System.Linq;

namespace Netezos.Encoding.Serialization
{
    public static class MichelsonFormatter
    {
        const int LineSize = 100;
        
        static readonly HashSet<PrimType> FramedPrims = new HashSet<PrimType>
        {
            PrimType.Pair,
            PrimType.Left,
            PrimType.Right,
            PrimType.Some,
            PrimType.pair,
            PrimType.or,
            PrimType.option,
            PrimType.map,
            PrimType.big_map,
            PrimType.list,
            PrimType.set,
            PrimType.contract,
            PrimType.lambda
        };

        static readonly HashSet<PrimType> AnnotatedPrims = new HashSet<PrimType>
        {
            PrimType.key,
            PrimType.unit,
            PrimType.signature,
            PrimType.operation,
            PrimType.@int,
            PrimType.nat,
            PrimType.@string,
            PrimType.bytes,
            PrimType.mutez,
            PrimType.@bool,
            PrimType.key_hash,
            PrimType.timestamp,
            PrimType.address
        };

        static readonly HashSet<PrimType> IfPrims = new HashSet<PrimType>
        {
            PrimType.IF,
            PrimType.IF_CONS,
            PrimType.IF_LEFT,
            PrimType.IF_NONE
        };
        
        public static string MichelineToMichelson(IMicheline data, bool inline = false)
        {
            return FormatNode(data, inline:inline,  isRoot:true);
        }

        static bool IsFramed(MichelinePrim prim)
        {
            return FramedPrims.Contains(prim.Prim) || AnnotatedPrims.Contains(prim.Prim) && prim.Annots != null;
        }
        
        static bool IsInline(MichelinePrim prim)
        {
            return prim.Prim == PrimType.PUSH;
        }

        static bool IsScript(MichelineArray node)
        {
            if (node.Count != 3)
                return false;
            
            return node.Any(x => x is MichelinePrim p && p.Prim == PrimType.parameter)
                && node.Any(x => x is MichelinePrim p && p.Prim == PrimType.storage)
                && node.Any(x => x is MichelinePrim p && p.Prim == PrimType.code);
        }

        static string FormatNode(IMicheline node, string indent = "", bool inline = false, bool isRoot = false, bool wrapped = false)
        {
            switch (node)
            {
                case MichelineArray array:
                {
                    var isScriptRoot = isRoot && IsScript(array);
                    var seqIndent = isScriptRoot ? indent : $"{indent}{new string(' ', 2)}";
                    var items = array.Select(x => FormatNode(x, seqIndent, inline, wrapped: true)).ToList();
                    if (!items.Any())
                        return "{}";
                    
                    var length = indent.Length + items.Sum(x => x.Length) + 4;
                    var space = isScriptRoot ? "" : " ";
                    var seq = inline || length < LineSize
                        ? string.Join($"{space}; ", items)
                        : string.Join($"{space};\n{seqIndent}", items);
                    
                    return isScriptRoot ? seq : $"{{ {seq} }}";
                }
                case MichelinePrim prim:
                    var expr = $"{prim.Prim}{(prim.Annots != null ? $" {string.Join(" ", prim.Annots)}" : "")}";
                    var args = prim.Args ?? new List<IMicheline>();
                    if (prim.Prim == PrimType.LAMBDA || IfPrims.Contains(prim.Prim))
                    {
                        var argIndent = $"{indent}{new string(' ', 2)}";
                        var items = args.Select(x => FormatNode(x, argIndent, inline)).ToList();
                        var lenght = indent.Length + expr.Length + items.Sum(x => x.Length) + items.Count() + 1;
                        if (inline || lenght < LineSize)
                            expr = $"{expr} {string.Join(" ", items)}";
                        else
                        {
                            expr = $"{expr}\n{argIndent}{string.Join($"\n{argIndent}", items)}";
                        }
                    }
                    else if (args.Count == 1)
                    {
                        var argIndent = $"{indent}{new string(' ', expr.Length + 1)}";
                        expr = $"{expr} {FormatNode(args[0], argIndent, inline)}";
                    }
                    else if (args.Count > 1)
                    {
                        var argIndent = $"{indent}{new string(' ', 2)}";
                        var altIndent = $"{indent}{new string(' ', expr.Length + 2)}";
                        foreach (var arg in args)
                        {
                            var item = FormatNode(arg, argIndent, inline);
                            var lenght = indent.Length + expr.Length + item.Length + 1;
                            if (inline || IsInline(prim) || lenght < LineSize)
                            {
                                argIndent = altIndent;
                                expr = $"{expr} {item}";
                            }
                            else
                                expr = $"{expr}\n{argIndent}{item}";
                        }
                    }

                    if (IsFramed(prim) && !isRoot && !wrapped)
                        return $"({expr})";
                    else
                        return expr;
                case MichelineBytes bytes:
                    return $"0x{Hex.Convert(bytes.Value)}";
                case MichelineInt val:
                    return $"{val.Value}";
                case MichelineString str:
                    return str.Value;
                default:
                    throw new ArgumentException($"Invalid micheline type {node.Type}");
            }
        }
    }
}