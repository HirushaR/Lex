using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Lex.CodeAnalysis.Binding
{
    internal abstract class BoundNode
    {
        public abstract BoundNodeKind Kind { get; }    

        public IEnumerable<BoundNode> GetChildren()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach(var property in properties)
            {
                if(typeof(BoundNode).IsAssignableFrom(property.PropertyType))
                {
                    var child = (BoundNode)property.GetValue(this);
                    if (child != null)
                        yield return child;
                }
                else if (typeof(IEnumerable<BoundNode>).IsAssignableFrom(property.PropertyType))
                {
                    var childern = (IEnumerable<BoundNode>)property.GetValue(this);
                    foreach(var child in childern)
                    {
                        if (child != null)
                            yield return child;
                    }
                    
                }
            }
        }

        public void WriteTo(TextWriter writer)
        {
            PrettyPrint(writer,this);
        }
        private static void PrettyPrint(TextWriter writer, BoundNode node, string indent = "", bool isLast = true)
        {
            var isToConsole = writer == Console.Out;

            var marker = isLast ? "└──" : "├──";

            if(isToConsole)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            
            writer.Write(indent);
            writer.Write(marker);


            WriterNode(writer,node);
            

            if(isToConsole)
                Console.ResetColor();

            writer.WriteLine();

            indent += isLast ? "   " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(writer, child, indent, child == lastChild);
        }

        private static void WriterNode(TextWriter writer, BoundNode node)
        {
            // TODO: Handle binary and unary operator
            // TODO: Change colors
            writer.Write(node.Kind);
        }

        public override string ToString() 
        {
            using (var writer = new StringWriter())
            {
                WriteTo(writer);
                return writer.ToString();
            }
        }    
    }
}
