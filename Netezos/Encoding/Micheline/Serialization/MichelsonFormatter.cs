using System;
using System.Collections.Generic;
using System.Linq;

namespace Netezos.Encoding.Serialization
{
    public static class MichelsonFormatter
    {
        const int LineSize = 100;
        
        public static string MichelineToMichelson(IMicheline data, bool inline = false)
        {
            return FormatNode(data, inline:inline,  root:true);
        }

        static bool IsFramed(MichelinePrim prim)
        {
            switch (prim.Prim)
            {
                case PrimType.Pair:
                case PrimType.Left:
                case PrimType.Right:
                case PrimType.Some:
                case PrimType.Elt:
                case PrimType.pair:
                case PrimType.or:
                case PrimType.option:
                case PrimType.map:
                case PrimType.big_map:
                case PrimType.list:
                case PrimType.set:
                case PrimType.contract:
                case PrimType.lambda:
                case PrimType.sapling_state:
                case PrimType.sapling_transaction:
                case PrimType.ticket:
                    return true;
                default:
                    return prim.Annots?.Any(x => x.Type != AnnotationType.Variable) ?? false;
            }
        }
        
        static bool IsInline(MichelinePrim prim)
        {
            return prim.Prim == PrimType.PUSH;
        }

        static bool IsScript(MichelineArray node)
        {
            return node.Count == 3
                && node.Any(x => x is MichelinePrim p && p.Prim == PrimType.parameter)
                && node.Any(x => x is MichelinePrim p && p.Prim == PrimType.storage)
                && node.Any(x => x is MichelinePrim p && p.Prim == PrimType.code);
        }

        static string FormatNode(IMicheline node, string indent = "", bool inline = false, bool root = false, bool wrapped = false)
        {
            switch (node)
            {
                case MichelineArray array:
                {
                    var isScriptRoot = root && IsScript(array);
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
                    if (prim.Prim == PrimType.LAMBDA || (prim.Prim >= PrimType.IF && prim.Prim <= PrimType.IF_NONE))
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

                    if (IsFramed(prim) && !root && !wrapped)
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