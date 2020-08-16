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
    internal static class Program
    {
        private static void Main()
        {
            var showTree = false;
            var showProgram = false;
            var variables = new Dictionary<VariableSymble, object>();
            var textBuilder = new StringBuilder();
            Compilation previous = null;

            while (true)
            {
                Console.ForegroundColor =ConsoleColor.Green;
                if( textBuilder.Length == 0)
                    Console.Write("» ");
                else
                    Console.Write("·");
                
                Console.ResetColor();
                
                var input = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);
              

                if(textBuilder.Length ==0)
                {
                    if(isBlank)
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
                        previous = null;
                        variables.Clear();
                        continue;
                    }
                }

                textBuilder.AppendLine(input);
                var text = textBuilder.ToString();
                var syntaxTree = SyntaxTree.Parse(text);

                if(!isBlank && syntaxTree.Diagnostics.Any())
                {
                    continue;
                }

                var compilation = previous == null
                                    ? new Compilation(syntaxTree)
                                    : previous.ContinueWith(syntaxTree);
                var result = compilation.Evaluate(variables);


                if (showTree)
                {
                    syntaxTree.Root.WriteTo(Console.Out);               
                }
                if (showProgram)
                {
                    compilation.EmitTree(Console.Out);
                         
                }

                if (!result.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(result.Value);
                    Console.ResetColor();
                    previous = compilation;
                }
                else
                {                 
                   

                    foreach (var diagnostic in result.Diagnostics)
                    {
                        var lineIndex = syntaxTree.Text.GetLineIndex(diagnostic.Span.Start);
                        var lineNumber = lineIndex + 1;
                        var line = syntaxTree.Text.Lines[lineIndex];
                        var character = diagnostic.Span.Start - line.Start +1;

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
                textBuilder.Clear();
            }
        }

      
    }
}
