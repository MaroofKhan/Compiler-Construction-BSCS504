using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class LexicalAnalyzer
    {
        string[] lines;
        public LexicalAnalyzer(string[] code)
        {
            this.lines = code;

            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                string line = lines[lineNumber];
                if (line.Contains('"'))
                {
                    int i = line.IndexOf('"');
                    string line1 = line.Substring(0, i);
                    string line2 = line.Substring(i, (line.Length - i));
                    if (line2.Contains('"'))
                    {
                        line2 = line2.Replace(" ", "-");
                    }

                    lines[lineNumber] = line1 + line2;
                }
            }

            string tokenSet = string.Empty;
            bool isMultiLineComment = false;
            for (int index = 1; index <= lines.Length; index++)
            {
                string line = lines[index - 1];
                if (String.isEmpty(line)) continue;
                string[] words = line.Split(' ');
                bool isString = false;
                string theString = string.Empty;

                for (int wordIndex = 0; wordIndex < words.Length; wordIndex++)
                {
                    string word = words[wordIndex];

                    if (String.containsBreaker(word) || isMultiLineComment)
                    {
                        if (DeterministicFiniteAutomaton.ValidateFloat(word))
                        {
                            if (!(isMultiLineComment))
                            {
                                string token = generateTokenSet(word, index);
                                String.appendLine(ref tokenSet, token);
                            }
                        }
                        else
                        {
                            char[] letters = word.ToCharArray();
                            string temporaryWord;
                            for (int letterIndex = 0; letterIndex < letters.Length; letterIndex++)
                            {

                                if (isString)
                                {
                                    while (letterIndex < letters.Length && letters[letterIndex] != '"')
                                    {
                                        if (letterIndex < letters.Length)
                                            theString += letters[letterIndex++];
                                        else break;
                                    }

                                    if (!(letterIndex < letters.Length))
                                        if (!(wordIndex < words.Length - 1))
                                        {
                                            string token = "(" + "invalid-lexene, " + theString + ", " + index + ")";
                                            String.appendLine(ref tokenSet, token);
                                            isString = false;
                                            theString = string.Empty;
                                            goto IgnoreLine;
                                        }
                                        else
                                        {
                                            letters = words[wordIndex++].ToCharArray();
                                            continue;
                                        }

                                    if (letters[letterIndex] == '"')
                                    {
                                        theString += letters[letterIndex];
                                        string token = "(" + "string-constant, " + theString.Replace("-", " ") + ", " + index + ")";
                                        String.appendLine(ref tokenSet, token);
                                        isString = false;
                                        theString = string.Empty;
                                        continue;
                                    }

                                }

                                if (isMultiLineComment)
                                {

                                    if (letterIndex < letters.Length - 1 && letters[letterIndex] == '-' && letters[letterIndex + 1] == '|')
                                    {

                                        isMultiLineComment = false;
                                        letterIndex++;
                                        continue;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }



                                if (letterIndex < letters.Length - 1 && letters[letterIndex] == '|' && letters[letterIndex + 1] == '-')
                                {

                                    letterIndex++;
                                    isMultiLineComment = true;
                                    continue;
                                }

                                if (letterIndex < letters.Length - 1 && letters[letterIndex] == '/' && letters[letterIndex + 1] == '/')
                                {
                                    goto IgnoreLine;
                                }


                                if (letters[letterIndex] == '"')
                                {
                                    theString += letters[letterIndex];
                                    isString = true;
                                    continue;
                                }

                                int currentLetterIndex = letterIndex;
                                temporaryWord = string.Empty;
                                if (Char.isPunctuator(letters[letterIndex]))
                                {
                                    if ((currentLetterIndex < letters.Length) && Char.isSimpleArithmeticOperator(letters[currentLetterIndex]))
                                    {
                                        currentLetterIndex = ((currentLetterIndex > 0) ? currentLetterIndex - 1 : currentLetterIndex);
                                        if (letters[currentLetterIndex] == '+')
                                        {
                                            if (currentLetterIndex < letters.Length - 1 && letters[currentLetterIndex + 1] == '+')
                                            {
                                                temporaryWord += letters[currentLetterIndex];
                                                temporaryWord += letters[currentLetterIndex++];

                                                string token = generateTokenSet(temporaryWord, index);
                                                String.appendLine(ref tokenSet, token);
                                                letterIndex = currentLetterIndex;
                                                temporaryWord = string.Empty;
                                            }
                                        }

                                        if (letters[currentLetterIndex] == '-')
                                            if (currentLetterIndex < letters.Length - 1 && letters[currentLetterIndex + 1] == '-')
                                            {
                                                temporaryWord += letters[currentLetterIndex++];
                                                temporaryWord += letters[currentLetterIndex++];
                                                string token = generateTokenSet(temporaryWord, index);
                                                String.appendLine(ref tokenSet, token);
                                                letterIndex = currentLetterIndex;
                                                temporaryWord = string.Empty;
                                            }

                                        if (!(currentLetterIndex < letters.Length)) continue;
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
            IgnoreLine:
                continue;
            }
            Filling.Write("token-set.txt", tokenSet);
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
