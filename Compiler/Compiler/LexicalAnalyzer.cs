using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

                bool Char = false;
                string CharConstant = string.Empty;

                bool SingleLineComment = false;

                string line = code[lineCount];

                if (line == string.Empty) continue;

                if (MultiLineComment)
                    if (line.Contains("-|"))
                    {
                        int index = line.IndexOf("-|");
                        line = line.Substring(index + 2, (line.Length - index - 2));
                        MultiLineComment = false;
                    }
                    else continue;

                string[] words = line.Split(' ');
                if (words.Length == 0) continue;

                for (int wordCount = 0; wordCount < words.Length; wordCount++)
                {
                    string word = words[wordCount];

                    if (word == string.Empty) continue;

                    if (SingleLineComment) continue;

                    if (MultiLineComment)
                        if (word.Contains("-|"))
                        {
                            int index = word.IndexOf("-|");
                            Console.WriteLine(word.Length);
                            Console.WriteLine(index);
                            word = word.Substring(index + 2, (word.Length - index - 2));
                            Console.WriteLine(word);
                            MultiLineComment = false;
                        }
                        else continue;

                    if (String)
                        if (word.Contains('"'))
                        {
                            StringConstant += " ";
                            int index = word.IndexOf('"');
                            StringConstant += word.Substring(0, index + 1);
                            word = word.Substring(index + 1, (word.Length - 1 - index));

                            Token token = new Token(ClassPart.StringConstant, StringConstant, (lineCount + 1));
                            tokens.Add(token);

                            StringConstant = string.Empty;
                            String = false;

                            if (word == string.Empty)
                                continue;
                        }
                        else
                        {
                            StringConstant += " " + word;
                            continue;
                        }

                    if (Char)
                    {
                        CharConstant += ' ';
                    }


                    if (RegularExpression.ValidateFloat(word))
                    {
                        Token token = new Token(ClassPart.classPart(word), word, (lineCount + 1));
                        tokens.Add(token);
                        continue;
                    }
                    else if (Language.containsBreaker(word))
                    {
                        string _word = string.Empty;

                        for (int letterCount = 0; letterCount < word.Length; letterCount++)
                        {
                            char letter = word[letterCount];

                            string __word = word.Substring(letterCount, word.Length - letterCount);

                            if (MultiLineComment)
                            {
                                if (!(letter == '-'))
                                    continue;
                            }

                            if (SingleLineComment) continue;

                            if (Language.isPunctuator(letter))
                            {
                                switch (letter)
                                {
                                    case '.':
                                        if (String)
                                        {
                                            StringConstant += letter;
                                            continue;
                                        }

                                        if ((_word == string.Empty) || Regex.IsMatch(_word, @"^[0-9]+$"))
                                        {
                                            string _constant = _word + letter;
                                            string _LAD = string.Empty;
                                            int _i = 1;
                                            while (letterCount + _i < word.Length && Regex.IsMatch(word[letterCount + _i].ToString(), @"^[0-9]$"))
                                            {
                                                _LAD += word[letterCount + _i];
                                                _i++;
                                            }
                                            if (Regex.IsMatch(_LAD, @"^[0-9]+$"))
                                            {
                                                _constant = _constant + _LAD;
                                                Token t = new Token(ClassPart.classPart(_constant), _constant, (lineCount + 1));
                                                tokens.Add(t);
                                                letterCount += _i - 1;
                                                _word = string.Empty;
                                                continue;
                                            }
                                            else
                                            {
                                                if (!(_word == string.Empty))
                                                {
                                                    Token _t = new Token(ClassPart.classPart(_word), _word, (lineCount + 1));
                                                    tokens.Add(_t);
                                                }

                                                Token __t = new Token(letter, (lineCount + 1));
                                                tokens.Add(__t);

                                                _word = string.Empty;
                                            }
                                        }
                                        
                                        break;
                                    case '{':
                                    case '}':
                                    case '(':
                                    case ')':
                                    case ',':
                                    case ':':
                                    case '[':
                                    case ']':
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
                                            Token _token = new Token(ClassPart.classPart(letter.ToString()), letter.ToString(), (lineCount + 1));
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
                                        if (MultiLineComment)
                                        {
                                            if (letter == '-' && letterCount + 1 < word.Length && word[letterCount + 1] == '|')
                                            {
                                                MultiLineComment = false;
                                                letterCount += 1;
                                                continue;
                                            }
                                        }

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
                                        if (letter == '|' && letterCount + 1 < word.Length && word[letterCount + 1] == '-')
                                        {
                                            MultiLineComment = true;
                                            letterCount += 1;
                                            continue;
                                        }
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

                                        if (Char)
                                        {
                                            CharConstant += letter;
                                            Char = false;

                                            Token _token = new Token(ClassPart.classPart(CharConstant), CharConstant, (lineCount + 1));
                                            tokens.Add(_token);
                                            CharConstant = string.Empty;

                                        }
                                        else
                                        {
                                            if (!(letterCount + 1 < word.Length))
                                            {
                                                if (!(wordCount + 1 < words.Length))
                                                {
                                                    Token ___token = new Token(ClassPart.classPart("'"), "'", (lineCount + 1));
                                                    tokens.Add(___token);

                                                    Char = false;
                                                    CharConstant = string.Empty;
                                                    continue;
                                                }
                                                else
                                                {
                                                    CharConstant += letter;
                                                    Char = true;
                                                    continue;
                                                }
                                            }

                                            int _index = 1;
                                            string _temporary = letter.ToString();
                                            while ((letterCount + _index < word.Length) && word[letterCount + _index] != '\'' && _index < 3)
                                            {
                                                _temporary += word[letterCount + _index];
                                                _index++;
                                            }

                                            if (_temporary.Length == 1)
                                            {
                                                Token ___token = new Token(ClassPart.classPart(_temporary), _temporary, (lineCount + 1));
                                                tokens.Add(___token);
                                                CharConstant = string.Empty;
                                                Char = false;
                                                continue;
                                            }

                                            if ((!(_temporary[1] == '\\')) && (_temporary.Length > 3))
                                            {
                                                Token ___token = new Token(ClassPart.classPart("'"), "'", (lineCount + 1));
                                                tokens.Add(___token);

                                                Char = false;
                                                CharConstant = string.Empty;

                                                continue;
                                            }

                                            if ((letterCount + _index < word.Length) && word[letterCount + _index] == '\'')
                                                _temporary += '\'';

                                            Token __token = new Token(ClassPart.classPart(_temporary), _temporary, (lineCount + 1));
                                            tokens.Add(__token);
                                            letterCount += ((_index < 3) || (_temporary[_temporary.Length - 1] == '\'')) ? _index : _index - 1;
                                        }
                                        break;

                                    default:
                                        if (String)
                                        {
                                            StringConstant += letter;
                                            break;
                                        }

                                        _word += letter;
                                        break;
                                }
                            }
                            else
                            {
                                if (String)
                                {
                                    StringConstant += letter;
                                    continue;
                                }

                                if (Char)
                                {
                                    Token __token = new Token(ClassPart.classPart("'"), "'", (lineCount + 1));
                                    tokens.Add(__token);

                                    CharConstant = string.Empty;
                                    Char = false;
                                }

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
                        if (Char)
                        {
                            Token _token = new Token(ClassPart.classPart("'"), "'", (lineCount + 1));
                            tokens.Add(_token);

                            CharConstant = string.Empty;
                            Char = false;
                        }

                        Token token = new Token(ClassPart.classPart(word), word, (lineCount + 1));
                        tokens.Add(token);
                    }
                }
                
                //Token lineBreak = new Token(new ClassPart("line-break", new string[] {}), "", (lineCount + 1));
                //tokens.Add(lineBreak);
                
            }
            
            Token[] array = tokens.ToArray();
            string[] tokenset = new string[array.Length];
            for (int index = 0; index < array.Length; index++)
            {
                tokenset[index] = array[index].token;
                Console.WriteLine(index + ": " + tokenset[index]);
            }
            Filling.Write(outputFileName, tokenset);
        }
    }
}