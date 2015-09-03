using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class Char
    {
        public
        static
        bool isPunctuator (char character)
        {
            return (character == '{' ||
                    character == '}' ||
                    character == '[' ||
                    character == ']' ||
                    character == '(' ||
                    character == ')' ||
                    character == ',' ||
                    character == ':'
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

    }
}
