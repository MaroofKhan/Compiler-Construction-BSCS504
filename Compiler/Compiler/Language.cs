using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    static class Language
    {

        public
        static
        string[] ACCESSMODIFIERS = { "access-modifiers", "NULL" };



        public
        static
        bool isPunctuator(char character)
        {
            return (character == '{' ||
                    character == '}' ||
                    character == '[' ||
                    character == ']' ||
                    character == '(' ||
                    character == ')' ||
                    character == ',' ||
                    character == ':' ||
                    character == '.' ||
                    character == '+' ||
                    character == '-' ||
                    character == '*' ||
                    character == '/' ||
                    character == '%' ||
                    character == '<' ||
                    character == '>' ||
                    character == '!' ||
                    character == '=' ||
                    character == '"' ||
                    character == '\'' ||
                    character == '&' ||
                    character == '|'
                    );
        }

        public
        static
        bool isRelationalOperator(char character)
        {
            return (
                character == '<' ||
                character == '>' ||
                character == '!'
                );
        }

        public
        static
        bool isAlphabet(char character)
        {

            return (
                    (
                        character >= 'a' &&
                        character <= 'z'
                    ) ||
                    (
                        character >= 'A' &&
                        character <= 'Z'
                    )
                );
        }

        public
        static
        bool isDigit(char character)
        {

            return (
                    character >= '0' &&
                    character <= '9'
                );
        }

        public
        static
        bool isSimpleArithmeticOperator(char character)
        {
            return (
                character == '+' ||
                character == '-'
                );
        }

        public
        static
        bool isArithmeticOperator(char character)
        {
            return (
                character == '*' ||
                character == '/' ||
                character == '%'
                );
        }

        public
        static
        bool isDirectAssignmentOperator(char character)
        {
            return (
                character == '='
                );
        }

        public
        static
        bool isLogicalOperator(string word)
        {
            return (word == "&&" || word == "||");
        }

        public
        static
        bool isLogicalOperator(char character)
        {
            return (character == '&' || character == '|');
        }

        public
        static
        bool containsBreaker(string word)
        {
            return (
                word.Contains('{') ||
                word.Contains('}') ||
                word.Contains('[') ||
                word.Contains(']') ||
                word.Contains('(') ||
                word.Contains(')') ||
                word.Contains(',') ||
                word.Contains(':') ||
                word.Contains('.') ||
                word.Contains('+') ||
                word.Contains('-') ||
                word.Contains('*') ||
                word.Contains('/') ||
                word.Contains('%') ||
                word.Contains('<') ||
                word.Contains('>') ||
                word.Contains('!') ||
                word.Contains('=') ||
                word.Contains('"') ||
                word.Contains('\'') ||
                word.Contains('&') ||
                word.Contains('|')
                );
        }

    }
}
