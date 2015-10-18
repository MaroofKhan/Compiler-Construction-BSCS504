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

        public
        static
        bool isAllAlphabets (string word)
        {
            return Regex.IsMatch(word, @"^[a-zA-Z]+$");
        }

        public
        static
        bool isAllDigits(string word)
        {
            return Regex.IsMatch(word, @"^[0-9]+$");
        }

        public
        static
        bool containsBreaker(string word)
        {
            return (
                word.Contains('+')  ||
                word.Contains('-')  ||
                word.Contains('*')  ||
                word.Contains('/')  ||
                word.Contains('.')  ||
                word.Contains('\t') ||
                word.Contains('\n') ||
                word.Contains('<')  ||
                word.Contains('>')  ||
                word.Contains('=')  ||
                word.Contains('&')  ||
                word.Contains('|')  ||
                word.Contains('!')  ||
                word.Contains('{')  ||
                word.Contains('}')  ||
                word.Contains('[')  ||
                word.Contains(']')  ||
                word.Contains('(')  ||
                word.Contains(')')  ||
                word.Contains(':')  ||
                word.Contains('"')  ||
                word.Contains(',')  
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
            
            if (DeterministicFiniteAutomaton.ValidateInteger(word))
            {
                ClassPart classPart = new ClassPart("integer-constant", new string[] { });
                return classPart;
            }
            else if (DeterministicFiniteAutomaton.ValidateFloat(word))
            {
                ClassPart classPart = new ClassPart("float-constant", new string[] { });
                return classPart;
            }
            else if (DeterministicFiniteAutomaton.ValidateBoolean(word))
            {
                ClassPart classPart = new ClassPart("boolean-constant", new string[] { });
                return classPart;
            }
            else if (DeterministicFiniteAutomaton.ValidateString(word))
            {
                ClassPart classPart = new ClassPart("string-constant", new string[] { });
                return classPart;
            }
            else if (DeterministicFiniteAutomaton.ValidateCharacter(word))
            {
                ClassPart classPart = new ClassPart("character-constant", new string[] { });
                return classPart;
            }

            foreach (ClassPart part in ClassPart.ClassParts)
            {
                if (part.partExists(word))
                return part;
            }

            if (DeterministicFiniteAutomaton.ValidateIdentifier(word))
            {
                ClassPart classPart = new ClassPart("identifier", new string[] { });
                return classPart;
            }



            return ClassPart.Invalid;
        }

        public
        static
        bool isAssignmentOperator(string word)
        {
            return (
                word == "+=" ||
                word == "-=" ||
                word == "*=" ||
                word == "/=" ||
                word == "%="
                );
        }

        public
        static
        bool isUnaryOperator(string word) 
        {
            return (
                word == "++" ||
                word == "--"
                );
        }
    }
}
