using System.Linq;
using System.IO;

namespace WikiGenerator
{
    public class Class : ConcreteNode
    {
        public Class(string Name, string summary) : base(Name, summary)
        {

        }

        public override string GetContent()
        {
            string temp = "# *";

            string[] path = GetPath().Split('.');
            for(int i = 0; i < path.Length - 1; i++)
            {
                temp += "[" + path[i] + "](";
                for (int j = 0; j < i; j++)
                    temp += path[j] + ".";
                temp += path[i] + ").";
            }
            temp += "***" + Name +"**\n";

            return temp;
        }

        public override void WritePage()
        {
            Children.OrderBy(x => x is Method);
            StreamWriter sw = new StreamWriter(Program.folder + GetPath() + ".md");
            sw.Write(GetContent());
            foreach(Node n in Children)
            {
                sw.Write(n.GetContent());
            }
            sw.Close();
        }
    }
}
