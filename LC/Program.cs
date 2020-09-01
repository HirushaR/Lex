using System;
using Lex.CodeAnalysis;
using Lex.CodeAnalysis.Binding;
using Lex.CodeAnalysis.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lex.CodeAnalysis.Text;

namespace Lex
{
    internal class LexRepl : Repl
    {

    }
    internal class Repl
    {
        private StringBuilder _inputBuilder;
        private Compilation _previous;
        public void Run()
        {
            var showTree = false;
            var showProgram = false;
            var variables = new Dictionary<VariableSymble, object>();
            var textBuilder = new StringBuilder();
            

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (textBuilder.Length == 0)
                    Console.Write("» ");
                else
                    Console.Write("·");

                Console.ResetColor();

                var input = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);


                if (textBuilder.Length == 0)
                {
                    if (isBlank)
                        break;
                    else if (input == "#showTree")
                    {
                        showTree = !showTree;
                        Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees");
                        continue;
                    }
                    else if (input == "#showProgram")
                    {
                        showProgram = !showProgram;
                        Console.WriteLine(showProgram ? "Showing Bound trees." : "Not showing Bound trees");
                        continue;
                    }
                    else if (input == "#cls")
                    {
                        Console.Clear();
                        continue;
                    }
                    else if (input == "#reset")
                    {
                        _previous = null;
                        variables.Clear();
                        continue;
                    }
                }

                textBuilder.AppendLine(input);
                var text = textBuilder.ToString();

                if (IsCompleteSubmition(text))
                    continue;

                Evaluate(showTree, showProgram, variables, text);
                textBuilder.Clear();
            }
        }

        private void Evaluate(bool showTree, bool showProgram, Dictionary<VariableSymble, object> variables, string text)
        {
            var syntaxTree = SyntaxTree.Parse(text);


            var compilation = _previous == null
                                ? new Compilation(syntaxTree)
                                : _previous.ContinueWith(syntaxTree);



            if (showTree)
            {
                syntaxTree.Root.WriteTo(Console.Out);
            }
            if (showProgram)
            {
                compilation.EmitTree(Console.Out);

            }
            var result = compilation.Evaluate(variables);

            if (!result.Diagnostics.Any())
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(result.Value);
                Console.ResetColor();
                _previous = compilation;
            }
            else
            {


                foreach (var diagnostic in result.Diagnostics)
                {
                    var lineIndex = syntaxTree.Text.GetLineIndex(diagnostic.Span.Start);
                    var lineNumber = lineIndex + 1;
                    var line = syntaxTree.Text.Lines[lineIndex];
                    var character = diagnostic.Span.Start - line.Start + 1;

                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($"({lineNumber}, {character}) : ");
                    Console.WriteLine(diagnostic);
                    Console.ResetColor();

                    var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
                    var suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.end);

                    var prefix = syntaxTree.Text.ToString(prefixSpan);
                    var error = syntaxTree.Text.ToString(diagnostic.Span);
                    var suffix = syntaxTree.Text.ToString(suffixSpan);

                    Console.Write("    ");
                    Console.Write(prefix);

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(error);
                    Console.ResetColor();

                    Console.Write(suffix);

                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        protected bool IsCompleteSubmition(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            
            var syntaxTree = SyntaxTree.Parse(text);

            if(syntaxTree.Diagnostics.Any())
                return false;
            
            return true;

        }
    
    }
    internal static class Program
    {
        private static void Main()
        {
           var repl = new LexRepl();
           repl.Run();
         
        }
  
    }
}
