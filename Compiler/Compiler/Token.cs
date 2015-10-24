using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Compiler
{
    class Token
    {
        public ClassPart classpart;
        public string valuepart;
        public int line;

        public Token(ClassPart classpart, string valuepart, int line)
        {
            this.classpart = classpart;
            this.valuepart = valuepart;
            this.line = line;
        }

        public Token(char valuepart, int line)
        {
            this.classpart = new ClassPart(valuepart.ToString(), new string[] { });
            this.valuepart = valuepart.ToString();
            this.line = line;
        }

        public Token(string token)
        {
            if (Regex.IsMatch(token, @"^\((,*),(\s*)(,*),(\s*)([0-9]*)\)$"))
            {
                this.classpart = new ClassPart(",", new string[] { });
                this.valuepart = ",";
                token = token.Substring(1);
                token = token.Substring(0, token.Length - 1);
                token = token.Replace(" ", "").Replace(",", "");
                this.line = Convert.ToInt32(token);
            }
            else if (Regex.IsMatch(token, @"^\(((.)*),(\s*)((.)*),(\s*)([0-9]*)\)$"))
            {
                token = token.Substring(1);
                token = token.Substring(0, token.Length - 1);
                token = token.Replace(" ", "");
                string[] parts = token.Split(',');

                this.classpart = new ClassPart(parts[0], new string[] { });
                this.valuepart = parts[1];
                this.line = Convert.ToInt32(parts[2]);

            }
            
            else throw new ArgumentException("DAMN!\nToken ain't the right format.");
        }

        public string token
        {
            get
            {
                return ("(" + classpart.name + "," + valuepart + "," + line + ")");
            }
        }

    }
}
