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

        private IEnumerable<(string Name,object Value)> GetProperties()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach(var property in properties)
            {
                if(property.Name == nameof(Kind))
                    continue;
                if(typeof(BoundNode).IsAssignableFrom(property.PropertyType) ||
                   typeof(IEnumerable<BoundNode>).IsAssignableFrom(property.PropertyType))
                    continue;

                var value = property.GetValue(this);
                if (value != null)
                    yield return (property.Name, value);
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

            if(isToConsole)
                Console.ForegroundColor = GetColor(node);
           
            var text = GetText(node);
            writer.Write(text);

            var isFirstProperty = true;

            foreach(var p in node.GetProperties())
            {
                if(isFirstProperty)
                    isFirstProperty = false;
                else
                {
                    if(isToConsole)
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    writer.Write(",");
                }
                 if(isToConsole)
                        Console.ForegroundColor = ConsoleColor.Yellow;    
                writer.Write(p.Name);

                if(isToConsole)
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                writer.Write(" = ");

                 if(isToConsole)
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                writer.Write(p.Value);
            }

            if(isToConsole)
                Console.ResetColor();

            writer.WriteLine();

            indent += isLast ? "   " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(writer, child, indent, child == lastChild);
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
