using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class ClassPart
    {
        public
        static
        ClassPart DataTypes = new ClassPart("data-type", new string[] {"Int", "Float", "Char", "String", "Bool"});

        public
        static
        ClassPart Else = new ClassPart("else", new string[] { "else" });

        public
        static
        ClassPart Variable = new ClassPart("variable", new string[] { "var", "firm" });

        public
        static
        ClassPart Whenever = new ClassPart("whenever", new string[] { "whenever" });

        public
        static
        ClassPart AccessModifiers = new ClassPart("access-modifiers", new string[] { "public", "private", "internal" });

        public
        static
        ClassPart Self = new ClassPart("self", new string[] { "self" });

        public
        static
        ClassPart Till = new ClassPart("till", new string[] { "till" });

        public
        static
        ClassPart Commence = new ClassPart("commence", new string[] { "commence" });

        public
        static
        ClassPart In = new ClassPart("in", new string[] { "in" });

        public
        static
        ClassPart Repeat = new ClassPart("repeat", new string[] { "repeat" });

        public
        static
        ClassPart Structure = new ClassPart("structure", new string[] { "structure" });

        public
        static
        ClassPart Task = new ClassPart("task", new string[] { "task" });

        public
        static
        ClassPart Class = new ClassPart("class", new string[] { "class" });

        public
        static
        ClassPart Return = new ClassPart("return", new string[] { "return" });

        public
        static
        ClassPart Returns = new ClassPart("returns", new string[] { "returns" });

        public
        static
        ClassPart RelationalOperators = new ClassPart("relational-operators", new string[] { "<", ">", "<=", ">=", "!=" });

        public
        static
        ClassPart AssignmentOperators = new ClassPart("assignment-operators", new string[] { "=", "+=", "-=", "*=", "/=", "%="});

        public
        static
        ClassPart SimpleArithmeticOperators = new ClassPart("simple-arithmetic-operators", new string[] { "+", "-" });

        public
        static
        ClassPart ArithmeticOperators = new ClassPart("arithmetic-operators", new string[] { "*", "/", "%" });

        public
        static
        ClassPart LogicalOperators = new ClassPart("logical-operators", new string[] { "&&", "||" });

        public
        static
        ClassPart UnaryOperators = new ClassPart("unary-operators", new string[] { "++", "--" });

        public
        static
        ClassPart[] ClassParts = new ClassPart[] {
            DataTypes,
            Else,
            Variable,
            Whenever,
            AccessModifiers,
            Self,
            Till,
            Commence,
            In,
            Repeat,
            Structure,
            Task,
            Class,
            Return,
            Returns,
            RelationalOperators,
            AssignmentOperators,
            SimpleArithmeticOperators,
            ArithmeticOperators,
            LogicalOperators,
            UnaryOperators
        };

        public
        static
        ClassPart Identifier = new ClassPart("identifier", new string[] { });


        public
        string name;
        
        string[] parts;
 
        private
        ClassPart(string name, string[] parts)
        {
            this.name = name;
            this.parts = parts;
        }

        public
        bool partExists(string part)
        {
            return parts.Contains(part);
        }
    }
}
