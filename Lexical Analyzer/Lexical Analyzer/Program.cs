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
                string[] words = line.Split(' ');
                foreach (string word in words)
                {
                    String _word = new String(word);
                    Console.WriteLine(word);
                    Console.WriteLine(_word.allAlphabets);
                }
            }
            
        }
    }
}
