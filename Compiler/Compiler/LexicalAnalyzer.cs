using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class LexicalAnalyzer
    {
        string[] code;
        public LexicalAnalyzer(string[] code)
        {
            this.code = code;
        }

        public void analyze(string outputFileName)
        {
            bool MultiLineComment = false;
            List<Token> tokens = new List<Token>();

            for (int lineCount = 0; lineCount < code.Length; lineCount++)
            {
                bool String = false;
                string StringConstant = string.Empty;

                bool SingleLineComment = false;

                string line = code[lineCount];

                if (line == string.Empty) continue;

                if (MultiLineComment)
                    if (line.Contains("-|"))
                    {
                        int index = line.IndexOf("-|");
                        line = line.Substring(index + 2, (line.Length - 1 - index - 2));
                        MultiLineComment = false;
                    }
                    else continue;

                string[] words = line.Split(' ');
                if (words.Length == 0) continue;

                for (int wordCount = 0; wordCount < words.Length; wordCount++)
                {
                    string word = words[wordCount];

                    if (SingleLineComment) continue;

                    if (String)
                        if (word.Contains('"'))
                        {
                            int index = word.IndexOf('"');
                            StringConstant += word.Substring(0, index + 1);
                            word = word.Substring(index + 1, (word.Length - 1 - index - 1));

                            Token token = new Token(ClassPart.StringConstant, StringConstant, (lineCount + 1));
                            tokens.Add(token);

                            StringConstant = string.Empty;
                            String = false;
                        }
                        else continue;

                    

                    if (Language.containsBreaker(word))
                    {
                        string _word = string.Empty;
                        for (int letterCount = 0; letterCount < word.Length; letterCount++)
                        {
                            char letter = word[letterCount];
                            if (Language.isPunctuator(letter))
                            {
                                switch (letter)
                                {
                                    case '{':
                                    case '}':
                                    case '(':
                                    case ')':
                                    case ',':
                                    case ':':
                                    case '.':
                                        if (String)
                                        {
                                            StringConstant += letter;
                                            continue;
                                        }
                                        if (!(_word == string.Empty))
                                        {
                                            Token _token = new Token(ClassPart.classPart(_word), _word, (lineCount + 1));
                                            tokens.Add(_token);
                                            _word = string.Empty;
                                        }
                                        Token token = new Token(letter, (lineCount + 1));
                                        tokens.Add(token);
                                        break;

                                    case '<':
                                    case '>':
                                    case '!':
                                    case '=':
                                    case '*':
                                    case '%':
                                        if (String)
                                        {
                                            StringConstant += letter;
                                            continue;
                                        }
                                        if (!(_word == string.Empty))
                                        {
                                            Token _token = new Token(ClassPart.classPart(_word), _word, (lineCount + 1));
                                            tokens.Add(_token);
                                            _word = string.Empty;
                                        }
                                        if (((letterCount + 1) < word.Length) && (word[letterCount + 1] == '='))
                                        {
                                            string temporary = letter.ToString() + '=';
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                            letterCount++;
                                        }
                                        else
                                        {
                                            string temporary = letter.ToString();
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                        }
                                        break;

                                    case '/':
                                        if (String)
                                        {
                                            StringConstant += letter;
                                            continue;
                                        }
                                        if (!(_word == string.Empty))
                                        {
                                            Token _token = new Token(ClassPart.classPart(_word), _word, (lineCount + 1));
                                            tokens.Add(_token);
                                            _word = string.Empty;
                                        }
                                        if (((letterCount + 1) < word.Length) && (word[letterCount + 1] == '='))
                                        {
                                            string temporary = letter.ToString() + '=';
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                            letterCount++;
                                        }
                                        else if (((letterCount + 1) < word.Length) && (word[letterCount + 1] == '/'))
                                        {
                                            SingleLineComment = true;
                                            continue;
                                        }
                                        else
                                        {
                                            string temporary = letter.ToString();
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                        }
                                        break;

                                    case '+':
                                    case '-':
                                        if (String)
                                        {
                                            StringConstant += letter;
                                            continue;
                                        }
                                        if (!(_word == string.Empty))
                                        {
                                            Token _token = new Token(ClassPart.classPart(_word), _word, (lineCount + 1));
                                            tokens.Add(_token);
                                            _word = string.Empty;
                                        }
                                        if (((letterCount + 1) < word.Length) && (word[letterCount + 1] == '='))
                                        {
                                            string temporary = letter.ToString() + '=';
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                            letterCount++;
                                        }
                                        else if (((letterCount + 1) < word.Length) && (word[letterCount + 1] == letter))
                                        {
                                            string temporary = letter.ToString() + letter;
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                            letterCount++;
                                        }
                                        else
                                        {
                                            string temporary = letter.ToString();
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                        }
                                        break;

                                    case '&':
                                    case '|':
                                        if (String)
                                        {
                                            StringConstant += letter;
                                            continue;
                                        }
                                        if (!(_word == string.Empty))
                                        {
                                            Token _token = new Token(ClassPart.classPart(_word), _word, (lineCount + 1));
                                            tokens.Add(_token);
                                            _word = string.Empty;
                                        }
                                        if (((letterCount + 1) < word.Length) && (word[letterCount + 1] == letter))
                                        {
                                            string temporary = letter.ToString() + letter;
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                            letterCount++;
                                        }
                                        else
                                        {
                                            string temporary = letter.ToString();
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                        }
                                        break;

                                    case '"':
                                        if (!(_word == string.Empty))
                                        {
                                            Token _token = new Token(ClassPart.classPart(_word), _word, (lineCount + 1));
                                            tokens.Add(_token);
                                            _word = string.Empty;
                                        }
                                        if (String)
                                        {
                                            StringConstant += letter;
                                            Token _token = new Token(ClassPart.StringConstant, StringConstant, (lineCount + 1));
                                            tokens.Add(_token);

                                            StringConstant = string.Empty;
                                            String = false;
                                            continue;
                                        }
                                        else
                                        {
                                            StringConstant += letter;
                                            String = true;
                                        }
                                        break;

                                    case '\'':
                                        if (String)
                                        {
                                            StringConstant += letter;
                                            continue;
                                        }
                                        if (!(_word == string.Empty))
                                        {
                                            Token _token = new Token(ClassPart.classPart(_word), _word, (lineCount + 1));
                                            tokens.Add(_token);
                                            _word = string.Empty;
                                        }
                                        if (((letterCount + 2) < word.Length) && (word[letterCount + 2] == '\''))
                                        {
                                            string temporary = letter.ToString() + word[letterCount + 1] + word[letterCount + 2];
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                            letterCount += 2;
                                        }
                                        else if (((letterCount + 3) < word.Length) && ((letterCount + 1) == '\\') &&(word[letterCount + 3] == '\''))
                                        {
                                            string temporary = letter.ToString() + word[letterCount + 1] + word[letterCount + 2] + word[letterCount + 3];
                                            Token _token = new Token(ClassPart.classPart(temporary), temporary, (lineCount + 1));
                                            tokens.Add(_token);
                                            letterCount += 3;
                                        }
                                        break;

                                    default:
                                        _word += letter;
                                        break;
                                }
                            }
                            else
                            {
                                _word += letter;
                            }
                        }

                        if (!(_word == string.Empty))
                        {
                            Token token = new Token(ClassPart.classPart(_word), _word, (lineCount + 1));
                            tokens.Add(token);
                            _word = string.Empty;
                        }
                    }
                    else
                    {
                        Token token = new Token(ClassPart.classPart(word), word, (lineCount + 1));
                        tokens.Add(token);
                    }

                    if (String)
                    {
                        Token token = new Token(ClassPart.classPart(StringConstant), word, (lineCount + 1));
                        tokens.Add(token);
                        StringConstant = string.Empty;
                        String = false;
                    }
                }
            }
            
            Token[] array = tokens.ToArray();
            string[] tokenset = new string[array.Length];
            for (int index = 0; index < array.Length; index++)
            {
                tokenset[index] = array[index].token;
                Console.WriteLine(tokenset[index]);
            }

            Filling.Write(outputFileName, tokenset);
        }
    }
}
