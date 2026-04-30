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
            var row1 = new LightElementNode("tr", "block", "double");
            row1.AddChild(new LightTextNode("Видимий рядок"));

            var tr = new LightElementNode("tr", "block", "double");
            var th1 = new LightElementNode("th", "inline", "double");
            th1.AddChild(new LightTextNode("Назва"));
            var th2 = new LightElementNode("th", "inline", "double");
            th2.AddChild(new LightTextNode("Ціна"));

       
            row2.SetState(new HiddenState());

            var row = new LightElementNode("tr", "block", "double");
            var td1 = new LightElementNode("td", "inline", "double");
            td1.AddChild(new LightTextNode("Яблуко"));
            var td2 = new LightElementNode("td", "inline", "double");
            td2.AddChild(new LightTextNode("30 грн"));

            row.AddChild(td1);
            row.AddChild(td2);
            table.AddChild(row);

            Console.WriteLine("--- Command Pattern ---");
            var command = new AddClassCommand(table, "interactive-table");
            command.Execute();

            Console.WriteLine("\n--- State Pattern ---");
            row.SetState(new HiddenState());
            Console.WriteLine("Рядок з 'Яблуком' тепер прихований через State.");

            Console.WriteLine("\n--- Render ---");
            Console.WriteLine(table.Render());

            Console.WriteLine("\n--- Iterator Pattern ---");
            foreach (var node in table)
            {
                Console.WriteLine("Вузол знайдено");
            }

            Console.WriteLine("\n--- Visitor Pattern ---");
            var visitor = new ElementCounterVisitor();
            table.Accept(visitor);
            Console.WriteLine($"Кількість тегів: {visitor.Count}");

    public class HiddenState : INodeState
    {
        public bool IsVisible() => false;
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

    public interface INodeState
    {
        bool IsVisible();
    }

    public class VisibleState : INodeState
    {
        public bool IsVisible() => true;
    }

    public class HiddenState : INodeState
    {
        public bool IsVisible() => false;
    }

    public interface ICommand
    {
        void Execute();
    }

    public class AddClassCommand : ICommand
    {
        private LightElementNode _node;
        private string _className;
        public AddClassCommand(LightElementNode node, string className)
        {
            _node = node;
            _className = className;
        }
        public void Execute() => _node.AddClass(_className);
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
        public string Render()
        {
            OnBeforeRender();
            string result = ExecuteRender();
            OnAfterRender();
            return result;
        }

        protected abstract string ExecuteRender();
        protected virtual void OnBeforeRender() { }
        protected virtual void OnAfterRender() { }
        public abstract void Accept(IVisitor visitor);
    }

    public class LightTextNode : LightNode
    {
        private string _text;
        public LightTextNode(string text) => _text = text;
        protected override string ExecuteRender() => _text;
        public override void Accept(IVisitor visitor) => visitor.VisitTextNode(this);
    }

    public class LightElementNode : LightNode, IEnumerable<LightNode>
    {
        private string _tagName;
        private List<LightNode> _children = new List<LightNode>();
        private INodeState _state = new VisibleState();

       
        private INodeState _state = new VisibleState();

        public LightElementNode(string tag, string display, string closing)
        {
            _tagName = tag;
        }

        public void AddChild(LightNode node) => _children.Add(node);
        public void SetState(INodeState state) => _state = state;

        protected override string ExecuteRender()
        {
            if (!_state.IsVisible()) return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append($"<{_tagName}");

            if (_classes.Count > 0)
                sb.Append($" class=\"{string.Join(" ", _classes)}\"");

            if (_closingType == "single")
            {
                sb.Append(" />");
            }
            else
            {
                sb.Append(">");
                foreach (var child in _children)
                    sb.Append(child.Render());
                sb.Append($"</{_tagName}>");
            }

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
                child.Accept(visitor);
        }

        public IEnumerator<LightNode> GetEnumerator()
        {
            yield return this;
            foreach (var child in _children)
            {
                if (child is LightElementNode element)
                {
                    foreach (var sub in element) yield return sub;
                }
                else yield return child;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}