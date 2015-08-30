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
        static
        bool allAlphabets (string word)
        {
            return Regex.IsMatch(word, @"^[a-zA-Z]+$");
        }

        public
        static
        bool isEmpty(string word)
        {
            return (word == string.Empty);
        }

        public
        static
        void appendLine(ref string mainString, string stringToAppend)
        {
            mainString += stringToAppend + Environment.NewLine;
        }

        public
        static
        ClassPart classPart (string word)
        {
            foreach (ClassPart part in ClassPart.ClassParts)
            {
                if (part.partExists(word))
                return part;
            }
            return ClassPart.Identifier;
        }
    }
}
