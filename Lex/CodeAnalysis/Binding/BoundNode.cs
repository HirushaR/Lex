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
            WriteProperties(writer.node);
            

            if(isToConsole)
                Console.ResetColor();

            writer.WriteLine();

            indent += isLast ? "   " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(writer, child, indent, child == lastChild);
        }

        private static void WriteProperties(object node)
        {
            throw new NotImplementedException();
        }

        private static void WriterNode(TextWriter writer, BoundNode node)
        {
            // TODO: Handle binary and unary operator
            // TODO: Change colors
            Console.ForegroundColor = GetColor(node);

            var text = GetText(node);
            writer.Write(text);
            
            Console.ResetColor();
        }

        private static object GetText(BoundNode node)
        {
            if (node is BoundBinaryExpression b)
                return b.Op.Kind.ToString() + "Expression";
            
            if (node is BoundUnaryExpression u)
                return u.op.Kind.ToString() + "Expression";
            
            return node.Kind.ToString();
        }

        private static ConsoleColor GetColor(BoundNode node)
        {
            if (node is BoundExpression)
                return ConsoleColor.Blue;
            
            if( node is BoundStatement)
                return ConsoleColor.Cyan;
            return ConsoleColor.Yellow;
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
