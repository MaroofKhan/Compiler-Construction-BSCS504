using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> language = new List<string>();
            string[] lines = Filling.Read("sample-token.txt");
            for (int number = 0; number < lines.Length; number++)
            {
                string line = lines[number];
                int lineNumber;
                try
                {
                    lineNumber = getLineNumber(line);
                }
                catch (Exception ex)
                {
                    continue;
                }
                string main = string.Empty;
                while (true)
                {
                    try
                    {
                        line = lines[number];

                        string classPart = line.Replace(" ", "-").Replace("(", "").Replace(")", "").Replace(",", " ").Split(' ')[0];
                        main = (main == string.Empty) ? classPart : main + " " + classPart;
                        try
                        {
                            if (lineNumber != getLineNumber(lines[number + 1])) break;
                            else number++;
                        }
                        catch (Exception ex)
                        {
                            number++;
                        }
                    }
                    catch (Exception exception)
                    {
                        break;
                    }
                }
                language.Add(main);
                
            }

            string[] arrayOfTokenStrings = language.ToArray();
            foreach (string classpart in arrayOfTokenStrings)
            {
                //TODO:
            }

        }

        static int getLineNumber (string line) 
        {
            return Convert.ToInt32(line.Replace(" ", "-").Replace("(", "").Replace(")", "").Replace(",", " ").Split(' ')[2]);
        }
    }
}
