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
        private StringBuilder _textBuilder;
        private Compilation _previous;
        private bool _showTree;
        private bool _showProgram;
        private Dictionary<VariableSymble, object> _variables;
        public void Run()
        {
    
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (_textBuilder.Length == 0)
                    Console.Write("» ");
                else
                    Console.Write("·");

                Console.ResetColor();

                var input = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);


                if (_textBuilder.Length == 0)
                {
                    if (isBlank)
                        break;

                    else if (input.StartsWith("#"))
                    {
                        EvaluateMetaCommand(input);
                        continue;
                    }
                  
                }

                _textBuilder.AppendLine(input);
                var text = _textBuilder.ToString();

                if (IsCompleteSubmition(text))
                    continue;

                EvaluateSubmition(text);
                _textBuilder.Clear();
            }
        }

        protected void EvaluateMetaCommand(string input)
        {
            if (input == "#showTree")
            {
                _showTree = !_showTree;
                Console.WriteLine(_showTree ? "Showing parse trees." : "Not showing parse trees");
                
            }
            else if (input == "#showProgram")
            {
                _showProgram = !_showProgram;
                Console.WriteLine(_showProgram ? "Showing Bound trees." : "Not showing Bound trees");
               
            }
            else if (input == "#cls")
            {
                Console.Clear();
               
            }
            else if (input == "#reset")
            {
                _previous = null;
                _variables.Clear();               
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid command {input}.");
                Console.ResetColor();
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
        protected void EvaluateSubmition(string text)
        {
            var syntaxTree = SyntaxTree.Parse(text);


            var compilation = _previous == null
                                ? new Compilation(syntaxTree)
                                : _previous.ContinueWith(syntaxTree);



            if (_showTree)
            {
                syntaxTree.Root.WriteTo(Console.Out);
            }
            if (_showProgram)
            {
                compilation.EmitTree(Console.Out);

            }
            var result = compilation.Evaluate(_variables);

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
