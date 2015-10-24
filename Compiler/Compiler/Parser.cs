using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Parser
    {
        public static Parser MainParser;
        public static void SetUpMainParser(Token[] tokenSet)
        {
            MainParser = new Parser(tokenSet);
        }

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

        public int parse()
        {

            if (START())
            {
                return -1;
            } else {
                return tokenIndex + 1;
                
            }
        }

        bool START()
        {
            if (GLOBALBODY())
            {
                if (START())
                    return true;
                else if (!(tokenIndex < tokens.Length))
                    return true;
            }

            return false;
        }

        bool GLOBALBODY()
        {
            return (DECLARATION() || VARIABLEDECLARATION() || TASK() || CLASS() || STRUCTURE() || ASSIGNMENT() || UNARY() || OBJMETHODS() || OBJPROPERTIES() || WHENEVER());
        }

        bool index
        {
            get
            {
                return tokenIndex < tokens.Length;
            }
        }

        bool WHENEVER()
        {
            if (index && tokens[tokenIndex] == "whenever")
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
                                if (ELSE())
                                {
                                    return true;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            else
                                return false;
                        }
                        tokenIndex--;
                    }
                }
                tokenIndex--;
            }
            return false;
        }

        bool ELSE()
        {
            if (index && tokens[tokenIndex] == "else")
            {
                tokenIndex++;
                if (WHENEVER())
                {
                    if (ELSE())
                    {
                        return true;
                    }

                    return true;
                }
                else
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

        bool DECLARATION()
        {
            if (index && tokens[tokenIndex] == "access-modifiers")
            {
                tokenIndex++;
                if (VARIABLEDECLARATION() || TASK() || CLASS() || STRUCTURE())
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
                            else
                                return false;
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
                if (VARIABLEDECLARATION() || TASK())
                    if (STRUCTBODY())
                        return true;
                tokenIndex--;
            }
            else if (VARIABLEDECLARATION() || TASK())
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
                            else
                                return false;
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
                if (VARIABLEDECLARATION() || TASK() || CLASS() || STRUCTURE() || COMMENCE())
                    if (CLASSBODY())
                        return true;
                tokenIndex--;
            }
            else if (VARIABLEDECLARATION() || TASK() || CLASS() || STRUCTURE() || COMMENCE())
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
                                        else
                                            return false;
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

        /* VARIABLE DECLARATION */
        bool VARIABLEDECLARATION()
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
                    return true;
                }
                else if (index && (Identifier() || Constant() || Expression()))
                {
                    tokenIndex++;
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

        /* ENDING */

        /* FUNCTON DECLARATION*/
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
                                            else
                                                return false;
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
            if (VARIABLEDECLARATION() || TASK() || ASSIGNMENT() || LOOPS() || UNARY() || OBJMETHODS() || OBJPROPERTIES() || WHENEVER())
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
                    if (PARAM2())
                        return true;
                }
                tokenIndex--;
            }

            return true;
        }

        /* ENDING*/

        /* LOOPS */
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
                                else
                                    return false;
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
                if (VARIABLEDECLARATION() || true)
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
                                            else
                                                return false;
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
                        else
                            return false;
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
                            else
                                return false;
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

        /* ENDING */

        /* ASSIGNMENTS */
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
                    if (Identifier() || Constant() || Expression() || INITIATION())
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

        /* ENDING */

        /* OBJECT'S METHOD AND PROPERTIES ACCESSING */
        bool OBJPROPERTIES()
        {
            if (OBJVAR())
            {
                if (index && (tokens[tokenIndex] == "=" || tokens[tokenIndex] == "assignment-operators"))
                {
                    tokenIndex++;
                    if (INITIATION() || Identifier() || Constant() || Expression())
                    {
                        tokenIndex++;
                        return true;
                    }
                    tokenIndex--;
                }
                else return true;
            }
            return false;
        }

        bool OBJMETHODS()
        {
            if (OBJVAR())
            {
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

        /* ENDING */

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
                    {
                        return true;
                    }
                    else return true;
                }
                tokenIndex--;
            }
            return true;
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

        bool Expression()
        {
            return false;
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
