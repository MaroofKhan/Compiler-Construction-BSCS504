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
        ClassPart classpart;
        string valuepart;
        int line;

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
            if (Regex.IsMatch(token, @"^\(([a-zA-Z]*),(\s*)([a-zA-Z]*),(\s*)([0-9]*)\)$"))
            {
                token = token.Substring(1);
                token = token.Substring(0, token.Length - 1);
                token = token.Replace(" ", "");
                string[] parts = token.Split('~');

                this.classpart = ClassPart.classPart(parts[0]);
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
