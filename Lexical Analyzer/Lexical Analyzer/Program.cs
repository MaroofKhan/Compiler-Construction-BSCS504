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
            string[] lines = Filling.Read("test-source-code.txt");
            string tokenSet = string.Empty;
            for (int index = 1; index <= lines.Length; index++)
            {
                string line = lines[index - 1];
                Console.WriteLine(line);
                string[] words = line.Split(' ');
                foreach (string word in words)
                {
                    if (String.allAlphabets(word))
                    {
                        string token = generateTokenSet(word, index);
                        String.appendLine(ref tokenSet, token);
                        continue;
                    }

                    char[] letters = word.ToCharArray();
                    string temporaryWord = string.Empty;
                    foreach (char letter in letters)
                    {
                        /* Word with all alphabets covered! -START- */
                        if (Char.isAlphabet(letter))
                        {
                            temporaryWord += letter;
                            continue;
                        }

                        else if (!(String.isEmpty(temporaryWord)))
                        {
                            string token = generateTokenSet(temporaryWord, index);
                            String.appendLine(ref tokenSet, token);
                            temporaryWord = string.Empty;
                        }
                        /* Word with all alphabets covered! -END- */

                        if ((String.isEmpty(temporaryWord)) && Char.isPunctuator(letter))
                        {
                            string token = generateTokenSet(letter, index);
                            String.appendLine(ref tokenSet, token);
                        }
                    }
                }
            }

            Filling.Write("test-token-set.txt", tokenSet);
            
        }

        static
        string generateTokenSet(string word, int line)
        {
            ClassPart classPart = String.classPart(word);
            return ("(" + classPart.name + ", " + word + ", " + line + ")");
        }

        static
        string generateTokenSet(char letter, int line)
        {
            return ("(" + letter + ", " + letter + ", " + line + ")");
        }
    }
}
