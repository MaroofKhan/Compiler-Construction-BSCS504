using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class TestSemanticTree
    {
        public static TestSemanticTree MainSemanticTree = new TestSemanticTree();

        public List<ErrorRecord> errors;
        List<ClassRecord> classes;
        Stack<ClassRecord> classStack;
        List<VariableRecord> variables;
        List<FunctionRecord> functions;

        ClassRecord lookupClass(string identifier)
        {
         foreach (ClassRecord record in classes.ToArray())
            if (record.identifier == identifier) return record;
            return null;
        }

        VariableRecord lookupVariable(string identifier)
        {
            foreach (VariableRecord record in variables.ToArray())
                if (record.identifier == identifier) return record;
            return null;
        }

        FunctionRecord lookupFuncion(string identifier)
        {
            foreach (FunctionRecord record in functions.ToArray())
                if (record.identifier == identifier) return record;
            return null;
        }

        ClassRecord currentClass { get { return classStack.Peek(); } }

        TestSemanticTree()
        {
            errors = new List<ErrorRecord>();
            classStack = new Stack<ClassRecord>();
            classes = new List<ClassRecord>();

            variables = new List<VariableRecord>();
            functions = new List<FunctionRecord>();

            ClassRecord record = new ClassRecord();
            record.identifier = "global";
            classStack.Push(record);
        }
        
        int tokenIndex;
        Token[] tokens;

        bool check<T> (T value, T[] array) {
            foreach (T _value in array)
                if (value.Equals(_value)) return true;
            return false;
        }

        string compatible_type (string type_1, string _operator, string type_2)
        {
            if (check<string>(_operator, new string[] { "-", "*" }))
                if (type_1 == "Int" || type_1 == "Float")
                    if (type_1 == type_2)
                        return type_1;
                    else return "Float";
                else return null;
            else if (check<string>(_operator, new string[] { "+" }))
                if (type_1 == "Int" || type_1 == "Float")
                    if (type_1 == type_2)
                        return type_1;
                    else return "Float";
                else if (type_1 == "String" || type_1 == "Char")
                    if (type_2 == "String" || type_2 == "Char")
                        return "String";
                    else return null;
                else return null;
            else if (check<string>(_operator, new string[] { "/", "%" }))
                if (type_1 == "Int" || type_1 == "Float")
                    if (type_1 == type_2)
                        return type_1;
                    else return null;
                else return null;
            else if (check<string>(_operator, new string[] { "<", "<=", ">", ">=" }))
                if (type_1 == "Int" || type_1 == "Float")
                    if (type_1 == type_2)
                        return "Bool";
                    else return null;
                else return null;
            else if (check<string>(_operator, new string[] { "==", "!=" }))
                if (type_1 == type_2)
                    return "Bool";
                else return null;
            else if (check<string>(_operator, new string[] { "=" }))
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
                                string type = null;
                                return (isExpression(ref type));
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
                                string type = null;
                                return (isExpression(ref type));
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
                                    string type = null;
                                    return (isExpression(ref type));
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
                                    string type = null;
                                    return (isExpression(ref type));
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
                        string type = null;
                        return (isExpression(ref type));
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
                    string type = null;
                    return (isExpression(ref type));
                }
                else return false;
            }
            else return false;
        }
        bool after_assignment_operator_identifier()
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
            {
                string id = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (checkIndex && tokens[tokenIndex].classpart.name == "(")
                {
                    tokenIndex++;
                    int _tokenIndex = tokenIndex;
                    if (passables() || true)
                    {
                        if (checkIndex && tokens[tokenIndex].classpart.name == ")")
                        {
                            FunctionRecord f = lookupFuncion(id);
                            if (f == null)
                            {
                                errors.Add(new ErrorRecord("Undeclared Function", "Function named " + id + " has not been declared", tokens[tokenIndex]));
                                return false;
                            }
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
                            VariableRecord var = lookupVariable(id);
                            if (var == null)
                            {
                                errors.Add(new ErrorRecord("Undeclared Variable", "Variable named " + id + " has not been declared", tokens[tokenIndex]));
                                return false;
                            }
                            tokenIndex++;
                            return true;
                        }
                        else return false;
                    }
                    else if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
                    {
                        string _id = tokens[tokenIndex].valuepart;
                        tokenIndex++;
                        if (checkIndex && tokens[tokenIndex].classpart.name == "]")
                        {
                            VariableRecord var = lookupVariable(id);
                            if (var == null)
                            {
                                errors.Add(new ErrorRecord("Undeclared Variable", "Variable named " + id + " has not been declared", tokens[tokenIndex]));
                                return false;
                            }

                            VariableRecord _var = lookupVariable(_id);
                            if (_var == null)
                            {
                                errors.Add(new ErrorRecord("Undeclared Variable", "Variable named " + id + " has not been declared", tokens[tokenIndex]));
                                return false;
                            }

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
                    string type = null;
                    if (isExpression(ref type))
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
            string type = null;
            return (isExpression(ref type));
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
                        string type = null;
                        if (isExpression(ref type) || (tokenIndex == __tokenIndex))
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
                                string type = null;
                                if (isExpression(ref type))
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
                string type = null;
                if (isExpression(ref type))
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
                string type = null;
                if (isExpression(ref type))
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
                    string id = tokens[tokenIndex].valuepart;
                    ClassRecord record = lookupClass(tokens[tokenIndex].valuepart);
                    ClassRecord r = new ClassRecord();
                    if (record == null)
                    {
                        r.identifier = tokens[tokenIndex].valuepart;
                        classes.Add(r);
                        classStack.Push(r);
                    }
                    else
                    {
                        errors.Add(new ErrorRecord("Redeclared Class", "Class named " + id + " has already been declared" , tokens[tokenIndex]));
                        return false;
                    }
                    tokenIndex++;
                    if (checkIndex && tokens[tokenIndex].classpart.name == "{")
                    {
                        tokenIndex++;
                        int __tokenIndex = tokenIndex;
                        if (class_body() || true)
                        {
                            if (checkIndex && tokens[tokenIndex].classpart.name == "}")
                            {
                                classStack.Pop();
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
                    string type = null;
                    if (parameters(ref type) || (tokenIndex == _tokenIndex))
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
                    string id = tokens[tokenIndex].valuepart;
                    FunctionRecord record = currentClass.lookupFunction(tokens[tokenIndex].valuepart);
                    FunctionRecord r = new FunctionRecord();
                    if (record == null)
                    {
                        r.identifier = tokens[tokenIndex].valuepart;
                    }
                    else
                    {
                        errors.Add(new ErrorRecord("Function Redeclaration", "Function named " + id + " already declared " + (currentClass.identifier == "global" ? "" : "in class " + currentClass.identifier), tokens[tokenIndex]));
                        return false;
                    }
                    tokenIndex++;
                    if (checkIndex && tokens[tokenIndex].classpart.name == "(")
                    {
                        tokenIndex++;
                        int _tokenIndex = tokenIndex;
                        string type = null;
                        if (parameters(ref type) || (tokenIndex == _tokenIndex))
                        {
                            if (checkIndex && tokens[tokenIndex].classpart.name == ")")
                            {
                                tokenIndex++;
                                if (checkIndex && tokens[tokenIndex].classpart.name == "returns")
                                {
                                    tokenIndex++;
                                    if (checkIndex && (tokens[tokenIndex].classpart.name == "identifier" || tokens[tokenIndex].classpart.name == "data-type"))
                                    {
                                        type += "#" + tokens[tokenIndex].valuepart;
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
                                                type += "#[" + tokens[tokenIndex - 1].valuepart + "]";
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
                                    r.signature = (type == null) ? "#" : type;
                                    int __tokenIndex = tokenIndex;
                                    if (task_body() || true)
                                    {
                                        if (checkIndex && tokens[tokenIndex].classpart.name == "}")
                                        {
                                            currentClass.addFunction(r);
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
                string type = null;
                if (isExpression(ref type))
                    return task_body();
                else return false;
            }
            else return (tokenIndex == _tokenIndex);
        }

        #region Parameters
        bool parameters(ref string type)
        {
            if (parameter(ref type))
            {
                if (checkIndex && tokens[tokenIndex].classpart.name == ",")
                {
                    tokenIndex++;
                    type += ",";
                    if (parameters(ref type))
                        return true;
                    else return false;
                }
                else return true;
            }
            else return false;
        }

        bool parameter(ref string type)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
            {
                tokenIndex++;
                if (checkIndex && tokens[tokenIndex].classpart.name == ":")
                {
                    tokenIndex++;
                    if (checkIndex && (tokens[tokenIndex].classpart.name == "identifier" || tokens[tokenIndex].classpart.name == "data-type"))
                    {
                        type += tokens[tokenIndex].valuepart;
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
                return (checkIndex && variable_declaration_2(ref record));
            }
            else return false;
        }

        bool variable_declaration_2(ref VariableRecord record)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "identifier")
            {
                string id = tokens[tokenIndex].valuepart;
                record.identifier = tokens[tokenIndex].valuepart;
                tokenIndex++;
                int _tokenIndex = tokenIndex;
                if (checkIndex && variable_declaration_3(ref record))
                {
                    VariableRecord _record = currentClass.lookupVariable(record.identifier);
                    if (_record == null)
                        currentClass.addVariable(record);
                    else errors.Add(new ErrorRecord("Variable Redeclaration", "Variable " + id + " is already declared", tokens[tokenIndex]));
                    
                    if (checkIndex && tokens[tokenIndex].classpart.name == ",")
                    {
                        tokenIndex++;
                        return (checkIndex && variable_declaration_2(ref record));
                    }
                    else return true;
                }
                else if ((tokenIndex == _tokenIndex) && checkIndex && variable_declaration_4(ref record))
                {
                    VariableRecord _record = currentClass.lookupVariable(record.identifier);
                    if (_record == null)
                        currentClass.addVariable(record);
                    else errors.Add(new ErrorRecord("Variable Redeclaration", "Variable " + id + " is already declared", tokens[tokenIndex]));
                    
                    if (checkIndex && tokens[tokenIndex].classpart.name == ",")
                    {
                        tokenIndex++;
                        return (checkIndex && variable_declaration_2(ref record));
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
                    if (tokens[tokenIndex].classpart.name == "identifier")
                    {
                        ClassRecord _record = lookupClass(tokens[tokenIndex].valuepart);

                        if (_record == null)
                            errors.Add(new ErrorRecord("Undeclared Data-Type", "Type " + tokens[tokenIndex].valuepart + " is undeclared", tokens[tokenIndex]));

                    }
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
                string type = null;
                if (checkIndex && isExpression(ref type))
                {
                    if (record.type == null) record.type = type;
                    else if (!(record.type == type))
                    {
                        errors.Add(new ErrorRecord("Type Mismatch", "Type of the value of " + tokens[tokenIndex].valuepart + " is not a" + tokens[tokenIndex].classpart.name, tokens[tokenIndex]));
                        return false;
                    }

                    return true;
                }
                else return false;
            }
            else return false;
        }
        #endregion
        #region isExpression
        bool isExpression(ref string type)
        {
            string op = null;
            return F(ref type, ref op);
        }

        bool OE(ref string type, ref string op)
        {
            if (AE(ref type, ref op))
            {
                if (OE2(ref type, ref op))
                    return true;
                else return false;
            }
            else return false;
        }

        bool OE2(ref string type, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "logical-operator-OR")
            {
                op = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (AE(ref type, ref op))
                {
                    if (OE2(ref type, ref op))
                        return true;
                    else return false;
                }
                else return false;
            }
            else return true;
        }

        bool AE(ref string type, ref string op)
        {
            if (ROP(ref type, ref op))
            {
                if (AE2(ref type, ref op))
                    return true;
                else return false;
            }
            else return false;
        }

        bool AE2(ref string type, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "logical-operator-AND")
            {
                op = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (ROP(ref type, ref op))
                    if (AE2(ref type, ref op))
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool ROP(ref string type, ref string op)
        {
            if (E(ref type, ref op))
                if (ROP2(ref type, ref op))
                    return true;
                else return false;
            else return false;
        }

        bool ROP2(ref string type, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "relational-operator")
            {
                op = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (E(ref type, ref op))
                    if (ROP2(ref type, ref op))
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool E(ref string type, ref string op)
        {
            if (T(ref type, ref op))
                if (E2(ref type, ref op))
                    return true;
                else return false;
            else return false;
        }

        bool E2(ref string type, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "simple-arithmetic-operator")
            {
                op = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (T(ref type, ref op))
                    if (E2(ref type, ref op))
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool T(ref string type, ref string op)
        {
            if (F(ref type, ref op))
                if (T2(ref type, ref op))
                    return true;
                else return false;
            else return false;
        }

        bool T2(ref string type, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "arithmetic-operator")
            {
                op = tokens[tokenIndex].valuepart;
                tokenIndex++;
                if (F(ref type, ref op))
                    if (T2(ref type, ref op))
                        return true;
                    else return false;
                else return false;
            }
            else return true;
        }

        bool F(ref string type, ref string op)
        {
            if (checkIndex && tokens[tokenIndex].classpart.name == "(")
            {
                tokenIndex++;
                if (OE(ref type, ref op))
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
                if (F(ref type, ref op))
                    return true;
                else return false;
            }
            else if (after_assignment_operator_identifier())
            {
                string type_;
                VariableRecord record = currentClass.scopeOfVariable(tokens[tokenIndex].valuepart);
                if (record == null)
                {
                    errors.Add(new ErrorRecord("Undeclared Variable", ("Variable " + tokens[tokenIndex].valuepart + " is undeclared"), tokens[tokenIndex]));
                    return false;
                }
                else type_ = record.type;

                if (op == null) type = type_;
                else if (compatible_type(type, op, type_) == null)
                {
                    errors.Add(new ErrorRecord("Incompatable Type", "Type Incompatability", tokens[tokenIndex]));
                    return false;
                }
                else type = compatible_type(type, op, type_);
                return true;
            }
            else if (checkIndex && constant())
            {
                string type_;
                switch (tokens[tokenIndex - 1].classpart.name)
                {
                    case "integer-constant":
                        type_ = "Int";
                        break;
                    case "float-constant":
                        type_ = "Float";
                        break;
                    case "character-constant":
                        type_ = "Char";
                        break;
                    case "string-constant":
                        type_ = "String";
                        break;
                    default:
                        type_ = "Bool";
                        break;
                }

                if (op == null) type = type_;
                else
                {
                    string _type = compatible_type(type, op, type_);
                    if (_type == null)  errors.Add(new ErrorRecord("Incompatable Type", "Type Incompatability", tokens[tokenIndex - 1]));
                    else
                    {
                        type = _type;
                        op = null;
                    }
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
            string x = null;
            if (isExpression(ref x))
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
        #endregion

    }
}
