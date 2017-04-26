using System;
using System.Collections.Generic;

namespace WikiGenerator
{
    public class Node
    {
        public string Name;
        public List<Node> Children;
        public Node Parent;

        public Node(string Name)
        {
            this.Name = Name;
            Children = new List<Node>();
        }

        public void Add(Node node, string[] path)
        {
            for (int i = 0; i < path.Length; i++)
                Console.Write(path[i] + ".");
            if (path.Length == 0)
            {
                Console.Write("\n");
                node.Parent = this;
                Children.Add(node);
                return;
            }
            string[] newPath = new string[path.Length - 1];
            for (int i = 1; i < path.Length; i++)
                newPath[i - 1] = path[i];
            string first = path[0];
            Node Child = Children.Find(x => x.Name == first);
            if(Child == null)
            {
                Child = new NameSpace(first)
                {
                    Parent = this
                };
                Children.Add(Child);
            }
            Child.Add(node, newPath);
        }

        public string GetPath()
        {
            string temp = "";
            if (Parent != null && Parent.Parent != null)
                temp += Parent.GetPath() + ".";
            temp += Name;
            return temp;
        }

        public virtual string GetContent()
        {
            return "";
        }

        public virtual void WritePage()
        {
            foreach (Node n in Children)
                n.WritePage();
        }

        public Node Root
        {
            get { return Parent == null ? this : Parent.Root; }
        }

        public bool IsPathPresent(string[] path)
        {
            if (path.Length == 0)
                return true;
            string[] newPath = new string[path.Length - 1];
            for (int i = 0; i < newPath.Length; i++)
                newPath[i] = path[i + 1];
            string first = path[0];
            Node n = Children.Find(x => x.Name == first);
            if (n == null)
                return false;
            return n.IsPathPresent(newPath);
        }
    }
}
