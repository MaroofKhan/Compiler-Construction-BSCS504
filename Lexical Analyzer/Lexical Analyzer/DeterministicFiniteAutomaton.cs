using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Lexical_Analyzer
{
    static 
    class DeterministicFiniteAutomaton
    {
        public
        static
        bool ValidateIdentifier(string Identifier)
        {
            return (Regex.IsMatch(Identifier, @"^([a-zA-Z]+)((_)*([a-zA-Z0-9]*))*$")
                    || Regex.IsMatch(Identifier, @"^[a-zA-Z]+$"));
        }

        public
        static
        bool ValidateInteger(string Int)
        {
            return Regex.IsMatch(Int, @"^[0-9]+$");
        }

        public
        static
        bool ValidateFloat(string Float)
        {
            return (Regex.IsMatch(Float, @"^[0-9]*(?:\.[0-9]+)?$") && !String.isEmpty(Float));
        }

        public
        static
        bool ValidateString(string String)
        {
            return (!(String == string.Empty) && (String.ToCharArray()[0] == '"' && String.ToCharArray()[String.ToCharArray().Length - 1] == '"'));
        }

        public
        static
        bool ValidateCharacter(string Char)
        {
            char[] characters = Char.ToCharArray();
            return (!(Char == string.Empty) && characters[0] == '\'' && ((characters[1] == '\\' && characters[3] == '\'') || characters[2] == '\''));
        }

        public
        static
        bool ValidateBoolean(string Bool)
        {
            switch (Bool)
            {
                case "YES":
                case "NO":
                    return true;
                default:
                    return false;
            }
        }
    }
}
