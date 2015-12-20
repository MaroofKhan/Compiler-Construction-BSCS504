using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class ICGTree
    {
        public static ICGTree MainSyntaxTree = new ICGTree();

        private ICGTree()
        {
            intermediateCode = new List<string>();
            intermediate_code = string.Empty;
        }

        int tokenIndex;
        Token[] tokens;
        public List<string> intermediateCode;
        string intermediate_code;

        int labelCount = 0;
        int varCount = 0;

        void addToICG(string line) { intermediateCode.Add(line); }

        string generateLabel() { return "L" + labelCount++; }

        string generateVar() { return "t" + varCount++; }


        public int analyze(Token[] tokens)
        {
            this.tokens = tokens;
            this.tokenIndex = 0;

            if (start())
                return -1;
            else return tokenIndex;

        }
        bool checkIndex { get { return tokenIndex < tokens.Length; } }

        bool start()
        {
            if (tokenIndex < tokens.Length)
                if (globalBody())
                    return start();
                else return false;
            else return true;
        }
        bool globalBody()
        {
            int _tokenIndex = tokenIndex;
            if (declarations())
                return true;
            else if (tokenIndex == _tokenIndex && whenever())
                return true;
            else if (tokenIndex == _tokenIndex && loops())
                return true;
            else if (tokenIndex == _tokenIndex && starting_with_identifier())
                return true;
            else return false;
        }
        bool starting_with_identifier()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
            {
                tokenIndex++;
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
                    else if (checkIndex && tokens[tokenIndex].classpart.name == ")")
                    {
                        tokenIndex++;
                        return true;
                    }
                    else return false;
                }
                else if (checkIndex && tokens[tokenIndex].classpart.name == "[")
                {
                    tokenIndex++;
                    if (checkIndex && tokens[tokenIndex].classpart.name == "integer-constant")
                    {
                        tokenIndex++;
                        if (checkIndex && tokens[tokenIndex].classpart.name == "]")
                        {
                            tokenIndex++;
                            if (checkIndex && (tokens[tokenIndex].classpart.name == "direct-assignment-operator" || tokens[tokenIndex].classpart.name == "assignment-operator"))
                            {
                                tokenIndex++;
                                string name = null;
                                return (isExpression(ref name));
                            }
                            else return true;
                        }
                        else return false;
                    }
                    else if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
                    {
                        tokenIndex++;
                        if (checkIndex && tokens[tokenIndex].classpart.name == "]")
                        {
                            tokenIndex++;
                            if (checkIndex && (tokens[tokenIndex].classpart.name == "direct-assignment-operator" || tokens[tokenIndex].classpart.name == "assignment-operator"))
                            {
                                tokenIndex++;
                                string name = null;
                                return (isExpression(ref name));
                            }
                            else return true;
                        }
                        else if (checkIndex && tokens[tokenIndex].classpart.name == "unary-operator")
                        {
                            tokenIndex++;
                            if (checkIndex && tokens[tokenIndex].classpart.name == "]")
                            {
                                tokenIndex++;
                                if (checkIndex && (tokens[tokenIndex].classpart.name == "direct-assignment-operator" || tokens[tokenIndex].classpart.name == "assignment-operator"))
                                {
                                    tokenIndex++;
                                    string name = null;
                                    return (isExpression(ref name));
                                }
                                else return true;
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
                                if (checkIndex && (tokens[tokenIndex].classpart.name == "direct-assignment-operator" || tokens[tokenIndex].classpart.name == "assignment-operator"))
                                {
                                    tokenIndex++;
                                    string name = null;
                                    return (isExpression(ref name));
                                }
                                else return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else if (checkIndex && tokens[tokenIndex].classpart.name == "unary-operator")
                {
                    tokenIndex++;
                    if (checkIndex && (tokens[tokenIndex].classpart.name == "direct-assignment-operator" || tokens[tokenIndex].classpart.name == "assignment-operator"))
                    {
                        tokenIndex++;
                        string name = null;
                        return (isExpression(ref name));
                    }
                    else return true;
                }
                else if (checkIndex && tokens[tokenIndex].classpart.name == ".")
                {
                    tokenIndex++;
                    return starting_with_identifier();
                }
                else if (checkIndex && (tokens[tokenIndex].classpart.name == "direct-assignment-operator" || tokens[tokenIndex].classpart.name == "assignment-operator"))
                {
                    tokenIndex++;
                    string name = null;
                    return (isExpression(ref name));
                }
                else return false;
            }
            else return false;
        }
        bool after_assignment_operator_identifier(ref string name, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
            {
                if (op == null) name = tokens[tokenIndex].valuepart;
                else
                {
                    string _name = generateVar();
                    addToICG(_name + " " + "=" + " " + name + " " + op + " " + tokens[tokenIndex].valuepart);
                    name = _name;
                }
                tokenIndex++;
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
                else if (checkIndex && tokens[tokenIndex].classpart.name == "[")
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
                    string _name = null;
                    string _op = null;
                    return after_assignment_operator_identifier(ref _name, ref _op);
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
        bool assignment_to_identifier()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
            {
                tokenIndex++;
                if (checkIndex && tokens[tokenIndex].classpart.name == "unary-operator")
                {
                    tokenIndex++;
                    return true;
                }
                else if (checkIndex && tokens[tokenIndex].classpart.name == "assignment-operator")
                {
                    tokenIndex++;
                    string name = null;
                    if (isExpression(ref name))
                        return true;
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        bool passables()
        {
            if (passable())
            {
                if (checkIndex && tokens[tokenIndex].classpart.name == ",")
                {
                    tokenIndex++;
                    return passables();
                }
                else return true;
            }
            else return true;
        }
        bool passable()
        {
            string name = null;
            return isExpression(ref name);
        }
        bool declarations_with_access_modifiers()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "access-modifier")
            {
                tokenIndex++;
                if (declarations())
                    return true;
                else return false;
            }
            else return false;
        }
        bool declarations()
        {
            int _tokenIndex = tokenIndex;
            if (variable_declaration())
                return true;
            else if ((tokenIndex == _tokenIndex) && task_declaration())
                return true;
            else if ((tokenIndex == _tokenIndex) && class_declaration())
                return true;
            else if ((tokenIndex == _tokenIndex) && structure_declaration())
                return true;
            else return false;
        }
        bool loops()
        {
            return (considering() || repeat() || till());
        }

        bool considering()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "considering")
            {
                tokenIndex++;
                int _tokenIndex = tokenIndex;
                if (variable_declaration() || (tokenIndex == _tokenIndex))
                {
                    if (checkIndex && tokens[tokenIndex].classpart.name == ";")
                    {
                        string label = generateLabel();
                        string outLabel = generateLabel();
                        addToICG(label + ":");
                        tokenIndex++;
                        int __tokenIndex = tokenIndex;
                        string name = null;
                        if (isExpression(ref name) || (tokenIndex == __tokenIndex))
                        {
                            addToICG("if" + " " + "(" + name + " " + "==" + " " + "false" + ")" + " " + "jmp" + " " + outLabel);
                            if (checkIndex && tokens[tokenIndex].classpart.name == ";")
                            {
                                tokenIndex++;
                                int ___tokenIndex = tokenIndex;
                                if (assignment_to_identifier() || (tokenIndex == ___tokenIndex))
                                {
                                    if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                                    {
                                        tokenIndex++;
                                        if (innerMostBody() || true)
                                        {
                                            if (checkIndex && tokens[tokenIndex].classpart.name == "}")
                                            {
                                                addToICG("jmp" + " " + label);
                                                tokenIndex++;
                                                return true;
                                            }
                                            else return false;
                                        }
                                        else return false;
                                    }
                                    else return false;
                                }
                                else return false;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        bool repeat()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "repeat")
            {
                string label = generateLabel();
                addToICG(label + ":");
                tokenIndex++;
                if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                {
                    tokenIndex++;
                    int _tokenIndex = tokenIndex;
                    if (innerMostBody() || (tokenIndex == _tokenIndex))
                    {
                        if (checkIndex && tokens[tokenIndex].classpart.name == "}")
                        {
                            tokenIndex++;
                            if (checkIndex && tokens[tokenIndex].classpart.name == "till")
                            {
                                tokenIndex++;
                                string name = null;
                                if (isExpression(ref name))
                                {
                                    addToICG("if" + " " + "(" + name + " " + "==" + " " + "true" + ")" +" " + "jmp" + " " + label);
                                    return true;
                                }
                                else return false;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        bool till()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "till")
            {
                string label = generateLabel();
                string outLabel = generateLabel();
                addToICG(label + ":");

                tokenIndex++;
                string name = null;
                if (isExpression(ref name))
                {
                    addToICG("if" + " " + "(" + name + " " + "==" + " " + "false" + ")" + " " + "jmp" + " " + outLabel);
                    if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                    {
                        tokenIndex++;
                        int _tokenIndex = tokenIndex;
                        if (innerMostBody() || (tokenIndex == _tokenIndex))
                        {
                            if (checkIndex && tokens[tokenIndex].classpart.name == "}")
                            {
                                addToICG(outLabel + ":");
                                tokenIndex++;
                                return true;
                            }
                            else
                                return false;
                        }
                        else if (checkIndex && tokens[tokenIndex].classpart.name == "}")
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
            else return false;
        }
        bool innerMostBody()
        {
            int _tokenIndex = tokenIndex;
            if (variable_declaration())
                return innerMostBody();
            else if ((tokenIndex == _tokenIndex) && starting_with_identifier())
                return innerMostBody();
            else if ((tokenIndex == _tokenIndex) && loops())
                return innerMostBody();
            else if ((tokenIndex == _tokenIndex) && whenever())
                return innerMostBody();
            else return false;
        }
        bool whenever()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "whenever")
            {
                string label = generateLabel();
                string outLabel = generateLabel();
                addToICG(label + ":");
                tokenIndex++;
                string name = null;
                if (isExpression(ref name))
                {
                    addToICG("if" + " " + "(" + name + " " + "==" + " " + "false" + ")" + " " + "jmp" + " " + outLabel);
                    if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                    {
                        tokenIndex++;
                        if (innerMostBody() || true)
                        {
                            addToICG("jmp" + " " + label);
                            if (checkIndex && tokens[tokenIndex].classpart.name == "}")
                            {
                                tokenIndex++;
                                if (_else(ref outLabel) || true)
                                {
                                    if (outLabel != null)
                                        addToICG(outLabel + ":");
                                    return true;
                                }
                                    
                                else return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        bool _else(ref string label)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "else")
            {
                addToICG(label + ":");
                label = null;
                tokenIndex++;
                if (whenever())
                    return true;
                else if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                {
                    tokenIndex++;
                    if (innerMostBody() || true)
                    {
                        if (checkIndex && tokens[tokenIndex].classpart.name == "}")
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
            else return false;
        }

        #region Structure Declaration
        bool structure_declaration()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "structure")
            {
                tokenIndex++;
                if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
                {
                    tokenIndex++;
                    if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                    {
                        tokenIndex++;
                        int __tokenIndex = tokenIndex;
                        if (structure_body() || (tokenIndex == __tokenIndex))
                        {
                            if (checkIndex && tokens[tokenIndex].classpart.name == "}")
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
                else return false;
            }
            else return false;
        }
        bool structure_body()
        {
            int _tokenIndex = tokenIndex;
            if (declarations_with_access_modifiers())
                return structure_body();
            else if ((tokenIndex == _tokenIndex) && declarations())
                return structure_body();
            else return false;

        }
        #endregion
        #region Class Declaration
        bool class_declaration()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "class")
            {
                tokenIndex++;
                if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
                {
                    tokenIndex++;
                    if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                    {
                        tokenIndex++;
                        int __tokenIndex = tokenIndex;
                        if (class_body() || (tokenIndex == __tokenIndex))
                        {
                            if (checkIndex && tokens[tokenIndex].classpart.name == "}")
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
                else return false;
            }
            else return false;
        }
        bool class_body()
        {
            int _tokenIndex = tokenIndex;
            if (declarations_with_access_modifiers())
                return class_body();
            else if ((tokenIndex == _tokenIndex) && declarations())
                return class_body();
            else if (commence())
                return class_body();
            else return true;

        }
        bool commence()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "access-modifier")
            {
                tokenIndex++;
                if (commence_statement())
                    return true;
                else return false;
            }
            else return commence_statement();
        }
        bool commence_statement()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "commence")
            {
                tokenIndex++;
                if (checkIndex && tokens[tokenIndex].classpart.name == "(")
                {
                    tokenIndex++;
                    int _tokenIndex = tokenIndex;
                    if (parameters() || (tokenIndex == _tokenIndex))
                    {
                        if (checkIndex && tokens[tokenIndex].classpart.name == ")")
                        {
                            tokenIndex++;
                            if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                            {
                                tokenIndex++;
                                int __tokenIndex = tokenIndex;
                                if (task_body() || (tokenIndex == __tokenIndex))
                                {
                                    if (checkIndex && tokens[tokenIndex].classpart.name == "}")
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
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        #endregion
        #region Task Declaration
        bool task_declaration()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "task")
            {
                tokenIndex++;
                if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
                {
                    tokenIndex++;
                    if (checkIndex && tokens[tokenIndex].classpart.name == "(")
                    {
                        tokenIndex++;
                        int _tokenIndex = tokenIndex;
                        if (parameters() || (tokenIndex == _tokenIndex))
                        {
                            if (checkIndex && tokens[tokenIndex].classpart.name == ")")
                            {
                                tokenIndex++;
                                if (checkIndex && tokens[tokenIndex].classpart.name == "returns")
                                {
                                    tokenIndex++;
                                    if (checkIndex && (tokens[tokenIndex].classpart.name == "identifier" || tokens[tokenIndex].classpart.name == "data-type"))
                                    {
                                        tokenIndex++;
                                    }
                                    else if (checkIndex && (tokens[tokenIndex].classpart.name == "["))
                                    {
                                        tokenIndex++;
                                        if (checkIndex && (tokens[tokenIndex].classpart.name == "identifier" || tokens[tokenIndex].classpart.name == "data-type"))
                                        {
                                            tokenIndex++;
                                            if (checkIndex && (tokens[tokenIndex].classpart.name == "]"))
                                            {
                                                tokenIndex++;
                                            }
                                            else return false;
                                        }
                                        else return false;
                                    }
                                    else return false;
                                }
                                if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                                {
                                    tokenIndex++;
                                    int __tokenIndex = tokenIndex;
                                    if (task_body() || (tokenIndex == __tokenIndex))
                                    {
                                        if (checkIndex && tokens[tokenIndex].classpart.name == "}")
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
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        bool task_body()
        {
            int _tokenIndex = tokenIndex;
            if (variable_declaration())
                return task_body();
            else if ((tokenIndex == _tokenIndex) && starting_with_identifier())
                return task_body();
            else if ((tokenIndex == _tokenIndex) && loops())
                return task_body();
            else if ((tokenIndex == _tokenIndex) && whenever())
                return task_body();
            else if ((tokenIndex == _tokenIndex) && checkIndex && tokens[tokenIndex].classpart.name == "return")
            {
                tokenIndex++;
                string name = null;
                if (isExpression(ref name))
                    return task_body();
                else return false;
            }
            else return (tokenIndex == _tokenIndex);
        }

        #region Parameters
        bool parameters()
        {
            if (parameter())
            {
                if (checkIndex && tokens[tokenIndex].classpart.name == ",")
                {
                    tokenIndex++;
                    if (parameters())
                        return true;
                    else return false;
                }
                else return true;
            }
            else return false;
        }

        bool parameter()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
            {
                tokenIndex++;
                if (checkIndex && tokens[tokenIndex].classpart.name == ":")
                {
                    tokenIndex++;
                    if (checkIndex && (tokens[tokenIndex].classpart.name == "identifier" || tokens[tokenIndex].classpart.name == "data-type"))
                    {
                        tokenIndex++;
                        return true;
                    }
                    else if (checkIndex && (tokens[tokenIndex].classpart.name == "["))
                    {
                        tokenIndex++;
                        if (checkIndex && (tokens[tokenIndex].classpart.name == "identifier" || tokens[tokenIndex].classpart.name == "data-type"))
                        {
                            tokenIndex++;
                            if (checkIndex && (tokens[tokenIndex].classpart.name == "]"))
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
                else return false;
            }
            else return false;
        }
        #endregion
        #endregion
        #region Variable Declaration
        bool variable_declaration()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "variable")
            {
                tokenIndex++;
                /*if (checkIndex && variable_declaration_2())
                {
                    if (checkIndex && tokens[tokenIndex].classpart.name == "line-terminator")
                    {
                        tokenIndex++;
                        return true;
                    }
                    else return false;
                }
                else return false;*/
                return (checkIndex && variable_declaration_2());
            }
            else return false;
        }

        bool variable_declaration_2()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
            {
                tokenIndex++;
                int _tokenIndex = tokenIndex;
                if (checkIndex && variable_declaration_3())
                {
                    if (checkIndex && tokens[tokenIndex].classpart.name == ",")
                    {
                        tokenIndex++;
                        return (checkIndex && variable_declaration_2());
                    }
                    else return true;
                }
                else if ((tokenIndex == _tokenIndex) && checkIndex && variable_declaration_4())
                {
                    if (checkIndex && tokens[tokenIndex].classpart.name == ",")
                    {
                        tokenIndex++;
                        return (checkIndex && variable_declaration_2());
                    }
                    else return true;
                }
                else return false;
            }
            else return false;
        }

        bool variable_declaration_3()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == ":")
            {
                tokenIndex++;
                if (checkIndex && (tokens[tokenIndex].classpart.name == "data-type" || tokens[tokenIndex].classpart.name == "identifier"))
                {
                    tokenIndex++;
                    int _tokenIndex = tokenIndex;
                    if (checkIndex && variable_declaration_4())
                        return true;
                    else return (tokenIndex == _tokenIndex);
                }
                else return false;
            }
            else return false;
        }

        bool variable_declaration_4()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "direct-assignment-operator")
            {
                tokenIndex++;
                string name = null;
                if (checkIndex && isExpression(ref name))
                    return true;
                else return false;
            }
            else return false;
        }
        #endregion
        #region isExpression
        bool isExpression(ref string name)
        {
            string op = null;
            return F(ref name, ref op);
        }

        bool OE(ref string name, ref string op)
        {
            if (AE(ref name, ref op))
            {
                if (OE2(ref name, ref op))
                    return true;
                else return false;
            }
            else return false;
        }

        bool OE2(ref string name, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "logical-operator-OR")
            {
                op = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (AE(ref name, ref op))
                {
                    if (OE2(ref name, ref op))
                        return true;
                    else return false;
                }
                else return false;
            }
            else return true;
        }

        bool AE(ref string name, ref string op)
        {
            if (ROP(ref name, ref op))
            {
                if (AE2(ref name, ref op))
                    return true;
                else return false;
            }
            else return false;
        }

        bool AE2(ref string name, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "logical-operator-AND")
            {
                op = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (ROP(ref name, ref op))
                    if (AE2(ref name, ref op))
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool ROP(ref string name, ref string op)
        {
            if (E(ref name, ref op))
                if (ROP2(ref name, ref op))
                    return true;
                else return false;
            else return false;
        }

        bool ROP2(ref string name, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "relational-operator")
            {
                op = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (E(ref name, ref op))
                    if (ROP2(ref name, ref op))
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool E(ref string name, ref string op)
        {
            if (T(ref name, ref op))
                if (E2(ref name, ref op))
                    return true;
                else return false;
            else return false;
        }

        bool E2(ref string name, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "simple-arithmetic-operator")
            {
                op = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (T(ref name, ref op))
                    if (E2(ref name, ref op))
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool T(ref string name, ref string op)
        {
            if (F(ref name, ref op))
                if (T2(ref name, ref op))
                    return true;
                else return false;
            else return false;
        }

        bool T2(ref string name, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "arithmetic-operator")
            {
                op = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (F(ref name, ref op))
                    if (T2(ref name, ref op))
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool F(ref string name, ref string op)
{
            if (checkIndex && tokens[tokenIndex].classpart.name == "(")
            {
                string _name = null;
                string _op = null;
                tokenIndex++;
                if (OE(ref _name, ref _op))
                {
                    if (op == null) name = _name;
                    else
                    {
                        string __name = generateVar();
                        addToICG(__name + " " + "=" + " " + name + " " + op + " " + _name);
                        name = __name;
                    }
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
                if (F(ref name, ref op))
                    return true;
                else return false;
            }
            else if (after_assignment_operator_identifier(ref name, ref op))
            {
                return true;
            }
            else if (checkIndex && constant())
            {
                if (op == null) name = tokens[tokenIndex - 1].valuepart;
                else
                {
                    string _name = generateVar();
                    addToICG(_name + " " + "=" + " " + name + " " + op + " " + tokens[tokenIndex - 1].valuepart);
                    name = _name;
                }
                return true;
            }
            else return false;
        }

        bool constant()
        {
            if (
                tokens[tokenIndex].classpart.name == "integer-constant" ||
                tokens[tokenIndex].classpart.name == "float-constant" ||
                tokens[tokenIndex].classpart.name == "character-constant" ||
                tokens[tokenIndex].classpart.name == "string-constant" ||
                tokens[tokenIndex].classpart.name == "boolean-constant"
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
            string name = null;
            if (isExpression(ref name))
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
        #endregion
}

