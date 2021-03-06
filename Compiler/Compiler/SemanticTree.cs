﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class SemanticTree
    {
        public static SemanticTree MainSemanticTree = new SemanticTree();
        private SemanticTree()
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

        bool checkIndex { get { return tokenIndex < tokens.Length; } }

        #region Shutdown
        bool start()
        {
            scopeStack.Push(0);
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
                                return (isExpression());
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
                                return (isExpression());
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
                                    return (isExpression());
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
                                    return (isExpression());
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
                        return (isExpression());
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
                    return (isExpression());
                }
                else return false;
            }
            else return false;
        }
        bool after_assignment_operator_identifier()
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
                    if (isExpression())
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
            return isExpression();
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
                        tokenIndex++;
                        int __tokenIndex = tokenIndex;
                        if (isExpression() || (tokenIndex == __tokenIndex))
                        {
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
                                if (isExpression())
                                    return true;
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
                tokenIndex++;
                if (isExpression())
                {
                    if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                    {
                        tokenIndex++;
                        int _tokenIndex = tokenIndex;
                        if (innerMostBody() || (tokenIndex == _tokenIndex))
                        {
                            if (checkIndex && tokens[tokenIndex].classpart.name == "}")
                            {
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
                tokenIndex++;
                if (isExpression())
                {
                    if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                    {
                        tokenIndex++;
                        if (innerMostBody() || true)
                        {
                            if (checkIndex && tokens[tokenIndex].classpart.name == "}")
                            {
                                tokenIndex++;
                                if (_else() || true)
                                    return true;
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
        bool _else()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "else")
            {
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
        #endregion
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
                if (isExpression())
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
                VariableRecord record = new VariableRecord();
                record.constant = (tokens[tokenIndex].valuepart == "firm");
                record.scope = currentScope;

                tokenIndex++;
                #region Terminator
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
                #endregion
                return (checkIndex && variable_declaration_2(ref record));
            }
            else return false;
        }

        bool variable_declaration_2(ref VariableRecord record)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
            {
                record.identifier = tokens[tokenIndex].valuepart;
                tokenIndex++;
                int _tokenIndex = tokenIndex;
                if (checkIndex && variable_declaration_3(ref record))
                {
                    variableTable.Add(record);
                    if (checkIndex && tokens[tokenIndex].classpart.name == ",")
                    {
                        tokenIndex++;
                        
                        VariableRecord _record = new VariableRecord();
                        _record.scope = currentScope;
                        _record.constant = record.constant;

                        return (checkIndex && variable_declaration_2(ref _record));
                    }
                    else return true;
                }
                else if ((tokenIndex == _tokenIndex) && checkIndex && variable_declaration_4(ref record))
                {
                    variableTable.Add(record);
                    if (checkIndex && tokens[tokenIndex].classpart.name == ",")
                    {
                        tokenIndex++;

                        VariableRecord _record = new VariableRecord();
                        _record.scope = currentScope;
                        _record.constant = record.constant;

                        return (checkIndex && variable_declaration_2(ref _record));
                    }
                    else return true;
                }
                else return false;
            }
            else return false;
        }

        bool variable_declaration_3(ref VariableRecord record)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == ":")
            {
                tokenIndex++;
                if (checkIndex && (tokens[tokenIndex].classpart.name == "data-type" || tokens[tokenIndex].classpart.name == "identifier"))
                {
                    record.type = tokens[tokenIndex].valuepart;
                    tokenIndex++;
                    int _tokenIndex = tokenIndex;
                    if (checkIndex && variable_declaration_4(ref record))
                        return true;
                    else return (tokenIndex == _tokenIndex);
                }
                else return false;
            }
            else return false;
        }

        bool variable_declaration_4(ref VariableRecord record)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "direct-assignment-operator")
            {
                tokenIndex++;
                if (checkIndex && isExpression())
                    return true;
                else return false;
            }
            else return false;
        }
        #endregion
        #region isExpression
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
        #endregion
}
