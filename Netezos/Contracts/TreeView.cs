using Netezos.Encoding;

namespace Netezos.Contracts
{
    public class TreeView
    {
        public string Name { get; set; } = null!;
        public Schema Schema { get; set; } = null!;
        public IMicheline Value { get; set; } = null!;

        public TreeView? Parent { get; set; }
        public List<TreeView>? Children { get; set; }

        public string Path
        {
            get
            {
                if (Parent == null)
                    return string.Empty;

                if (Parent.Schema is OptionSchema)
                    return Parent.Path;

                var path = Parent.Path;
                return path.Length == 0 ? Name : $"{path}.{Name}";
            }
        }

        public IEnumerable<TreeView> Nodes()
        {
            yield return this;
            if (Children != null)
                foreach (var child in Children)
                    foreach (var item in child.Nodes())
                        yield return item;
        }

        public IEnumerable<TreeView> Leafs()
        {
            if (Children == null)
            {
                yield return this;
            }
            else
            {
                foreach (var child in Children)
                    foreach (var item in child.Leafs())
                        yield return item;
            }
        }

        public override string ToString() => $"{Path}: {Schema.Prim}[{Children?.Count}]";
    }
}
