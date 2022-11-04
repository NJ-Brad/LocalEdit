namespace LocalEdit.C4Types
{
    public class C4Workspace
    {
        public IEnumerable<C4Item> Model { get; set; } = new List<C4Item>();
        public IEnumerable<C4Relationship> Relationships { get; set; } = new List<C4Relationship>();

    }
}
