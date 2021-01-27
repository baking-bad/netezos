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
        static readonly HashSet<PrimType> IfPrimTypes = new HashSet<PrimType>
        {
            PrimType.IF,
            PrimType.IF_CONS,
            PrimType.IF_LEFT,
            PrimType.IF_NONE
        };
        static readonly HashSet<string> WrapPrimTypes = new HashSet<string>
        {
            "Left",
            "Right",
            "Some",
            "Pair"
        };
        
        public static string MichelineToMichelson(IMicheline data, bool inLine = false, bool wrap = false)
        {
            var res = FormatNode(data, inLine:inLine,  isRoot:true);
            return wrap && WrapPrimTypes.Any(x => res.StartsWith(x)) ? $"({res})" : res;
        }

        static bool IsFramed(MichelinePrim prim)
        {
            if (FramedPrims.Contains(prim.Prim))
                return true;
            if (AnnotatedPrims.Contains(prim.Prim))
                return prim.Annots != null;

            return false;
        }
        
        static bool IsInline(MichelinePrim prim)
        {
            return prim.Prim == PrimType.PUSH;
        }
        
        static bool ForceNewLine(MichelinePrim prim)
        {
            return prim.Prim == PrimType.or;
        }

        static bool IsScript(MichelineArray node)
        {
            if (node.Count != 3)
                return false;
            
            return node.Any(x => x is MichelinePrim p && p.Prim == PrimType.parameter)
                   && node.Any(x => x is MichelinePrim p && p.Prim == PrimType.storage)
                   && node.Any(x => x is MichelinePrim p && p.Prim == PrimType.code);
        }

        static string FormatNode(IMicheline node, string indent = "", bool inLine = false, bool isRoot = false, bool wrapped = false)
        {
            switch (node)
            {
                case MichelineArray array:
                {
                    var isScriptRoot = isRoot && IsScript(array);
                    var seqIndent = isScriptRoot ? indent : $"{indent}{new string(' ', 2)}";
                    var items = array.Select(x => FormatNode(x, seqIndent, inLine, wrapped: true)).ToList();
                    if (!items.Any())
                        return "{}";
                    
                    var length = indent.Length + items.Sum(x => x.Length) + 4;
                    var space = isScriptRoot ? "" : " ";
                    var seq = inLine || length < LineSize
                        ? string.Join($"{space}; ", items)
                        : string.Join($"{space};\n{seqIndent}", items);
                    
                    return isScriptRoot ? seq : $"{{ {seq} }}";
                }
                case MichelinePrim prim:
                    var expr = $"{prim.Prim}{(prim.Annots != null ? $" {string.Join(" ", prim.Annots)}" : "")}";
                    var args = prim.Args != null && prim.Args.Any() ? prim.Args : new List<IMicheline>();
                    if (prim.Prim == PrimType.LAMBDA || IfPrimTypes.Contains(prim.Prim))
                    {
                        var argIndent = $"{indent}{new string(' ', 2)}";
                        var items = args.Select(x => FormatNode(x, argIndent, inLine)).ToList();
                        var lenght = indent.Length + expr.Length + items.Sum(x => x.Length) + items.Count() + 1;
                        if (inLine || lenght < LineSize)
                            expr = $"{expr} {string.Join(" ", items)}";
                        else
                        {
                            expr = $"{expr}\n{argIndent}{string.Join($"\n{argIndent}", items)}";
                        }
                    }
                    else if (args.Count() == 1)
                    {
                        var argIndent = $"{indent}{new string(' ', expr.Length + 1)}";
                        expr = $"{expr} {FormatNode((IMicheline)args[0], argIndent, inLine)}";
                    }
                    else if (args.Count() > 1)
                    {
                        var argIndent = $"{indent}{new string(' ', 2)}";
                        var altIndent = $"{indent}{new string(' ', expr.Length + 2)}";
                        foreach (var arg in args)
                        {
                            var item = FormatNode((IMicheline)arg, argIndent, inLine);
                            var lenght = indent.Length + expr.Length + item.Length + 1;
                            if ((inLine || IsInline(prim) || lenght < LineSize) && !ForceNewLine(prim))
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
                    return $"0x{bytes.Value}";
                case MichelineInt val:
                    return val.Value.ToString();
                case MichelineString str:
                    return str.Value;
                default:
                    throw new ArgumentException($"Unexpected node {node}");
            }
        }
    }
}