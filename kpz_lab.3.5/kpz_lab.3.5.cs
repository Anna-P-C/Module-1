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
            var row1 = new LightElementNode("tr", "block", "double");
            row1.AddChild(new LightTextNode("Видимий рядок"));

            var row2 = new LightElementNode("tr", "block", "double");
            row2.AddChild(new LightTextNode("Цей рядок ми приховаємо"));

            table.AddChild(row1);
            table.AddChild(row2);

            Console.WriteLine("--- До зміни стану ---");
            Console.WriteLine(table.Render());

       
            row2.SetState(new HiddenState());

            Console.WriteLine("\n--- Після зміни стану (row2 приховано) ---");
            Console.WriteLine(table.Render());

            Console.ReadKey();
        }
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

    public abstract class LightNode
    {
        public abstract string Render();
    }

    public class LightTextNode : LightNode
    {
        private string _text;
        public LightTextNode(string text) => _text = text;
        public override string Render() => _text;
    }

    public class LightElementNode : LightNode
    {
        private string _tagName;
        private List<LightNode> _children = new List<LightNode>();

       
        private INodeState _state = new VisibleState();

        public LightElementNode(string tag, string display, string closing)
        {
            _tagName = tag;
        }

        public void AddChild(LightNode node) => _children.Add(node);

     
        public void SetState(INodeState state) => _state = state;

        public override string Render()
        {
           
            if (!_state.IsVisible()) return "";

            StringBuilder sb = new StringBuilder();
            sb.Append($"<{_tagName}>");
            foreach (var child in _children) sb.Append(child.Render());
            sb.Append($"</{_tagName}>");
            return sb.ToString();
        }
    }
}