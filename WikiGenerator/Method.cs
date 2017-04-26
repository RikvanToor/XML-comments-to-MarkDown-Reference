using System.Collections.Generic;

namespace WikiGenerator
{
    public class Method : ConcreteNode
    {
        List<Parameter> ParameterList;
        public Method(string Name, string summary, List<Parameter> paramList) : base(Name, summary)
        {
            ParameterList = paramList;
        }

        public override string GetContent()
        {
            string temp = "### :m: ";
            if (Name == "#ctor")
                temp += "*Constructor*";
            else
                temp += Name;
            temp += "(";
            foreach(Parameter p in ParameterList)
            {
                if (p.Name != "")
                    temp += p.Name;
                else
                    temp += p.Type;
                temp += ",";
            }
            if (ParameterList.Count > 0)
                temp = temp.Remove(temp.Length - 1,1);
            temp += ")\n";
            if (Summary.Length > 0 && Summary[0] == '\n')
                temp += Summary.Replace("      ","") + "\n";
            if (ParameterList.Count == 0)
                return temp;
            temp += "Name | Description \n";
            temp += "--- | --- \n";
            foreach(Parameter p in ParameterList)
            {
                temp += p.Name + " | *";
                if (Root.IsPathPresent(p.Type.Split('.')))
                    temp += "[" + p.Type + "](" + p.Type + ")";
                else
                    temp += p.Type;
                temp += "*<br/>" +  p.Summary + " \n";
            }
            return temp + "\n";
        }
    }
}
