using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3_Composite
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var table = new LightElementNode("table", "block", "double");
            table.AddClass("data-table");

            var tr = new LightElementNode("tr", "block", "double");

            var th1 = new LightElementNode("th", "inline", "double");
            th1.AddChild(new LightTextNode("Назва"));

            var th2 = new LightElementNode("th", "inline", "double");
            th2.AddChild(new LightTextNode("Ціна"));

            tr.AddChild(th1);
            tr.AddChild(th2);
            table.AddChild(tr);

            var row = new LightElementNode("tr", "block", "double");
            var td1 = new LightElementNode("td", "inline", "double");
            td1.AddChild(new LightTextNode("Яблуко"));

            var td2 = new LightElementNode("td", "inline", "double");
            td2.AddChild(new LightTextNode("30 грн"));

            row.AddChild(td1);
            row.AddChild(td2);
            table.AddChild(row);

            var img = new LightElementNode("img", "inline", "single");
            img.AddClass("product-image");
            table.AddChild(img);

            Console.WriteLine("Демонстрація OuterHTML ");
            Console.WriteLine(table.OuterHTML());

            Console.WriteLine("\nДемонстрація InnerHTML ");
            Console.WriteLine(table.InnerHTML());

            Console.ReadKey();
        }
    }

    public abstract class LightNode
    {
        public abstract string OuterHTML();
        public abstract string InnerHTML();
    }

    public class LightTextNode : LightNode
    {
        private string _text;
        public LightTextNode(string text) => _text = text;
        public override string OuterHTML() => _text;
        public override string InnerHTML() => _text;
    }

    public class LightElementNode : LightNode
    {
        private string _tagName;
        private string _displayType;
        private string _closingType;
        private List<string> _classes = new List<string>();
        private List<LightNode> _children = new List<LightNode>();

        public LightElementNode(string tag, string display, string closing)
        {
            _tagName = tag;
            _displayType = display;
            _closingType = closing;
        }

        public void AddClass(string className) => _classes.Add(className);
        public void AddChild(LightNode node) => _children.Add(node);

        public override string InnerHTML()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var child in _children)
            {
                sb.Append(child.OuterHTML());
            }
            return sb.ToString();
        }

        public override string OuterHTML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<{_tagName}");

            if (_classes.Count > 0)
            {
                sb.Append($" class=\"{string.Join(" ", _classes)}\"");
            }

            if (_closingType == "single")
            {
                sb.Append(" />");
            }
            else
            {
                sb.Append(">");
                sb.Append(InnerHTML());
                sb.Append($"</{_tagName}>");
            }

            return sb.ToString();
        }
    }
}
