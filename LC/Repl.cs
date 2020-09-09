using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace Lex
{
    internal abstract class Repl
    {
        
        public void Run()
        {
    
            while (true)
            {
                var text = EditSubmissionOld();
                if (text == null)
                    return;

                EvaluateSubmition(text);
               
            }
        }

        private sealed class SubmissionView
        {
            private readonly ObservableCollection<string> _submissionDocument;
            private readonly int _cursortop;
            private int  _renderedLineCount;

            public SubmissionView(ObservableCollection<String> submissionDocument)
            {
                _submissionDocument = submissionDocument;
                _submissionDocument.CollectionChanged += SubmissionDocumentChange;
                _cursortop = Console.CursorTop;
            }

            private void SubmissionDocumentChange(object sender, NotifyCollectionChangedEventArgs e) => Render();

            private void Render()
            {
               Console.SetCursorPosition(0,_cursortop);


               var lineCount = 0;

               foreach(var line in _submissionDocument)
               {
                   Console.ForegroundColor = ConsoleColor.Green;
                    
                    if (lineCount == 0)
                        Console.Write("·");
                    else
                    {
                       
                        Console.Write("» ");
                    }
                        
                    Console.ResetColor();
                    Console.WriteLine(line);
                    lineCount++;
                   
               }
               var numberOfBlankLines = _renderedLineCount - lineCount;
               if(numberOfBlankLines > 0)
               {
                    var blankLine = new string(' ', Console.WindowWidth);
                    while(numberOfBlankLines > 0)
                    {
                        Console.WriteLine(blankLine);
                    }
               }

               _renderedLineCount = lineCount;
               
            }

            private int currentLineIndex;

            public int GetCurrentLineIndex()
            {
                return currentLineIndex;
            }

            public void SetCurrentLineIndex(int value)
            {
                currentLineIndex = value;
            }

            private int currentLineCharacter;

            public int GetCurrentLineCharacter()
            {
                return currentLineCharacter;
            }

            public void SetCurrentLineCharacter(int value)
            {
                currentLineCharacter = value;
            }
        }

          private String EditSubmission()
          {
              var document = new ObservableCollection<string>();
              var view = new SubmissionView(document);

              while (true)
              {
                  var key = Console.ReadKey(true);
                  HandleKey(key,document,view);
              }
          }

        private void HandleKey(ConsoleKeyInfo key, ObservableCollection<string> document, SubmissionView view)
        {
            
        }

        private String EditSubmissionOld()
        {
            StringBuilder _textBuilder = new StringBuilder();
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
