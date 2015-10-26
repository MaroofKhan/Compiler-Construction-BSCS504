using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Compiler
{
    class RegularExpression
    {
        public
        static
        bool ValidateIdentifier(string Identifier)
        {
            return (Regex.IsMatch(Identifier, @"^([a-zA-Z]+)((_)*([a-zA-Z0-9]*))*$") ||
                    Regex.IsMatch(Identifier, @"^\[([a-zA-Z]+)((_)*([a-zA-Z0-9]*))\]*$"));
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
            return (Regex.IsMatch(Float, @"^([0-9]*)?(\.[0-9]+)$"));
        }

        public
        static
        bool ValidateString(string String)
        {
            return (Regex.IsMatch(String, @"^""(.)*""$"));
        }

        public
        static
        bool ValidateCharacter(string Char)
        {
            return (Regex.IsMatch(Char, @"^'(\w)'$") || Regex.IsMatch(Char, @"^'\\[n,b,r,t]'$") || Regex.IsMatch(Char, @"^' '$"));
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
