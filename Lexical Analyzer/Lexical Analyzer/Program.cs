using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lexical_Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string sourceCode = "source-code.txt",
                         tokenSet   = "token-set.txt";
            
            string[] lines = Filling.Read(sourceCode);
            LexicalAnalyzer LA = new LexicalAnalyzer(lines);
            LA.analyze(tokenSet);

            string[] tokens = Filling.Read(tokenSet);
            SyntaxAnalyzer SA = new SyntaxAnalyzer(tokens);
            SA.analyze();

        }
    }
}
