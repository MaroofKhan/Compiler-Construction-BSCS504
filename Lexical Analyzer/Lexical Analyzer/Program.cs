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

            string[] lines = Filling.Read("source-code.txt");
            LexicalAnalyzer LA = new LexicalAnalyzer(lines);

        }
    }
}
