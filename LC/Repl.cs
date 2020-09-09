using System;
using System.Text;

namespace Lex
{
    internal abstract class Repl
    {
        private readonly StringBuilder _textBuilder = new StringBuilder();
        public void Run()
        {
    
            while (true)
            {
                

                var text = EditSubmission();
                if (text == null)
                    return;

                EvaluateSubmition(text);
                _textBuilder.Clear();
            }
        }

        private String EditSubmission()
        {
            StringBuilder textBuilder = new StringBuilder();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (_textBuilder.Length != 0)
                    Console.Write("·");
                else
                    Console.Write("» ");

                Console.ResetColor();

                var input = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);


                if (_textBuilder.Length == 0)
                {
                    if (isBlank)
                        return null;

                    if (input.StartsWith("#"))
                    {
                        EvaluateMetaCommand(input);
                        continue;
                    }
                }

                _textBuilder.AppendLine(input);
                var text = _textBuilder.ToString();

                if (!IsCompleteSubmition(text))
                    continue;

                return text;
               
            }
            
        }

        protected virtual void EvaluateMetaCommand(string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid command {input}.");
            Console.ResetColor();
        }

        protected abstract bool IsCompleteSubmition(string text);

        protected abstract void EvaluateSubmition(string text);
      
    
    }
}
