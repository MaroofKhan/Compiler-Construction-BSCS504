using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Parser
    {
        Token[] tokenSet;
        string[] tokens;
        int tokenIndex;

        public Parser(Token[] tokenSet)
        {
            this.tokenSet = tokenSet;
            List<string> tokens = new List<string>();
            foreach (Token token in tokenSet)
                tokens.Add(token.classpart.name);
            this.tokens = tokens.ToArray();
            tokenIndex = 0;
        }

        public Token parse(string damn)
        {
            if (START())
            {
                return null;
            } else {
                return tokenSet[tokenIndex];
            }
        }

        bool START()
        {
            if (DECLARATION() || VAR() || TASK() || CLASS() || STRUCTURE() || LOOPS() || ASSIGNMENT() || UNARY())
            {
                if (START())
                    return true;
                else if (!(tokenIndex < tokens.Length))
                    return true;
            }

            return false;
        }

        bool index
        {
            get
            {
                return tokenIndex < tokens.Length;
            }
        }

        bool DECLARATION()
        {
            if (index && tokens[tokenIndex] == "access-modifiers")
            {
                tokenIndex++;
                if (VAR() || TASK() || CLASS() || STRUCTURE())
                    return true;
                tokenIndex++;
            }

            return false;
        }

        bool STRUCTURE()
        {
            if (index && tokens[tokenIndex] == "structure")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "identifier")
                {
                    tokenIndex++;
                    if (index && tokens[tokenIndex] == "{")
                    {
                        tokenIndex++;
                        if (STRUCTBODY())
                        {
                            if (index && tokens[tokenIndex] == "}")
                            {
                                tokenIndex++;
                                return true;
                            }
                        }
                        tokenIndex--;
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }

            return false;
        }

        bool STRUCTBODY()
        {
            if (index && tokens[tokenIndex] == "access-modifiers")
            {
                tokenIndex++;
                if (VAR() || TASK())
                    if (STRUCTBODY())
                        return true;
                tokenIndex--;
            }
            else if (VAR() || TASK())
                if (STRUCTBODY())
                    return true;

            return true;
        }

        bool CLASS()
        {
            if (index && tokens[tokenIndex] == "class")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "identifier")
                {
                    tokenIndex++;
                    if (index && tokens[tokenIndex] == "{")
                    {
                        tokenIndex++;
                        if (CLASSBODY())
                        {
                            if (index && tokens[tokenIndex] == "}")
                            {
                                tokenIndex++;
                                return true;
                            }
                        }
                        tokenIndex--;
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }

            return false;
        }

        bool CLASSBODY()
        {
            if (index && tokens[tokenIndex] == "access-modifiers")
            {
                tokenIndex++;
                if (VAR() || TASK() || CLASS() || STRUCTURE() || COMMENCE())
                    if (CLASSBODY())
                        return true;
                tokenIndex--;
            }
            else if (VAR() || TASK() || CLASS() || STRUCTURE() || COMMENCE())
                if (CLASSBODY())
                    return true;

            return true;
        }

        bool COMMENCE()
        {
            
            if (index && tokens[tokenIndex] == "commence")
            {
                    tokenIndex++;
                    if (index && tokens[tokenIndex] == "(")
                    {
                        tokenIndex++;
                        if (PARAMS())
                        {
                            if (index && tokens[tokenIndex] == ")")
                            {
                                tokenIndex++;
                                if (index && tokens[tokenIndex] == "{")
                                {
                                    tokenIndex++;
                                    if (COREBODY())
                                    {
                                        if (index && tokens[tokenIndex] == "}")
                                        {
                                            tokenIndex++;
                                            return true;
                                        }
                                    }
                                    tokenIndex--;
                                }
                                tokenIndex--;
                            }
                        }
                        tokenIndex--;
                    }
                    tokenIndex--;
            }
            return false;
        }
        bool TASK()
        {
            if (index && tokens[tokenIndex] == "task")
            {
                tokenIndex++;
                if (index && Identifier())
                {
                    tokenIndex++;
                    if (index && tokens[tokenIndex] == "(")
                    {
                        tokenIndex++;
                        if (PARAMS())
                        {
                            if (index && tokens[tokenIndex] == ")")
                            {
                                tokenIndex++;
                                if (RETURNS())
                                {
                                    if (index && tokens[tokenIndex] == "{")
                                    {
                                        tokenIndex++;
                                        if (COREBODY())
                                        {
                                            if (index && tokens[tokenIndex] == "}")
                                            {
                                                tokenIndex++;
                                                return true;
                                            }
                                        }
                                        tokenIndex--;
                                    }
                                }
                                tokenIndex--;
                            }
                        }
                        tokenIndex--;
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }

            return false;
        }

        bool COREBODY()
        {
            if (VAR() || TASK() || ASSIGNMENT() || LOOPS() || UNARY() || OBJTASK() || OBJVAR())
            {
                if (COREBODY())
                    return true;
            }

            return true;
        }

        bool RETURNS()
        {
            if (index && tokens[tokenIndex] == "returns")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "data-type")
                {
                    tokenIndex++;
                    return true;
                }
                tokenIndex--;
            }
            return true;
        }

        bool PARAMS()
        {
            if (PARAM())
            {
                if (PARAM2())
                {
                    return true;
                }
            }

            return true;
        }

        bool PARAM()
        {
            if (index && tokens[tokenIndex] == "identifier")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == ":")
                {
                    tokenIndex++;
                    if (index && tokens[tokenIndex] == "data-type")
                    {
                        tokenIndex++;
                        return true;
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }

            return false;
        }

        bool PARAM2()
        {
            if (index && tokens[tokenIndex] == ",")
            {
                tokenIndex++;
                if (PARAM())
                {
                    return true;
                }
                tokenIndex--;
            }

            return true;
        }

        bool VAR()
        {

            if (index && tokens[tokenIndex] == "variable")
            {
                tokenIndex++;
                if (VAR2())
                {
                    return true;
                }
                tokenIndex--;
            }

            return false;
        }

        bool VAR2()
        {
            if (index && tokens[tokenIndex] == "identifier")
            {
                tokenIndex++;
                if (VAR3())
                {
                    if (VAR6())
                    {
                        return true;
                    }
                }
                tokenIndex--;
            }

            return false;
        }

        bool VAR3()
        {
            if (index && tokens[tokenIndex] == ":")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "data-type")
                {
                    tokenIndex++;
                    if (VAR5())
                    {
                        return true;
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }

            else if (VAR4())
            {
                return true;
            }

            return false;
        }

        bool VAR4()
        {
            if (index && tokens[tokenIndex] == "=")
            {
                tokenIndex++;
                if (INITIATION())
                {
                    tokenIndex++;
                    return true;
                }
                else if (index && Identifier() || Constant() || Expression())
                {
                    return true;
                }
                tokenIndex--;
            }

            return false;
        }

        bool VAR5()
        {
            if (VAR4())
            {
                return true;
            }
            return true;
        }

        bool VAR6()
        {
            if (index && tokens[tokenIndex] == ",")
            {
                tokenIndex++;
                if (VAR2())
                    return true;
                tokenIndex--;
            }

            return true;
        }

        bool LOOPS()
        {
            return (CONSIDERING() || TILL() || REPEAT() || ARRAYIN());
        }

        bool ARRAYIN()
        {
            if (index && Identifier())
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "in")
                {
                    tokenIndex++;
                    if (index && Constant())
                    {
                        tokenIndex++;
                        if (index && tokens[tokenIndex] == "{")
                        {
                            tokenIndex++;
                            if (COREBODY())
                            {
                                if (index && tokens[tokenIndex] == "}")
                                {
                                    tokenIndex++;
                                    return true;
                                }
                            }
                            tokenIndex--;
                        }
                        tokenIndex--;
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }
            return false;
        }

        bool CONSIDERING()
        {
            if (index && tokens[tokenIndex] == "considering")
            {
                tokenIndex++;
                if (VAR() || true)
                {
                    if (index && tokens[tokenIndex] == ";")
                    {
                        tokenIndex++;
                        if (CONDITION() || true)
                        {
                            if (index && tokens[tokenIndex] == ";")
                            {
                                tokenIndex++;
                                if (ASSIGNMENT() || UNARY() || true)
                                {
                                    if (index && tokens[tokenIndex] == "{")
                                    {
                                        tokenIndex++;
                                        if (COREBODY())
                                        {
                                            if (index && tokens[tokenIndex] == "}")
                                            {
                                                tokenIndex++;
                                                return true;
                                            }
                                        }
                                        tokenIndex--;
                                    }
                                }
                                tokenIndex--;
                            }
                        }
                        tokenIndex--;
                    }
                }
                tokenIndex--;
            }
            return false;
        }

        bool REPEAT()
        {
            if (index && tokens[tokenIndex] == "repeat")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "{")
                {
                    tokenIndex++;
                    if (COREBODY())
                    {
                        if (index && tokens[tokenIndex] == "}")
                        {
                            tokenIndex++;
                            if (index && tokens[tokenIndex] == "till")
                            {
                                tokenIndex++;
                                if (CONDITION())
                                {
                                    return true;
                                }
                                tokenIndex--;
                            }
                        }
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }
            return false;
        }

        bool TILL()
        {
            if (index && tokens[tokenIndex] == "till")
            {
                tokenIndex++;
                if (CONDITION())
                {
                    if (index && tokens[tokenIndex] == "{")
                    {
                        tokenIndex++;
                        if (COREBODY())
                        {
                            if (index && tokens[tokenIndex] == "}")
                            {
                                tokenIndex++;
                                return true;
                            }
                        }
                        tokenIndex--;
                    }
                }
                tokenIndex--;
            }

            return false;
        }

        bool CONDITION()
        {
            if (index && (Identifier() || Constant() || Expression()))
            {
                tokenIndex++;
                return true;
            }

            return false;
        }


        bool UNARY()
        {
            if (index && tokens[tokenIndex] == "identifier")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "unary-operators")
                {
                    tokenIndex++;
                    return true;
                }
                tokenIndex--;
            }
            return false;
        }
        bool ASSIGNMENT()
        {
            if (index && tokens[tokenIndex] == "identifier")
            {
                tokenIndex++;
                if (index && (tokens[tokenIndex] == "=" || tokens[tokenIndex] == "assignment-operators"))
                {
                    tokenIndex++;
                    if (Identifier() || Constant() || Expression())
                    {
                        tokenIndex++;
                        return true;
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }

            return false;
        }

        bool INITIATION()
        {
            if (index && Identifier())
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "(")
                {
                    tokenIndex++;
                    if (PASSABLE())
                    {
                        if (index && tokens[tokenIndex] == ")")
                        {
                            tokenIndex++;
                            return true;
                        }
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }

            return false;
        }

        bool TASKCALL()
        {
            if (index && Identifier())
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "(")
                {
                    tokenIndex++;
                    if (PASSABLE())
                    {
                        if (index && tokens[tokenIndex] == ")")
                        {
                            tokenIndex++;
                            return true;
                        }
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }
            return false;
        }

        bool OBJVAR()
        {
            if (index && Identifier())
            {
                tokenIndex++;
                if (OBJVAR2())
                    return true;
                tokenIndex--;
            }

            return false;
        }

        bool OBJVAR2()
        {
            if (index && tokens[tokenIndex] == ".")
            {
                tokenIndex++;
                if (index && Identifier())
                {
                    tokenIndex++;
                    if (OBJVAR2())
                        return true;
                    else
                        return true;
                }
                tokenIndex--;
            }
            return false ;
        }

        bool OBJTASK()
        {
            if (index && tokens[tokenIndex] == ".")
            {
                tokenIndex++;
                if (index && Identifier())
                {
                    tokenIndex++;
                    if (index && tokens[tokenIndex] == "(")
                    {
                        tokenIndex++;
                        if (PASSABLE())
                        {
                            if (index && tokens[tokenIndex] == ")")
                            {
                                tokenIndex++;
                                return true;
                            }
                        }
                        tokenIndex--;
                    }
                    tokenIndex--;
                }
                tokenIndex--;
            }
            return false;
        }

        bool PASSABLE()
        {
            if (index && (Identifier() || Constant() || Expression()))
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == ",")
                {
                    tokenIndex++;
                    if (PASSABLE())
                        return true;
                }
                else return true;
            }

            return true;
        }

        public Token parse()
        {
            if (VariableDeclaration())
            {
                Console.WriteLine(tokenIndex);
                Console.WriteLine(tokens.Length);

                if (tokenIndex < tokens.Length)
                    return tokenSet[tokenIndex];
                return null;
            }

            Console.WriteLine(tokenIndex);
            Console.WriteLine(tokens.Length);

            return tokenSet[tokenIndex];
        }

        bool VariableDeclaration()
        {
            if (AccessModifier())
            {
                tokenIndex++;
                if (Variable())
                {
                    tokenIndex++;
                    return (VariableDeclaration2());
                }
                tokenIndex--;
            }
            else if (Variable())
            {
                tokenIndex++;
                return (VariableDeclaration2());
            }

            return false;
        }

        bool VariableDeclaration2()
        {
            if (Identifier())
            {
                tokenIndex++;
                if (WithDT() || WithAssignment())
                {
                    if (Comma())
                    {
                        tokenIndex++;
                        if (VariableDeclaration2())
                        {
                            return true;
                        }
                        tokenIndex--;
                    }

                    else
                    {
                        return true;
                    }
                }
                tokenIndex--;
            }
            return false;
        }

        bool WithDT()
        {
            if (tokens[tokenIndex] == ":")
            {
                tokenIndex++;
                if (tokens[tokenIndex] == "data-type")
                {
                    tokenIndex++;
                    if (WithAssignment())
                    {
                        tokenIndex++;
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                tokenIndex--;
            }

            return false;
        }

        bool WithAssignment()
        {
            if (tokens[tokenIndex] == "direct-assignment-operator")
            {
                tokenIndex++;
                if (Identifier() || Constant() || Expression())
                {
                    tokenIndex++;
                    return true;
                }
                tokenIndex--;
            }

            return false;
        }

        bool Expression()
        {
            return false;
        }

        bool Comma()
        {
            return (tokens[tokenIndex] == ",");
        }

        bool Constant()
        {
            return (
                tokens[tokenIndex] == "integer-constant" ||
                tokens[tokenIndex] == "float-constant" ||
                tokens[tokenIndex] == "char-constant" ||
                tokens[tokenIndex] == "string-constant"
                );
        }
        bool AccessModifier()
        {
            return (tokens[tokenIndex] == "access-modifiers");
        }

        bool Variable()
        {
            return (tokens[tokenIndex] == "variable");
        }

        bool Identifier()
        {
            return (tokens[tokenIndex] == "identifier");
        }


    }
}
