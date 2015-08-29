using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class Char
    {
        char value;

        public
        Char(char value)
        {
            this.value = value;
        }

        public
        bool isPunctuator
        {
            get
            {
                return (value == '{' ||
                        value == '}' ||
                        value == '[' ||
                        value == ']' ||
                        value == '(' ||
                        value == ')' ||
                        value == ',' ||
                        value == ':'
                        );
            }
        }
    }
}
