using System;

namespace lex
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine(">");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;
                if (line == "1 + 2 * 3")
                {
                    Console.WriteLine("7");
                }
                else
                {
                    Console.WriteLine("ERROE:invalid Expression");
                }
            }
        }
    }
}
