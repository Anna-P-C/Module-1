using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Lab3_Composite
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var table = new LightElementNode("table", "block", "double");
            var tr = new LightElementNode("tr", "block", "double");
            var th = new LightElementNode("th", "inline", "double");
            th.AddChild(new LightTextNode("Назва"));

            tr.AddChild(th);
            table.AddChild(tr);

            Console.WriteLine("--- Тест Visitor  ---");
            var visitor = new ElementCounterVisitor();
            table.Accept(visitor);

            Console.WriteLine($"Всього HTML-тегів знайдено: {visitor.Count}");

            Console.ReadKey();
        }
    }

    public interface IVisitor
    {
        void VisitTextNode(LightTextNode node);
        void VisitElementNode(LightElementNode node);
    }

    public class ElementCounterVisitor : IVisitor
    {
        public int Count { get; private set; }
        public void VisitTextNode(LightTextNode node) { }
        public void VisitElementNode(LightElementNode node) => Count++;
    }

    public abstract class LightNode
    {
        public abstract string Render();
        public abstract void Accept(IVisitor visitor);
    }

    public class LightTextNode : LightNode
    {
        private string _text;
        public LightTextNode(string text) => _text = text;
        public override string Render() => _text;
        public override void Accept(IVisitor visitor) => visitor.VisitTextNode(this);
    }

    public class LightElementNode : LightNode
    {
        private string _tagName;
        private List<LightNode> _children = new List<LightNode>();

        public LightElementNode(string tag, string display, string closing)
        {
            _tagName = tag;
        }

        public void AddChild(LightNode node) => _children.Add(node);

        public override string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<{_tagName}>");
            foreach (var child in _children) sb.Append(child.Render());
            sb.Append($"</{_tagName}>");
            return sb.ToString();
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitElementNode(this);
            foreach (var child in _children)
            {
                child.Accept(visitor);
            }
        }
    }
}