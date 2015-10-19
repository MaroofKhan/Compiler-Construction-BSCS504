using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class SyntaxAnalyzer
    {
        string[] tokens;
        public SyntaxAnalyzer(string[] tokens)
        {
            this.tokens = tokens;
        }

        public void analyze()
        {
            List<string> lines = new List<string>();
            List<int> numbers = new List<int>();
            for (int token = 0; token < tokens.Length; token++)
            {
                lines.Add(classpart(tokens[token]));
                numbers.Add(line(tokens[token]));
            }

            Parser parser = new Parser();
            parser.lineNumbers = numbers.ToArray();
            parser.sentence = lines.ToArray();
            int[] errors = parser.parse();
            foreach (int error in errors)
            {
                Console.WriteLine(error);
            }
        }

        private string classpart(string token)
        {
            return token.Replace(" ", "-").Replace("~", "").Replace("~", "").Replace("|", " ").Split(' ')[0];
        }
        private int line(string token)
        {
            return Convert.ToInt32(
                    token.Replace(" ", "-").Replace("~", "").Replace("~", "").Replace("|", " ").Split(' ')[2]
                );
        }
    }



}
