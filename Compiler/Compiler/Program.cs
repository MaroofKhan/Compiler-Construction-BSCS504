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
            string[] code = Filling.Read("source-code.txt");
            LexicalAnalyzer LA = new LexicalAnalyzer(code);
            LA.analyze("token-set.txt");


        }
    }
}
