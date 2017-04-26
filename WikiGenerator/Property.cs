namespace WikiGenerator
{
    public class Property : ConcreteNode
    {
        public Property(string Name, string summary) : base(Name, summary)
        {

        }

        public override string GetContent()
        {
            string temp = "### :parking: " + Name + "\n";
            temp += Summary.Replace("      ","") + "\n";
            return temp;
        }
    }
}
