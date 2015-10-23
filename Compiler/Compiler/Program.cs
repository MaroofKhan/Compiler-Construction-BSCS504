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
            SyntaxAnalyzer SA = new SyntaxAnalyzer(tokens);
            SA.analyze();


        }
    }
}
