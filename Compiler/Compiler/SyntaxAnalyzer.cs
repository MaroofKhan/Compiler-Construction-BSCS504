using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class SyntaxAnalyzer
    {
        Token[] tokenSet;
        Selection[] selectionSet;

        public SyntaxAnalyzer(string[] tokenSet)
        {
            List<Token> tokens = new List<Token>();
            foreach (string token in tokenSet)
                tokens.Add(new Token(token));
            this.tokenSet = tokens.ToArray();
        }

        public void analyze()
        {
            Token token = (new Parser(tokenSet)).parse("asdsad");
            Console.WriteLine("OUT: " + (token == null ? "PARSED!" : token.token));
        }
    }
}
