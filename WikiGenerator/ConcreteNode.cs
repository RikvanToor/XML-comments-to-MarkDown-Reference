namespace WikiGenerator
{
    public class ConcreteNode : Node
    {
        public string Summary;

        public ConcreteNode(string Name) : base(Name)
        {
        }

        public ConcreteNode(string Name, string summary) : base(Name)
        {
            Summary = summary;
        }
    }
}
