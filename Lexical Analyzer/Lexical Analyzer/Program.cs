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
            /* Reading from the .txt file */
            string[] lines = Filling.Read("source-code.txt");
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
            
            Filling.Write("token-set.txt", lines);
        }
    }
}
