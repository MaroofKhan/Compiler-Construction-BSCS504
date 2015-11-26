using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class TestSemanticTree
    {

        public static TestSemanticTree x = new TestSemanticTree();
        private TestSemanticTree()
        {
            variableTable = new List<VariableRecord>();
            functionTable = new List<FunctionRecord>();
            scopeStack = new Stack<int>();
        }

        List<VariableRecord> variableTable;
        List<FunctionRecord> functionTable;
        Stack<int> scopeStack;
        
        int tokenIndex;
        Token[] tokens;

        int currentScope { get { return scopeStack.Peek(); } }

        int scopeOfVariable (string identifier)
        {
            foreach (VariableRecord record in variableTable)
            {
                if (record.identifier == identifier)
                    return record.scope;
            }
            return -1;
        }

        int scopeOfFunction (string identifier)
        {
            foreach (FunctionRecord record in functionTable)
            {
                if (record.identifier == identifier)
                    return record.scope;
            }
            return -1;
        }

        string compatible_type (string type_1, string _operator, string type_2)
        {
            if (_operator == "-" || _operator == "*" || _operator == "/" || _operator == "%" || _operator == "<" || _operator == ">" || _operator == "<=" || _operator == "=>")
                if (type_1 == "Int" || type_1 == "Float")
                    if (type_1 == type_2)
                        return type_1;
                    else return "Float";
                else return null;
            else if (_operator == "+")
                if (type_1 == "Int" || type_1 == "Float")
                    if (type_1 == type_2)
                        return type_1;
                    else return "Float";
                else if (type_1 == "String" || type_1 == "Char")
                    if (type_2 == "String" || type_2 == "Char")
                        return "String";
                    else return null;
                else return null;
            else if (_operator == "==" || _operator == "=" || _operator == "!=")
                if (type_1 == type_2)
                    return type_1;
                else return null;
            else if (_operator == "&&" || _operator == "||")
                if (type_1 == "Bool")
                    if (type_1 == type_2)
                        return type_1;
                    else return null;
                else return null;
            else return null;
        }

        bool checkIndex { get { return tokenIndex < tokens.Length; } }

        public int parse(Token[] tokens)
        {
            this.tokens = tokens;
            this.tokenIndex = 0;
            int index = this.parse();
            int i = 1;
            foreach (VariableRecord record in variableTable.ToArray())
            {
                Console.WriteLine(i++);
                Console.WriteLine(record.identifier);
                Console.WriteLine(record.type);
                Console.WriteLine(record.scope);
                Console.WriteLine(record.constant);
            }
            
            return index;
        }
        public int parse()
        {
            if (start())
                return -1;
            else return tokenIndex;
        }

        bool start()
        {
            scopeStack.Push(0);
            if (tokenIndex < tokens.Length)
                if (isExpression())
                    return start();
                else return false;
            else return true;
        }

        bool after_assignment_operator_identifier()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
            {
                tokenIndex++;
                /*
                if (checkIndex && tokens[tokenIndex].classpart.name == "(")
                {
                    tokenIndex++;
                    int _tokenIndex = tokenIndex;
                    if (passables() || (tokenIndex == _tokenIndex))
                    {
                        if (checkIndex && tokens[tokenIndex].classpart.name == ")")
                        {
                            tokenIndex++;
                            return true;
                        }
                        else return false;
                    }
                    else return false;
                }
                else */if (checkIndex && tokens[tokenIndex].classpart.name == "[")
                {
                    tokenIndex++;
                    if (checkIndex && tokens[tokenIndex].classpart.name == "integer-constant")
                    {
                        tokenIndex++;
                        if (checkIndex && tokens[tokenIndex].classpart.name == "]")
                        {
                            tokenIndex++;
                            return true;
                        }
                        else return false;
                    }
                    else if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
                    {
                        tokenIndex++;
                        if (checkIndex && tokens[tokenIndex].classpart.name == "]")
                        {
                            tokenIndex++;
                            return true;
                        }
                        else if (checkIndex && tokens[tokenIndex].classpart.name == "unary-operator")
                        {
                            tokenIndex++;
                            if (checkIndex && tokens[tokenIndex].classpart.name == "]")
                            {
                                tokenIndex++;
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else if (checkIndex && tokens[tokenIndex].classpart.name == "unary-operator")
                    {
                        tokenIndex++;
                        if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
                        {
                            tokenIndex++;
                            if (checkIndex && tokens[tokenIndex].classpart.name == "]")
                            {
                                tokenIndex++;
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else if (checkIndex && tokens[tokenIndex].classpart.name == ".")
                {
                    tokenIndex++;
                    return after_assignment_operator_identifier();
                }
                else if (checkIndex && tokens[tokenIndex].classpart.name == "unary-operator")
                {
                    tokenIndex++;
                    return true;
                }
                else return true;
            }
            else if (checkIndex && tokens[tokenIndex].classpart.name == "unary-operator")
            {
                tokenIndex++;
                if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
                {
                    tokenIndex++;
                    return true;
                }
                else return false;
            }
            else return false;
        }

        bool isExpression()
        {
            return F();
        }

        bool OE()
        {
            if (AE())
            {
                if (OE2())
                    return true;
                else return false;
            }
            else return false;
        }

        bool OE2()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "logical-operator-OR")
            {
                tokenIndex++;
                if (AE())
                {
                    if (OE2())
                        return true;
                    else return false;
                }
                else return false;
            }
            else return true;
        }

        bool AE()
        {
            if (ROP())
            {
                if (AE2())
                    return true;
                else return false;
            }
            else return false;
        }

        bool AE2()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "logical-operator-AND")
            {
                tokenIndex++;
                if (ROP())
                    if (AE2())
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool ROP()
        {
            if (E())
                if (ROP2())
                    return true;
                else return false;
            else return false;
        }

        bool ROP2()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "relational-operator")
            {
                tokenIndex++;
                if (E())
                    if (ROP2())
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool E()
        {
            if (T())
                if (E2())
                    return true;
                else return false;
            else return false;
        }

        bool E2()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "simple-arithmetic-operator")
            {
                tokenIndex++;
                if (T())
                    if (E2())
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool T()
        {
            if (F())
                if (T2())
                    return true;
                else return false;
            else return false;
        }

        bool T2()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "arithmetic-operator")
            {
                tokenIndex++;
                if (F())
                    if (T2())
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool F()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "(")
            {
                tokenIndex++;
                if (OE())
                {
                    if (checkIndex && tokens[tokenIndex].classpart.name == ")")
                    {
                        tokenIndex++;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else if (checkIndex && tokens[tokenIndex].classpart.name == "!")
            {
                tokenIndex++;
                if (F())
                    return true;
                else return false;
            }
            else if (after_assignment_operator_identifier())
                return true;
            else if (checkIndex && constant())
                return true;
            else return false;
        }

        bool constant()
        {
            if (
                tokens[tokenIndex].classpart.name == "integer-constant" ||
                tokens[tokenIndex].classpart.name == "float-constant" ||
                tokens[tokenIndex].classpart.name == "character-constant" ||
                tokens[tokenIndex].classpart.name == "string-constant"
                )
            { tokenIndex++; return true; }
            else if (array_constant())
                return true;
            else return false;
        }
        bool array_constant()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "[")
            {
                tokenIndex++;
                if (array_values())
                {
                    if (checkIndex && tokens[tokenIndex].classpart.name == "]")
                    {
                        tokenIndex++;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        bool array_values()
        {
            if (isExpression())
            {
                if (checkIndex && tokens[tokenIndex].classpart.name == ",")
                {
                    tokenIndex++;
                    return array_values();
                }
                else return true;
            }
            else return true;
        }

    }
}
