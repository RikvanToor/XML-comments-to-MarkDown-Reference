namespace WikiGenerator
{
    public class Parameter
    {
        public string Type, Name, Summary;
        public Parameter(string type, string name, string summary)
        {
            this.Type = type;
            this.Name = name;
            this.Summary = summary;
        }
    }
}
