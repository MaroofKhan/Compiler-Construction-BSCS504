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
            string tokenSet = string.Empty;
            for (int index = 1; index <= lines.Length; index++)
            {
                string line = lines[index - 1];
                if (String.isEmpty(line)) continue;
                string[] words = line.Split(' ');
                foreach (string word in words)
                {
                    if (String.containsBreaker(word))
                    {
                        if (DeterministicFiniteAutomaton.ValidateFloat(word))
                        {
                            string token = generateTokenSet(word, index);
                            String.appendLine(ref tokenSet, token);
                        }
                        else
                        {
                            char[] letters = word.ToCharArray();
                            string temporaryWord;
                            for (int letterIndex = 0; letterIndex < letters.Length; letterIndex++)
                            {
                                int currentLetterIndex = letterIndex;
                                temporaryWord = string.Empty;
                                if (Char.isPunctuator(letters[letterIndex]))
                                {
                                    if ((currentLetterIndex < letters.Length) && Char.isSimpleArithmeticOperator(letters[currentLetterIndex]))
                                    {
                                        temporaryWord += letters[currentLetterIndex++];
                                        if ((currentLetterIndex < letters.Length) && (Char.isSimpleArithmeticOperator(letters[currentLetterIndex]) || Char.isAssignmentOperator(letters[currentLetterIndex])))
                                        {
                                            temporaryWord += letters[currentLetterIndex++];
                                            string token = generateTokenSet(temporaryWord, index);
                                            String.appendLine(ref tokenSet, token);
                                            letterIndex = currentLetterIndex;
                                            temporaryWord = string.Empty;

                                        }
                                        else
                                        {
                                            string token = generateTokenSet(temporaryWord, index);
                                            String.appendLine(ref tokenSet, token);
                                            letterIndex = currentLetterIndex;
                                            temporaryWord = string.Empty;
                                        }
                                    }
                                    else if ((currentLetterIndex < letters.Length) && Char.isArithmeticOperator(letters[currentLetterIndex]))
                                    {
                                        temporaryWord += letters[currentLetterIndex++];
                                        if ((currentLetterIndex < letters.Length) && Char.isAssignmentOperator(letters[currentLetterIndex]))
                                        {
                                            temporaryWord += letters[currentLetterIndex++];
                                            string token = generateTokenSet(temporaryWord, index);
                                            String.appendLine(ref tokenSet, token);
                                            letterIndex = currentLetterIndex;
                                            temporaryWord = string.Empty;

                                        }
                                        else
                                        {
                                            string token = generateTokenSet(temporaryWord, index);
                                            String.appendLine(ref tokenSet, token);
                                            letterIndex = currentLetterIndex;
                                            temporaryWord = string.Empty;
                                        }
                                    }
                                    else if ((currentLetterIndex < letters.Length) && Char.isLogicalOperator(letters[currentLetterIndex]))
                                    {
                                        temporaryWord += letters[currentLetterIndex++];
                                        if ((currentLetterIndex < letters.Length) && Char.isLogicalOperator(letters[currentLetterIndex]))
                                        {
                                            temporaryWord += letters[currentLetterIndex++];
                                            string token = generateTokenSet(temporaryWord, index);
                                            String.appendLine(ref tokenSet, token);
                                            letterIndex = currentLetterIndex;
                                            temporaryWord = string.Empty;
                                        }
                                    }
                                    else if (currentLetterIndex < letters.Length && (Char.isRelationalOperator(letters[currentLetterIndex])))
                                    {
                                        temporaryWord += letters[currentLetterIndex++];
                                        if ((currentLetterIndex < letters.Length) && Char.isAssignmentOperator(letters[currentLetterIndex]))
                                        {
                                            temporaryWord += letters[currentLetterIndex++];
                                            string token = generateTokenSet(temporaryWord, index);
                                            String.appendLine(ref tokenSet, token);
                                            letterIndex = currentLetterIndex;
                                            temporaryWord = string.Empty;
                                        }
                                        else
                                        {
                                            string token = generateTokenSet(temporaryWord, index);
                                            String.appendLine(ref tokenSet, token);
                                            letterIndex = currentLetterIndex;
                                            temporaryWord = string.Empty;
                                        }
                                    }
                                    else if (currentLetterIndex < letters.Length && (Char.isAssignmentOperator(letters[currentLetterIndex])))
                                    {
                                        temporaryWord += letters[currentLetterIndex++];
                                        if ((currentLetterIndex < letters.Length) && Char.isAssignmentOperator(letters[currentLetterIndex]))
                                        {
                                            temporaryWord += letters[currentLetterIndex++];
                                            string token = generateTokenSet(temporaryWord, index);
                                            String.appendLine(ref tokenSet, token);
                                            letterIndex = currentLetterIndex;
                                            temporaryWord = string.Empty;
                                        }
                                        else
                                        {
                                            string token = generateTokenSet(temporaryWord, index);
                                            String.appendLine(ref tokenSet, token);
                                            letterIndex = currentLetterIndex;
                                            temporaryWord = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        string token = generateTokenSet(letters[letterIndex], index);
                                        String.appendLine(ref tokenSet, token);
                                        goto Continue;
                                    }
                                }

                                

                                bool isFloat = false;
                                while ((currentLetterIndex < letters.Length) && (Char.isDigit(letters[currentLetterIndex]) || (letters[currentLetterIndex] == '.')))
                                {
                                    if ((currentLetterIndex < letters.Length - 1) && (letters[currentLetterIndex] == '.'))
                                    {

                                        if (Char.isDigit(letters[currentLetterIndex + 1]) && !isFloat)
                                        {
                                            isFloat = true; 
                                            temporaryWord += '.';
                                            while (Char.isDigit(letters[++currentLetterIndex]))
                                            {
                                                temporaryWord += letters[currentLetterIndex];
                                            }

                                            if (!(currentLetterIndex < letters.Length))
                                            {
                                                if (!String.isEmpty(temporaryWord))
                                                {
                                                    string token = generateTokenSet(temporaryWord, index);
                                                    String.appendLine(ref tokenSet, token);
                                                    letterIndex = currentLetterIndex;
                                                    temporaryWord = string.Empty;
                                                    
                                                }
                                            }
                                            else
                                            {

                                                if (!String.isEmpty(temporaryWord))
                                                {
                                                    string token = generateTokenSet(temporaryWord, index);
                                                    String.appendLine(ref tokenSet, token);
                                                    letterIndex = currentLetterIndex;
                                                    temporaryWord = string.Empty;
                                                    goto Continue;
                                                }
                                            }

                                            if (!String.isEmpty(temporaryWord))
                                            {
                                                string token = generateTokenSet(temporaryWord, index);
                                                String.appendLine(ref tokenSet, token);
                                                letterIndex = currentLetterIndex;
                                                temporaryWord = string.Empty;

                                                string anotherToken = generateTokenSet('.', index);
                                                String.appendLine(ref tokenSet, anotherToken);
                                                break;
                                            }

                                            
                                        }
                                        else
                                        {
                                            string token = generateTokenSet(word, index);
                                            String.appendLine(ref tokenSet, token);
                                        }
                                    }
                                    else
                                    {
                                        temporaryWord += letters[currentLetterIndex++];
                                    }
                                }
                                if (!String.isEmpty(temporaryWord))
                                {
                                    string token = generateTokenSet(temporaryWord, index);
                                    String.appendLine(ref tokenSet, token);
                                    letterIndex = currentLetterIndex;
                                    continue;
                                }

                                while ((currentLetterIndex < letters.Length) && Char.isAlphabet(letters[currentLetterIndex]))
                                {
                                    temporaryWord += letters[currentLetterIndex++];
                                }
                                if (!String.isEmpty(temporaryWord))
                                {
                                    string token = generateTokenSet(temporaryWord, index);
                                    String.appendLine(ref tokenSet, token);
                                    letterIndex = currentLetterIndex;
                                    continue;
                                }


                            Continue:
                                continue;
                            }
                        }
                    }
                    else
                    {
                        string token = generateTokenSet(word, index);
                        String.appendLine(ref tokenSet, token);
                    }

                    /*
                    if (DeterministicFiniteAutomaton.ValidateFloat(word))
                    {
                        Console.WriteLine("FLOAT");
                        continue;
                    }
                    else if (DeterministicFiniteAutomaton.ValidateIdentifier(word))
                    {
                        Console.WriteLine("IDENTIFIER");
                        continue;
                    }

                    if (String.isAllAlphabets(word)) {
                        /* Can be a keyword, or identifier
                        string token = generateTokenSet(word, index);
                        String.appendLine(ref tokenSet, token);
                    }
                    else if (String.isAllDigits(word))
                    {
                        /* Can be an integer
                        string token = generateTokenSet(word, index);
                        String.appendLine(ref tokenSet, token);
                    }


                    
                    Console.WriteLine(word);
                    char[] letters = word.ToCharArray();
                    for (int letterIndex = 0; letterIndex < letters.Length; letterIndex++)
                    {
                        string temporaryWord = string.Empty;
                        if (Char.isAlphabet(letters[letterIndex]))
                        {
                            int currentLetterIndex = letterIndex;
                            while (currentLetterIndex < letters.Length && (Char.isAlphabet(letters[currentLetterIndex]) ||
                                    letters[currentLetterIndex] == '_' || Char.isDigit(letters[currentLetterIndex])))
                            {
                                temporaryWord += letters[currentLetterIndex++];
                            }
                            letterIndex = currentLetterIndex;
                            if (!String.isEmpty(temporaryWord))
                            {
                                string token = generateTokenSet(temporaryWord, index);
                                String.appendLine(ref tokenSet, token);
                            }
                            continue;
                        }

                        if (Char.isDigit(letters[letterIndex]))
                        {
                            int currentLetterIndex = letterIndex;
                            while (currentLetterIndex < letters.Length && ((Char.isDigit(letters[currentLetterIndex])) ||
                                       letters[currentLetterIndex] == '.' || Char.isAlphabet(letters[currentLetterIndex])))
                            {
                                temporaryWord += letters[currentLetterIndex++];
                            }

                            letterIndex = currentLetterIndex;
                            if (!String.isEmpty(temporaryWord))
                            {
                                string token = generateTokenSet(temporaryWord, index);
                                String.appendLine(ref tokenSet, token);
                                temporaryWord = string.Empty;
                            }
                            continue;
                        }

                        if (Char.isSimpleArithmeticOperator(letters[letterIndex]))
                        {

                        }
                        */
                        /*
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
                        /* Word with all alphabets covered! -END-
                        
 
                        if (Char.isSimpleArithmeticOperator(letter))
                        {
                            char anotherLetter = letters[letterIndex + 1];
                            if (Char.isSimpleArithmeticOperator(anotherLetter) || Char.isAssignmentOperator(anotherLetter))
                            {
                                string anotherWord = string.Empty + letter + anotherLetter;
                                string token = generateTokenSet(anotherWord, index);
                                String.appendLine(ref tokenSet, token);
                                letterIndex++;
                            }
                            else
                            {
                                string token = generateTokenSet(letter, index);
                                String.appendLine(ref tokenSet, token);
                            }
                        }
                        else if (Char.isArithmeticOperator(letter))
                        {
                            char anotherLetter = letters[letterIndex + 1];
                            if (Char.isAssignmentOperator(anotherLetter))
                            {
                                string anotherWord = string.Empty + letter + anotherLetter;
                                string token = generateTokenSet(anotherWord, index);
                                String.appendLine(ref tokenSet, token);
                                letterIndex++;
                            }
                            else
                            {
                                string token = generateTokenSet(letter, index);
                                String.appendLine(ref tokenSet, token);
                            }
                        }

                        if (Char.isPunctuator(letter))
                        {
                            string token = generateTokenSet(letter, index);
                            String.appendLine(ref tokenSet, token);
                        } 
                        */
                    //}
                }
                
            }
            Console.WriteLine(tokenSet);
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
