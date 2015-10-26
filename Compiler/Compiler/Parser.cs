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
            if (Start())
                return -1;
            else return tokenIndex;
        }

        bool Start()
        {

            if (tokenIndex < tokens.Length)
                if (GlobalBody())
                    return Start();
                else return false;
            else return true;

        }

        bool GlobalBody()
        {
            int _tokenIndex = tokenIndex;
            if (Declarations())
                return true;
            else if (tokenIndex == _tokenIndex && Whenever())
                return true;
            else if (tokenIndex == _tokenIndex && Loops())
                return true;
            else if (tokenIndex == _tokenIndex && Assignment())
                return true;
            else return false;
        }

        bool AccessModifier()
        {
            if (index && tokens[tokenIndex] == "access-modifiers")
            {
                tokenIndex++;
                if (Declarations()) return true;
                else return false;
            }
            else return false;
        }

        bool Declarations()
        {
            int _tokenIndex = tokenIndex;
            if (VariableDeclaration())
                return true;
            else if (tokenIndex == _tokenIndex && TaskDeclaration())
                return true;
            else if (tokenIndex == _tokenIndex && ClassDeclaration())
                return true;
            else if (tokenIndex == _tokenIndex && StructureDeclaration())
                return true;
            else return false;
        }

        bool StructureDeclaration()
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
                        if (StructureBody())
                        {
                            if (index && tokens[tokenIndex] == "}")
                            {
                                tokenIndex++;
                                return true;
                            }
                            else
                                return false;
                        }
                        else if (index && tokens[tokenIndex] == "}")
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

        bool StructureBody()
        {
            int _tokenIndex = tokenIndex;
            if (AccessModifier())
                return StructureBody();
            else if (tokenIndex == _tokenIndex && Declarations())
                return StructureBody();
            else return false;
        }

        bool ClassDeclaration()
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
                        if (ClassBody() || true)
                        {
                            if (index && tokens[tokenIndex] == "}")
                            {
                                tokenIndex++;
                                return true;
                            }
                            else
                                return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        bool Commence()
        {
            if (index && tokens[tokenIndex] == "commence")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "(")
                {
                    tokenIndex++;
                    if (Parameters() || true)
                    {
                        if (index && tokens[tokenIndex] == ")")
                        {
                            tokenIndex++;
                            if (index && tokens[tokenIndex] == "{")
                            {
                                tokenIndex++;
                                if (InnerMostBody() || true)
                                {
                                    if (index && tokens[tokenIndex] == "}")
                                    {
                                        tokenIndex++;
                                        return true;
                                    }
                                    else
                                        return false;
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

        bool ClassBody()
        {
            int _tokenIndex = tokenIndex;
            if (AccessModifierClass())
                return ClassBody();
            else if (tokenIndex == _tokenIndex && DeclarationsClass())
                return ClassBody();
            else return false;
        }

        bool AccessModifierClass()
        {
            if (index && tokens[tokenIndex] == "access-modifiers")
            {
                tokenIndex++;
                if (DeclarationsClass()) return true;
                else return false;
            }
            else return false;
        }

        bool DeclarationsClass()
        {

            int _tokenIndex = tokenIndex;
            if (VariableDeclaration())
                return true;
            else if (tokenIndex == _tokenIndex && TaskDeclaration())
                return true;
            else if (tokenIndex == _tokenIndex && ClassDeclaration())
                return true;
            else if (tokenIndex == _tokenIndex && StructureDeclaration())
                return true;
            else if (tokenIndex == _tokenIndex && Commence())
                return true;
            else return false;

        }

        bool TaskDeclaration()
        { 
            if (index && tokens[tokenIndex] == "task")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "identifier")
                {
                    tokenIndex++;
                    if (index && tokens[tokenIndex] == "(")
                    {
                        tokenIndex++;
                        if (Parameters() || true)
                        {
                            if (index && tokens[tokenIndex] == ")")
                            {
                                tokenIndex++;
                                if (Returns() || true)
                                {
                                    if (index && tokens[tokenIndex] == "{")
                                    {
                                        tokenIndex++;
                                        if (InnerMostBody() || true)
                                        {
                                            if (index && tokens[tokenIndex] == "}")
                                            {
                                                tokenIndex++;
                                                return true;
                                            }
                                            else
                                                return false;
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

        bool Returns()
        {
            if (index && tokens[tokenIndex] == "returns")
            {
                tokenIndex++;
                if (index && (tokens[tokenIndex] == "identifier" || tokens[tokenIndex] == "data-type"))
                {
                    tokenIndex++;
                    return true;
                }
                else if (index && (tokens[tokenIndex] == "["))
                {
                    tokenIndex++;
                    if (index && (tokens[tokenIndex] == "identifier" || tokens[tokenIndex] == "data-type"))
                    {
                        tokenIndex++;
                        if (index && (tokens[tokenIndex] == "]"))
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

        bool Parameters()
        {
            if (Parameter())
            {
                if (index && tokens[tokenIndex] == ",")
                {
                    tokenIndex++;
                    if (Parameters())
                        return true;
                    else return false;
                }
                else return true;
            }
            else return false;
        }

        bool Parameter()
        {
            if (index && tokens[tokenIndex] == "identifier")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == ":")
                {
                    tokenIndex++;
                    if (index && (tokens[tokenIndex] == "identifier" || tokens[tokenIndex] == "data-type"))
                    {
                        tokenIndex++;
                        return true;
                    }
                    else if (index && (tokens[tokenIndex] == "["))
                    {
                        tokenIndex++;
                        if (index && (tokens[tokenIndex] == "identifier" || tokens[tokenIndex] == "data-type"))
                        {
                            tokenIndex++;
                            if (index && (tokens[tokenIndex] == "]"))
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

        bool InnerMostBody()
        {
            int _tokenIndex = tokenIndex;
            if (VariableDeclaration())
                return InnerMostBody();
            else if (tokenIndex == _tokenIndex && Whenever())
                return InnerMostBody();
            else if (tokenIndex == _tokenIndex && Loops())
                return InnerMostBody();
            else if (tokenIndex == _tokenIndex && Assignment())
                return InnerMostBody();
            else return false;

        }

        bool VariableDeclaration()
        {
            if (index && tokens[tokenIndex] == "variable")
            {
                tokenIndex++;
                if (VariableDeclaration_())
                    return true;
                else return false;
            }
            else return false;
        }

        bool VariableDeclaration_()
        {
            if (index && tokens[tokenIndex] == "identifier")
            {
                tokenIndex++;
                if (VariableDeclaration__())
                {
                    if (index && tokens[tokenIndex] == ",")
                    {
                        tokenIndex++;
                        if (VariableDeclaration_())
                            return true;
                        else return false;
                    }
                    else return true;
                }
                else return false;
            }
            else return false;
        }

        bool VariableDeclaration__()
        {
            if (index && tokens[tokenIndex] == ":")
            {
                tokenIndex++;
                if (index && (tokens[tokenIndex] == "identifier" || tokens[tokenIndex] == "data-type"))
                {
                    tokenIndex++;
                    if (VariableDeclaration___())
                        return true;
                    else return true;
                }
                else return false;
            }
            else if (index && (tokens[tokenIndex] == "["))
            {
                tokenIndex++;
                if (index && (tokens[tokenIndex] == "identifier" || tokens[tokenIndex] == "data-type"))
                {
                    tokenIndex++;
                    if (index && (tokens[tokenIndex] == "]"))
                    {
                        tokenIndex++;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else if (VariableDeclaration___())
                return true;
            else return false;
        }

        bool VariableDeclaration___()
        {
            if (index && tokens[tokenIndex] == "=")
            {
                tokenIndex++;
                if (Expression())
                    return true;
                else return false;
            }
            else return false;
        }

        bool AfterAssignment_()
        {
             if (Expression())
                return true;
            else return false;
        }

        bool AfterAssignmentID_()
        {
            if (index && tokens[tokenIndex] == "identifier")
            {
                tokenIndex++;
                if (Identifier_())
                    return true;
                else return true;
            }
            else return false;
        }

        bool Identifier_()
        {
            if (index && tokens[tokenIndex] == ".")
            {
                tokenIndex++;
                if (AfterAssignmentID_())
                    return true;
                else return false;
            }
            else if (index && tokens[tokenIndex] == "(")
            {
                tokenIndex++;
                if (Passables())
                {
                    if (index && tokens[tokenIndex] == ")")
                    {
                        tokenIndex++;
                        return true;
                    }
                    else return false;
                }
                else if (index && tokens[tokenIndex] == ")")
                {
                    tokenIndex++;
                    return true;
                }
                else return false;
            }
            else return false;
        }

        bool Passables()
        {
            if (index)
            {
                if (Expression())
                {
                    if (index && tokens[tokenIndex] == ",")
                    {
                        tokenIndex++;
                        if (Passables())
                            return true;
                        else return false;
                    }
                    else return true;
                }
                else return false;
            }
            else return false;
        }

        bool Whenever()
        {
            if (index && tokens[tokenIndex] == "whenever")
            {
                tokenIndex++;
                if (Condition())
                {
                    if (index && tokens[tokenIndex] == "{")
                    {
                        tokenIndex++;
                        if (InnerMostBody() || true)
                        {
                            if (index && tokens[tokenIndex] == "}")
                            {
                                tokenIndex++;
                                if (Else() || true)
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

        bool Else()
        {
            if (index && tokens[tokenIndex] == "else")
            {
                tokenIndex++;
                if (Whenever())
                    return true;
                else if (index && tokens[tokenIndex] == "{")
                {
                    tokenIndex++;
                    if (InnerMostBody() || true)
                    {
                        if (index && tokens[tokenIndex] == "}")
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

        bool Condition()
        {
            if (Expression())
                return true;
            else return false;
        }

        bool Loops()
        {
            return (Considering() || Repeat() || Till());
        }

        bool Considering()
        {
            if (index && tokens[tokenIndex] == "considering")
            {
                tokenIndex++;
                if (VariableDeclaration() || true)
                {
                    if (index && tokens[tokenIndex] == ";")
                    {
                        tokenIndex++;
                        if (Condition() || true)
                        {
                            if (index && tokens[tokenIndex] == ";")
                            {
                                tokenIndex++;
                                if (Assignment() || true)
                                {
                                    if (index && tokens[tokenIndex] == "{")
                                    {
                                        tokenIndex++;
                                        if (InnerMostBody() || true)
                                        {
                                            if (index && tokens[tokenIndex] == "}")
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

        bool Assignment()
        {
            if (BeforeAssignmentID_())
            {
                if (index && (tokens[tokenIndex] == "=" || tokens[tokenIndex] == "assignment-operators"))
                {
                    tokenIndex++;
                    if (AfterAssignment_())
                        return true;
                    else return false;
                }
                else if (index && tokens[tokenIndex] == "unary-operators")
                {
                    tokenIndex++;
                    return true;
                }
                else return false;
            }
            else return false;
        }

        bool BeforeAssignmentID_()
        {
            if (index && tokens[tokenIndex] == "identifier")
            {
                tokenIndex++;
                if (Identifier__())
                    return true;
                else return true;
            }
            else return false;
        }

        bool Identifier__()
        {
            if (index && tokens[tokenIndex] == ".")
            {
                tokenIndex++;
                if (BeforeAssignmentID_())
                    return true;
                else return false;
            }
            else return false;
        }

        bool Repeat()
        {
            if (index && tokens[tokenIndex] == "repeat")
            {
                tokenIndex++;
                if (index && tokens[tokenIndex] == "{")
                {
                    tokenIndex++;
                    if (InnerMostBody() || true)
                    {
                        if (index && tokens[tokenIndex] == "}")
                        {
                            tokenIndex++;
                            if (index && tokens[tokenIndex] == "till")
                            {
                                tokenIndex++;
                                if (Condition())
                                {
                                    return true;
                                }
                                else return false;
                            }
                            else return false;
                        }
                        else
                            return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        bool Till()
        {
            if (index && tokens[tokenIndex] == "till")
            {
                tokenIndex++;
                if (Condition())
                {
                    if (index && tokens[tokenIndex] == "{")
                    {
                        tokenIndex++;
                        if (InnerMostBody() || true)
                        {
                            if (index && tokens[tokenIndex] == "}")
                            {
                                tokenIndex++;
                                return true;
                            }
                            else
                                return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        bool index
        {
            get
            {
                return tokenIndex < tokens.Length;
            }
        }
        bool Expression()
        {
            return F();
        }

        bool OE()
        {
            if (AE())
            {
                if (OE2())
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }



        bool OE2()
        {
            if (index && tokens[tokenIndex] == "logical-operator-OR")
            {
                tokenIndex++;
                if (AE())
                {
                    if (OE2())
                    {
                        return true;
                    }
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
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        bool AE2()
        {
            if (index && tokens[tokenIndex] == "logical-operator-AND")
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
            if (index && tokens[tokenIndex] == "relational-operators")
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
            if (index && tokens[tokenIndex] == "simple-arithmetic-operators")
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
            if (index && tokens[tokenIndex] == "arithmetic-operators")
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
            if (index && tokens[tokenIndex] == "(")
            {
                tokenIndex++;
                if (OE())
                {
                    if (index && tokens[tokenIndex] == ")")
                    {
                        tokenIndex++;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else if (index && tokens[tokenIndex] == "!")
            {
                tokenIndex++;
                if (F())
                    return true;
                else return false;
            }
            else if (AfterAssignmentID_())
                return true;
            else if (index && Constant())
            {
                tokenIndex++;
                return true;
            }
            else return false;
        }

        bool Constant()
        {
            return (
                tokens[tokenIndex] == "integer-constant" ||
                tokens[tokenIndex] == "float-constant" ||
                tokens[tokenIndex] == "character-constant" ||
                tokens[tokenIndex] == "string-constant" ||
                tokens[tokenIndex] == "integer-array-constant"
                );
        }
    }
}
