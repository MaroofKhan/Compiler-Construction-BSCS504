using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {

            const string sourcecode = "source-code.txt";
            const string tokenset = "token-set.txt";
            const string intermediatecode = "intermediate-code.txt";

            
            string[] code = Filling.Read(sourcecode);
            
            LexicalAnalyzer LA = new LexicalAnalyzer(code);
            LA.analyze(tokenset);
            

            string[] tokens = Filling.Read(tokenset);
            SyntaxAnalyzer SA = new SyntaxAnalyzer(tokens);
            int tokenIndex = SA.analyze();
            
            //int tokenIndex = 0;
            
            if (tokenIndex == -1)
            {
                List<Token> _tokens = new List<Token>();
                foreach (string token in tokens)
                    _tokens.Add(new Token(token));

                TestSemanticTree.MainSemanticTree.parse(_tokens.ToArray());
                List<ErrorRecord> errors= TestSemanticTree.MainSemanticTree.errors;
                if (errors.ToArray().Length == 0)
                {
                    ICGTree.MainSyntaxTree.analyze(intermediatecode, _tokens.ToArray());
                }
                else
                {
                    foreach (ErrorRecord error in errors.ToArray())
                        Console.WriteLine(error.identifier + "on line# " + error._token.line + "(" + error.type + ")");
                }
            
            }
            else
            {
                Token token = new Token(tokens[(tokenIndex < tokens.Length) ? tokenIndex : tokenIndex - 1]);
                string line = code[token.line - 1];
                int index = line.IndexOf(token.valuepart);
                string error = string.Empty;
                while (index > 0)
                {
                    index--;
                    error += " ";
                }
                error += "^";
                Console.WriteLine(line);
                Console.WriteLine(error);
            }
        }
    }
}
