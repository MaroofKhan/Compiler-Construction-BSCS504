using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
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
        ClassPart AccessModifiers = new ClassPart("access-modifier", new string[] { "public", "private", "internal" });

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
        ClassPart RelationalOperators = new ClassPart("relational-operator", new string[] { "<", ">", "<=", ">=", "!=", "!", "==" });

        public
        static
        ClassPart AssignmentOperators = new ClassPart("assignment-operator", new string[] { "+=", "-=", "*=", "/=", "%="});

        public
        static
        ClassPart DirectAssignmentOperators = new ClassPart("direct-assignment-operator", new string[] { "=" });


        public
        static
        ClassPart SimpleArithmeticOperators = new ClassPart("simple-arithmetic-operator", new string[] { "+", "-" });

        public
        static
        ClassPart ArithmeticOperators = new ClassPart("arithmetic-operator", new string[] { "*", "/", "%" });

        public
        static
        ClassPart LogicalOperatorOR = new ClassPart("logical-operator-OR", new string[] { "||" });

        public
        static
        ClassPart LogicalOperatorAND = new ClassPart("logical-operator-AND", new string[] { "&&" });

        public
        static
        ClassPart UnaryOperators = new ClassPart("unary-operator", new string[] { "++", "--" });

        public
        static
        ClassPart Considering = new ClassPart("considering", new string[] { "considering" });

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
            DirectAssignmentOperators,
            SimpleArithmeticOperators,
            ArithmeticOperators,
            LogicalOperatorOR,
            LogicalOperatorAND,
            UnaryOperators,
            Considering
        };

        public
        static
        ClassPart Invalid = new ClassPart("invalid-lexene", new string[] { });

        public
        static
        ClassPart IntegerConstant = new ClassPart("integer-constant", new string[] { });

        public
        static
        ClassPart FloatConstant = new ClassPart("float-constant", new string[] { });

        public
        static
        ClassPart StringConstant = new ClassPart("string-constant", new string[] { });

        public
        static
        ClassPart CharacterConstant = new ClassPart("character-constant", new string[] { });

        public
        static
        ClassPart BooleanConstant = new ClassPart("boolean-constant", new string[] { });

        public
        static
        ClassPart Identifier = new ClassPart("identifier", new string[] { });

        public
        static
        ClassPart classPart(string word)
        {

            if (RegularExpression.ValidateInteger(word)) return ClassPart.IntegerConstant;
            else if (RegularExpression.ValidateFloat(word)) return ClassPart.FloatConstant;
            else if (RegularExpression.ValidateBoolean(word)) return ClassPart.BooleanConstant;
            else if (RegularExpression.ValidateString(word)) return ClassPart.StringConstant;
            else if (RegularExpression.ValidateCharacter(word)) return ClassPart.CharacterConstant;

            foreach (ClassPart part in ClassPart.ClassParts)
                if (part.partExists(word)) return part;

            if (RegularExpression.ValidateIdentifier(word)) return ClassPart.Identifier;

            return ClassPart.Invalid;
        }

        public
        string name;
        
        string[] parts;
 
        public
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
