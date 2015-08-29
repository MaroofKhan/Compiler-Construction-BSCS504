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
                foreach (char character in value)
                {
                    if (!((character > 'a' && character < 'z') || (character > 'A' && character < 'Z')))
                        return false;
                }
                return true;
            }
        }

        public
        ClassPart classPart
        {
            get
            {
                foreach (ClassPart part in ClassPart.ClassParts)
                {
                    if (part.partExists(value))
                        return part;
                }
                return null;
            }
        }
    }
}
