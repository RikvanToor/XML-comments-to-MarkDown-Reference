using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace WikiGenerator
{
    class Program
    {
        /// <summary>
        /// The path to the folder where you want your MarkDown files to be stored.
        /// </summary>
        public static string folder = "";

        /// <summary>
        /// Gets input file and output folder, reads the file, parses the root node.
        /// </summary>
        /// <param name="args">-</param>
        static void Main(string[] args)
        {
            Console.Write("Path to XML file: ");
            string filepath = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Path to folder where files should be stored: ");
            folder = Console.ReadLine();
            if (folder[folder.Length - 1] != '\\' && folder[folder.Length - 1] != '/')
                folder += "\\";
            XDocument xdoc = XDocument.Load(filepath);
            XElement root = xdoc.Root;
            var members = root.Descendants("members").First().Nodes();

            Node rootNode = new Node("Spoorloos");
            foreach(XElement member in members)
            {
                var name = member.Attribute("name").Value;

                string[] splitOnParams = name.Split('(');
                string[] totalPath = splitOnParams[0].Substring(2).Split('.');
                string shortName = totalPath[totalPath.Length - 1];
                string[] parameters = new string[splitOnParams.Length - 1];

                string[] pathEx = new string[totalPath.Length - 1];
                for (int i = 0; i < totalPath.Length - 1; i++)
                    pathEx[i] = totalPath[i];

                var asdf = member.Descendants(XName.Get("summary"));
                string type = name.Substring(0, 2);
                Node n = ParseNode(type, shortName, member);
                rootNode.Add(n, pathEx);
            }
            rootNode.WritePage();

            StreamWriter sw = new StreamWriter(folder + "home.md");
            sw.WriteLine("### This codebase contains the following namespaces:");
            foreach(Node n in rootNode.Children)
            {
                sw.WriteLine("* [" + n.Name + "](" + n.Name + ")");
            }
            sw.Close();
        }

        /// <summary>
        /// Creates a node of the proper type from data gotten out of the XML file
        /// </summary>
        /// <param name="type">The type of the member from the XML file. Should be "T:","M:" or "P:"</param>
        /// <param name="shortName">The name of the member, without any parameters or parentheses and without any parent classes or namespaces.</param>
        /// <param name="member">The actual XElement from the XML file.</param>
        /// <returns>A node of the proper type</returns>
        private static Node ParseNode(string type, string shortName, XElement member)
        {
            Node n;
            switch (type)
            {
                case "T:":
                    string summary = member.Descendants(XName.Get("summary")).First()?.Value;
                    n = new Class(shortName, summary);
                    return n;
                case "M:":
                    summary = member.Descendants(XName.Get("summary")).First()?.Value;
                    // Gets all parameters from the method member
                    var paramCol = member.Descendants(XName.Get("param"));
                    List<Parameter> paramList = new List<Parameter>();
                    //Gets the types of all the parameters from the name of the member.
                    string[] splitOnParams = member.Attribute("name").Value.Split('(');
                    
                    if (splitOnParams.Length > 1)
                    {
                        string paramsTotal = splitOnParams[1];
                        paramsTotal = paramsTotal.Substring(0, paramsTotal.Length - 1);
                        string[] types = paramsTotal.Split(',');
                        for (int i = 0; i < types.Length; i++)
                        {
                            string paramType = types[i];
                            Parameter p = ParseParameter(paramType, paramCol, i);
                            paramList.Add(p);
                        }
                    }
                    n = new Method(shortName, summary, paramList);
                    return n;
                case "P:":
                    summary = member.Descendants(XName.Get("summary")).First()?.Value;
                    n = new Property(shortName, summary);
                    return n;
                case "F:":
                    summary = member.Descendants(XName.Get("summary")).First()?.Value;
                    n = new Property(shortName, summary);
                    return n;
                default:
                    throw new Exception("Type " + type + " of the member is unknown");
            }
        }
        /// <summary>
        /// Gets parameters from a method XElement
        /// </summary>
        /// <param name="type"></param>
        /// <param name="paramCol"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        static Parameter ParseParameter(string type, IEnumerable<XElement> paramCol, int index)
        {
            string paramName = "";
            string paramSummary = "";
            try
            {
                XElement element = paramCol.ElementAt(index);
                paramName = element.Attribute("name").Value;
                paramSummary = element.Value;
            }
            catch { }
            return new Parameter(type, paramName, paramSummary);
        }
    }
}
