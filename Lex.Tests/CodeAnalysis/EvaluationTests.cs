using System;
using Lex.CodeAnalysis;
using Lex.CodeAnalysis.Syntax;
using Lex.CodeAnalysis.Symbols;
using System.Collections.Generic;
using Xunit;

namespace Lex.Tests.CodeAnalysis
{
    public class EvaluationTests
    {
        [Theory]
        [InlineData("1", 1)]
        [InlineData("+1", 1)]
        [InlineData("-1", -1)]
        [InlineData("~1", -2)]
        [InlineData("14 + 12", 26)]
        [InlineData("12 - 3", 9)]
        [InlineData("4 * 2", 8)]
        [InlineData("9 / 3", 3)]
        [InlineData("(10)", 10)]

        [InlineData("5 % 4", 1)]
        [InlineData("3 ** 2", 9)]

        [InlineData("12 == 3", false)]
        [InlineData("3 == 3", true)]
        [InlineData("12 != 3", true)]
        [InlineData("3 != 3", false)]

        [InlineData("3 < 4", true)]
        [InlineData("5 < 4", false)]
        [InlineData("3 <= 3", true)]
        [InlineData("4 <= 5", true)]
        [InlineData("5 <= 4", false)]

        [InlineData("1 | 2", 3)]
        [InlineData("1 | 0", 1)]
        [InlineData("1 & 2", 0)]
        [InlineData("1 & 0", 0)]
        [InlineData("1 ^ 0", 1)]
        [InlineData("0 ^ 1", 1)]
        [InlineData("1 ^ 3", 2)]

        [InlineData("3 > 4", false)]
        [InlineData("5 > 4", true)]
        [InlineData("3 >= 3", true)]
        [InlineData("4 >= 5", false)]
        [InlineData("5 >= 4", true)]
       

        [InlineData("false == false", true)]
        [InlineData("true == false", false)]
        [InlineData("false != false", false)]
        [InlineData("true != false", true)]
        [InlineData("true && true", true)]
        [InlineData("false || false", false)]

        [InlineData("true | true", true)]
        [InlineData("false | false", false)]
        [InlineData("true | false", true)]
        [InlineData("false | true", true)]

        [InlineData("true & true", true)]
        [InlineData("false & false", false)]
        [InlineData("true & false", false)]
        [InlineData("false & true", false)]

        [InlineData("true ^ true", false)]
        [InlineData("false ^ false", false)]
        [InlineData("true ^ false", true)]
        [InlineData("false ^ true", true)]

        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("!true", false)]
        [InlineData("!false", true)]
        [InlineData("{a = 0 (a = 10) * a }", 100)]
        [InlineData("{a = 0 if a ==0 a=10 a }", 10)]
        [InlineData("{a = 0 if a == 4 a=10 a }", 0)]

        [InlineData("{a = 0 if a == 0 a=10 else a =5 a }", 10)]
        [InlineData("{a = 0 if a == 4 a=10 else a =5 a }", 5)]
        [InlineData("{i = 10  result = 0 while i > 0 {result = result + i i = i -1} result}", 55)]
        [InlineData("{result = 0 for i = 1 to 10 by 1 { result = result + i } result }", 55)]
        [InlineData("{ i = 0 while i < 5 { i = i + 1 if i == 5 continue } i }", 5)]
        [InlineData("{result = 0 for i = 1 to 10 { result = result + i } result }", 55)]

        [InlineData("\"test\"", "test")]
        [InlineData("\"te\"\"st\"", "te\"st")]
        [InlineData("\"test\" == \"test\"", true)]
        [InlineData("\"test\" != \"test\"", false)]
        [InlineData("\"test\" + \"abc\"", "testabc")]
        [InlineData("\"test\" == \"abc\"", false)]
        [InlineData("\"test\" != \"abc\"", true)]
        public void Evaluator_Computes_CorrectValues(string text, object expectedValue)
        {
            AssertValue(text, expectedValue);
        } 
        // [Fact]
        // public void Evaluator_VariableDeclaration_Reports_Redeclaration()
        // {
        //     var text = @"
        //         {
        //             x = 10
        //             y = 100
        //             {
        //                  x = 10
        //             }
        //             [x] = 5
        //         }
        //     ";

