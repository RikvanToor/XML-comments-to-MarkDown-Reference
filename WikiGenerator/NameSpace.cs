using System.IO;

namespace WikiGenerator
{
    public class NameSpace : Node
    {
        public NameSpace(string Name) : base(Name)
        {

        }

        public override string GetContent()
        {
            string temp = "# " + Name + " *(namespace)* \n";
            temp += "## Members:\n";
            foreach(Node n in Children)
            {
                temp += "* ["+n.Name + "]("+n.GetPath()+")\n";
            }
            return temp;
        }

        public override void WritePage()
        {
            StreamWriter sw = new StreamWriter(Program.folder + GetPath() + ".md");
            sw.WriteLine(GetContent());
            sw.Close();
            base.WritePage();
        }
    }
}
