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
            int lineNumber = 1;
            List<string> lines = new List<string>();
            for (int token = 0; token < tokens.Length; lineNumber++)
            {
                string statement = string.Empty;
                while (token < tokens.Length && line(tokens[token]) == lineNumber)
                {
                    statement += ((statement == string.Empty) ? classpart(tokens[token]) : " " + classpart(tokens[token]));
                    token++;
                }

                lines.Add(statement);
            }

            string[] statements = lines.ToArray();
            foreach (string statement in statements)
            {
                Console.WriteLine(statement);
            }
        }

        private string classpart(string token)
        {
            return token.Replace(" ", "-").Replace("(", "").Replace(")", "").Replace("|", " ").Split(' ')[0];
        }
        private int line(string token)
        {
            return Convert.ToInt32(
                    token.Replace(" ", "-").Replace("(", "").Replace(")", "").Replace("|", " ").Split(' ')[2]
                );
        }
    }



}