        //     var diagnostics = @"
        //         Variable 'x' is already declared.
        //     ";

        //     AssertDiagnostics(text, diagnostics);
        // }
       
        [Fact]
        public void Evaluator_BlockStatement_NoInfiniteLoop()
        {
            var text = @"
                {                   
                [)][]
            ";

            var diagnostics = @"
                ERROR: Unexpected token <CloseParenthesisToken>, expected <IdentifierToken>.
                ERROR: Unexpected token <EndOfFileToken>, expected <CloseBraceToken>.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Name_Reports_Undefined()
        {
            var text = @"[x] + 10";

            var diagnostics = @"
                Variable 'x' doesn't exist.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Name_Reports_NoErrorForInserted()
        {
            var text = @"1 +[]";

            var diagnostics = @"
                ERROR: Unexpected token <EndOfFileToken>, expected <IdentifierToken>.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        public void Evaluator_Assigned_Reports_Undefined()
        {
            var text = @"[x] * 10";

            var diagnostics = @"
                Variable 'x' doesn't exist.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        // [Fact]
        // public void Evaluator_Assigned_Reports_CannotAssign()
        // {
        //     var text = @"
        //         {
        //             let x = 10
        //             x [=] 0
        //         }
        //     ";

        //     var diagnostics = @"
        //         Variable 'x' is read-only and cannot be assigned to.
        //     ";

        //     AssertDiagnostics(text, diagnostics);
        // }

        [Fact]
        public void Evaluator_Assigned_Reports_CannotConvert()
        {
            var text = @"
                {
                    x = 10
                    x = [true]
                }
            ";

            var diagnostics = @"
                Cannot convert type 'bool' to 'int'.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        public void Evaluator_IfStatement_Reports_CannotConvert()
        {
            var text = @"
                {
                    x = 10
                    if [10]
                        x = 10
                }
            ";

            var diagnostics = @"
                Cannot convert type 'int' to 'bool'.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_WhileStatement_Reports_CannotConvert()
        {
            var text = @"
                {
                    x = 10
                    while [10]
                        x = 10
                }
            ";

            var diagnostics = @"
                Cannot convert type 'int' to 'bool'.
            ";

            AssertDiagnostics(text, diagnostics);
        }     
         [Fact]
        public void Evaluator_ForStatement_Reports_CannotConvert_Lowerbound()
        {
            var text = @"
                {
                    result = 10
                    for i = [false] to 10
                        result = result + i
                }
            ";

            var diagnostics = @"
                Cannot convert type 'bool' to 'int'.
            ";

            AssertDiagnostics(text, diagnostics);
        }

         [Fact]
        public void Evaluator_ForStatement_Reports_CannotConvert_upperbound()
        {
            var text = @"
                {
                    result = 10
                    for i = 0 to [true]
                        result = result + i
                }
            ";

            var diagnostics = @"
                Cannot convert type 'bool' to 'int'.
            ";

            AssertDiagnostics(text, diagnostics);
        }


        [Fact]
        public void Evaluator_Unary_Reports_Undefined()
        {
            var text = @"[+]true";

            var diagnostics = @"
                Unary operator '+' is not defined for type 'bool'.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Binary_Reports_Undefined()
        {
            var text = @"10 [*] false";

            var diagnostics = @"
                Binary operator '*' is not defined for types 'int' and 'bool'.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        public void Evaluator_InvokeFunctionArguments_NoInfiniteLoop()
        {
            var text = @"
                print(""Hi""[[=]][)]
            ";

            var diagnostics = @"
               ERROR: Unexpected token <EaqlesToken>, expected <CloseParenthesisToken>.
               ERROR: Unexpected token <EaqlesToken>, expected <IdentifierToken>.
               ERROR: Unexpected token <CloseParenthesisToken>, expected <IdentifierToken>.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_FunctionParameters_NoInfiniteLoop()
        {
            var text = @"
                function hi(name: string[[[=]]][)]
                {
                    print(""Hi "" + name + ""!"" )
                }[]
            ";

            var diagnostics = @"
               
                ERROR: Unexpected token <EaqlesToken>, expected <CloseParenthesisToken>.
                ERROR: Unexpected token <EaqlesToken>, expected <OpenBraceToken>.
                ERROR: Unexpected token <EaqlesToken>, expected <IdentifierToken>.
                ERROR: Unexpected token <CloseParenthesisToken>, expected <IdentifierToken>.
                ERROR: Unexpected token <EndOfFileToken>, expected <CloseBraceToken>.
            ";

            AssertDiagnostics(text, diagnostics);
        }
        [Fact]
        public void Evaluator_Void_Function_Should_Not_Return_Value()
        {
            var text = @"
                function test()
                {
                    return [1]
                }
            ";

            var diagnostics = @"
                Since the function 'test' does not return a value the 'return' keyword cannot be followed by an expression.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Function_With_ReturnValue_Should_Not_Return_Void()
        {
            var text = @"
                function test(): int
                {
                    [return]
                }
            ";

            var diagnostics = @"
                An expression of type 'int' expected.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Not_All_Code_Paths_Return_Value()
        {
            var text = @"
                function [test](n: int): bool
                {
                    if (n > 10)
                       return true
                }
            ";

            var diagnostics = @"
                Not all code paths return a value.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Expression_Must_Have_Value()
        {
            var text = @"
                function test(n: int)
                {
                    return
                }
                value = [test(100)]
            ";

            var diagnostics = @"
                Expression must have a Value.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Theory]
        [InlineData("[break]", "break")]
        [InlineData("[continue]", "continue")]
        public void Evaluator_Invalid_Break_Or_Continue(string text, string keyword)
        {
            var diagnostics = $@"
                The keyword '{keyword}' can only be used inside of loops.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Invalid_Return()
        {
            var text = @"
                [return]
            ";

            var diagnostics = @"
                The 'return' keyword can only be used inside of functions.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Parameter_Already_Declared()
        {
            var text = @"
                function sum(a: int, b: int, [a: int]): int
                {
                    return a + b + c
                }
            ";

            var diagnostics = @"
                A parameter with the name 'a' already exists.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Function_Must_Have_Name()
        {
            var text = @"
                function [(]a: int, b: int): int
                {
                    return a + b
                }
            ";

            var diagnostics = @"
               ERROR: Unexpected token <OpenParenthesisToken>, expected <IdentifierToken>.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Wrong_Argument_Type()
        {
            var text = @"
                function test(n: int): bool
                {
                    return n > 10
                }
                testValue = ""string""
                test([testValue])
            ";

            var diagnostics = @"
               
                Function 'test', parameter 'n' requires a value of type 'int' arguments but was given a value of type  'string'.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Bad_Type()
        {
            var text = @"
                function test(n: [invalidtype])
                {
                }
            ";

            var diagnostics = @"
                Type 'invalidtype' doesn't exist.
            ";

            AssertDiagnostics(text, diagnostics);
        }

        private static void AssertValue(string text, object expectedValue)
        {
            var syntaxTree = SyntaxTree.Parse(text);
            var compilation = new Compilation(syntaxTree);
            var variables = new Dictionary<VariableSymble, object>();
            var result = compilation.Evaluate(variables);

            Assert.Empty(result.Diagnostics);
            Assert.Equal(expectedValue, result.Value);
        }

        private void AssertDiagnostics(string text, string diagnosticText)
        {
            var annotatedText = AnnotatedText.Parse(text);
            var syntaxTree = SyntaxTree.Parse(annotatedText.Text);
            var compilation = new Compilation(syntaxTree);
            var result = compilation.Evaluate(new Dictionary<VariableSymble, object>());

            var expectedDiagnostics = AnnotatedText.UnindentLines(diagnosticText);

            if (annotatedText.Spans.Length != expectedDiagnostics.Length)
                throw new Exception("ERROR: Must mark as many spans as there are expected diagnostics");

            Assert.Equal(expectedDiagnostics.Length, result.Diagnostics.Length);

            for (var i = 0; i < expectedDiagnostics.Length; i++)
            {
                var expectedMessage = expectedDiagnostics[i];
                var actualMessage = result.Diagnostics[i].Message;
                Assert.Equal(expectedMessage, actualMessage);

                var expectedSpan = annotatedText.Spans[i];
                var actualSpan = result.Diagnostics[i].Span;
                Assert.Equal(expectedSpan, actualSpan);
            }
        }
    }

    
    
}
