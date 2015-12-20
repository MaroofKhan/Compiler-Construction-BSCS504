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

            
            string[] code = Filling.Read(sourcecode);
            
            LexicalAnalyzer LA = new LexicalAnalyzer(code);
            LA.analyze(tokenset);
            

            string[] tokens = Filling.Read(tokenset);
            List<Token> _tokens = new List<Token>();
            foreach (string token in tokens)
                _tokens.Add(new Token(token));
            ICGTree.MainSyntaxTree.analyze(_tokens.ToArray());
            foreach (string line in ICGTree.MainSyntaxTree.intermediateCode.ToArray())
                Console.WriteLine(line);
            /*
             * SyntaxAnalyzer SA = new SyntaxAnalyzer(tokens);
            int tokenIndex = SA.analyze();
            
            //int tokenIndex = 0;


            List<Token> _tokens = new List<Token>();
            foreach (string token in tokens)
                _tokens.Add(new Token(token));
            TestSemanticTree.MainSemanticTree.parse(_tokens.ToArray());
            List<ErrorRecord> errors= TestSemanticTree.MainSemanticTree.errors;

            Console.WriteLine("Errors");
            foreach (ErrorRecord record in errors.ToArray())
                Console.WriteLine(record.identifier + ", " + record.type + ", "+ record._token.line);
            
            if (tokenIndex == -1)
            {
                Console.WriteLine("Successfully parsed!");
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
             * */


        }
    }
}
