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

        public SyntaxAnalyzer(string[] tokenSet)
        {
            List<Token> tokens = new List<Token>();
            foreach (string token in tokenSet)
                tokens.Add(new Token(token));
            this.tokenSet = tokens.ToArray();
        }

        public int analyze()
        {
            int tokenIndex = SyntaxTree.MainSyntaxTree.analyze(tokenSet);
            return tokenIndex;
        }
    }
}
