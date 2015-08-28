using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Lexical_Analyzer
{
    class String
    {
        string value;

        public
        String(string value)
        {
            this.value = value;
        }

        public
        bool allAlphabets
        {
            get
            {
                return Regex.IsMatch(value, @"^[a-zA-Z]+$");
            }
        }

        public
        bool isPunctuator
        {
            get
            {
                return (value == "{" ||
                        value == "}" ||
                        value == "[" ||
                        value == "]" ||
                        value == "(" ||
                        value == ")" ||
                        value == "," ||
                        value == ":"
                        );
            }
        }
    }
}
