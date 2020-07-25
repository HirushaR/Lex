using lex.CodeAnalysis;
using lex.CodeAnalysis.Binding;
using lex.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lex
{
    internal static class Program
    {
        private static void Main()
        {
            bool showTree = false;
            while (true)
            {
                Console.WriteLine(">");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if (line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees" : "Not showing parse tree");
                    continue;
                }
                else if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                var color = Console.ForegroundColor;
                var parser = new Parser(line);
                var binder = new Binder();
                var syntaxTree = SyntexTree.Parse(line);
                var boundExpression = binder.BindExpression(syntaxTree.Root);
                var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();
     

               
            
                if (showTree)
                {

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ForegroundColor = color;
                }

                if (!diagnostics.Any())
                {
                    var e = new Evaluator(syntaxTree.Root);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach (var diagnostics in syntaxTree.Diagnostics)
                    {
                        Console.WriteLine(diagnostics);
                    }
                    Console.ForegroundColor = color;
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {

            // └──
            // │ 
            // ├─

            var marker = isLast ? "└──" : "├─";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);
            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }
            Console.WriteLine();

            // indent += "    ";
            indent += isLast ? "    " : "│   ";

            var lastChild = node.getChildern().LastOrDefault();


            foreach (var child in node.getChildern())
                PrettyPrint(child, indent, child == lastChild);
        }
    }
}
