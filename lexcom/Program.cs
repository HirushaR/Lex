using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lex.CodeAnalysis;
using Lex.CodeAnalysis.Symbols;
using Lex.CodeAnalysis.Syntax;

namespace Lex
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length ==0)
            {
                Console.Error.WriteLine("usage: lex <source-paths>");
                return;
            }

            if (args.Length > 1)
            {
                Console.Error.WriteLine("Error: Onl one path supported right now");
                return;
            }

            var path = args.Single();

            var text = File.ReadAllText(path);
            var syntaxTree = SyntaxTree.Parse(text);

            var compilation = new Compilation(syntaxTree);
            var result = compilation.Evaluate(new Dictionary<VariableSymble , object>());
            
            if (result.Diagnostics.Any()){
                Console.Error.WriteDiagnostics(result.Diagnostics, syntaxTree);
            }else{
                if (result.Value != null){
                    Console.Out.WriteLine(result.Value);
                }
            }
        }
    }
}
