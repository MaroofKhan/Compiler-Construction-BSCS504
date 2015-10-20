using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexical_Analyzer
{
    class Parser
    {
        public string[] sentence;
        public int[] lineNumbers;
        int index = 0;
        List<int> error = new List<int>();
        public int[] parse()
        {
            int _index = sentence.Length;
            while (index < sentence.Length && _index > 0)
                if (!(dec() || var_call_object() || task_call_object() || task_call() || _struct() || _class() || assignment() || elemet_in_array() ||
                    whenever() || till_loop() || repeat_till_loop() || task()))
                {
                    error.Add(lineNumbers[index]);
                    int number = lineNumbers[index];
                    while (check && number == lineNumbers[index])
                        index++;
                    _index--;
                }

            return error.Distinct().ToList().ToArray();
        }

        bool check
        {
            get
            {
                return index < sentence.Length;
            }
        }

        //VAR-CALL-FROM-OBJ
        bool var_call_object()
        {
            if (check && ID(sentence[index]))
            {
                index++;
                if (check && (sentence[index] == "."))
                {
                    index++;
                    if (check && ID(sentence[index]))
                    {
                        index++;
                        return true;
                    }
                    index--;
                }
                index--;
            }
            return false;
        }


        //TASK-CALL-FROM-OBJ
        bool task_call_object()
        {
            if (check && ID(sentence[index]))
            {
                index++;
                if (check && (sentence[index] == "."))
                {
                    index++;
                    if (task_call())
                    {
                        return true;
                    }
                    index--;
                }
                index--;
            }
            return false;
        }

        //TASK-CALL
        bool task_call()
        {
            if (check && ID(sentence[index]))
            {
                index++;
                if (check && (sentence[index] == "("))
                {
                    index++;
                    if (parametersToPass())
                    {
                        if (check && (sentence[index] == ")"))
                        {
                            index++;
                            return true;
                        }
                    }
                    index--;
                }
                index--;
            }
            return false;
        }

        bool parametersToPass()
        {
            if (check && parameterToPass())
            {
                if (check && parameters2ToPass())
                {
                    return true;
                }
            }

            return true;
        }

        bool parameters2ToPass()
        {
            if (check && COMMA(sentence[index]))
            {
                index++;
                if (parameterToPass())
                {
                    if (parameters2ToPass())
                        return true;
                }
                index--;
            }

            return true;
        }

        bool parameterToPass()
        {
            if (check && ID_CONST(sentence[index]))
            {
                index++;
                return true;
            }
            return false;
        }

        //STRUCTURE
        bool _struct()
        {
            if (check && AM(sentence[index]))
            {
                index++;
                if (_struct2())
                {
                    return true;
                }
                index--;
            }
            return false;
        }

        bool _struct2()
        {
            if (check && (sentence[index] == "structure"))
            {
                index++;
                if (check && ID(sentence[index]))
                {
                    index++;
                    if (check && (sentence[index] == "{"))
                    {
                        index++;
                        if (check && (sentence[index] == "{"))
                        {
                            index++;
                            if (body())
                            {
                                if (check && (sentence[index] == "}"))
                                {
                                    index++;
                                    return true;
                                }
                            }
                            else
                            {
                                if (check && (sentence[index] == "}"))
                                {
                                    index++;
                                    return true;
                                }
                            }
                        }
                        index--;
                    }
                    index--;
                }
                index--;
            }
            return false;
        }


        //CLASS
        bool _class()
        {
            if (check && AM(sentence[index]))
            {
                index++;
                if (_class2())
                {
                    return true;
                }
                index--;
            }
            return false;
        }

        bool _class2()
        {
            if (check && (sentence[index] == "class"))
            {
                index++;
                if (check && ID(sentence[index]))
                {
                    index++;
                    if (check && (sentence[index] == "{"))
                    {
                        index++;
                        if (check && (sentence[index] == "{"))
                        {
                            index++;
                            if (bodyWithCommence())
                            {
                                if (check && (sentence[index] == "}"))
                                {
                                    index++;
                                    return true;
                                }
                            }
                            else
                            {
                                if (check && (sentence[index] == "}"))
                                {
                                    index++;
                                    return true;
                                }
                            }
                        }
                        index--;
                    }
                    index--;
                }
                index--;
            }
            return false;
        }

        

        bool bodyWithCommence()
        {
            if (commence())
            {
                if (body())
                {
                    if (bodyWithCommence())
                        return true;
                }
                else
                {
                    if (bodyWithCommence())
                        return true;
                }
            }

            return false;
        }

        bool commence()
        {
            if (check && COMMENCE(sentence[index]))
            {
                index++;
                if (check && (sentence[index] == "("))
                {
                    index++;
                    if (parameters())
                    {
                        if (check && (sentence[index] == ")"))
                        {
                            index++;
                            if (RETURN(sentence[index]))
                            {
                                index++;
                                if (check && (sentence[index] == "{"))
                                {
                                    index++;
                                    if (body())
                                    {
                                        if (check && (sentence[index] == "}"))
                                        {
                                            index++;
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        if (check && (sentence[index] == "}"))
                                        {
                                            index++;
                                            return true;
                                        }
                                    }
                                    index--;
                                }
                                index--;
                            }
                            else
                            {
                                if (check && (sentence[index] == "{"))
                                {
                                    index++;
                                    if (body())
                                    {
                                        if (check && (sentence[index] == "}"))
                                        {
                                            index++;
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        if (check && (sentence[index] == "}"))
                                        {
                                            index++;
                                            return true;
                                        }
                                    }
                                    index--;
                                }
                            }
                            index--;
                        }
                    }
                    index--;
                }
                index--;
            }
            return false;
        }

        //ASSIGNMENT
        bool assignment()
        {
            if (check && ID(sentence[index]))
            {
                index++;
                if (check && AOP(sentence[index]))
                {
                    index++;
                    if (check && ID_CONST(sentence[index]))
                    {
                        index++;
                        return true;
                    }
                    else if (check && UNARY(sentence[index]))
                    {
                        index++;
                        return true;
                    }
                    index--;
                }
                index--;
            }
            return false;
        }

        //ELEMENT-IN-ARRAY
        bool elemet_in_array()
        {
            if (check && ID(sentence[index]))
            {
                index++;
                if (check && (sentence[index] == "in"))
                {
                    index++;
                    if (check && (ID_CONST(sentence[index])))
                    {
                        index++;
                        if (check && (sentence[index] == "{"))
                        {
                            index++;
                            if (body())
                            {
                                if (check && (sentence[index] == "}"))
                                {
                                    index++;
                                    return true;
                                }
                            }
                            else
                            {
                                if (check && (sentence[index] == "}"))
                                {
                                    index++;
                                    return true;
                                }
                            }
                        }
                        index--;
                    }
                    index--;
                }
                index--;
            }
            return false;
        }


        //WHENEVER_ELSE
        bool whenever()
        {
            if (check && WHENEVER(sentence[index]))
            {
                index++;
                if (cond())
                {
                    if (check && (sentence[index] == "{"))
                    {
                        index++;
                        if (body())
                        {
                            if (check && (sentence[index] == "}"))
                            {
                                index++;
                                if (_else())
                                {
                                    return true;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (check && (sentence[index] == "}"))
                            {
                                index++;
                                if (_else())
                                {
                                    return true;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                        index--;
                    }
                }
                index--;
            }
            return false;
        }

        bool _else()
        {
            if (check && ELSE(sentence[index]))
            {
                index++;
                if (check && (sentence[index] == "{"))
                {
                    index++;
                    if (body())
                    {
                        if (check && (sentence[index] == "}"))
                        {
                            index++;
                            if (_else())
                            {
                                return true;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (check && (sentence[index] == "}"))
                        {
                            index++;
                            if (_else())
                            {
                                return true;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    index--;
                }
                else if (whenever())
                {
                    return true;
                }
                index--;
            }
            return false;
        }

        //REPEAT-TILL-LOOP
        bool repeat_till_loop()
        {
            if (check && REPAT(sentence[index]))
            {
                index++;
                if (check && (sentence[index] == "{"))
                {
                    index++;
                    if (body())
                    {
                        if (check && (sentence[index] == "}"))
                        {
                            index++;
                            return true;
                        }
                    }
                    else
                    {
                        if (check && (sentence[index] == "}"))
                        {
                            index++;
                            if (check && (TILL(sentence[index])))
                            {
                                index++;
                                if (check && cond())
                                {
                                    return true;
                                }
                                index--;
                            }
                            index--;
                        }
                    }
                    index--;
                }
                index--;
            }
            return false;
        }

        //TILL-LOOP
        bool till_loop()
        {
            if (check && TILL(sentence[index]))
            {
                index++;
                if (cond())
                {
                    if (check && (sentence[index] == "{"))
                    {
                        index++;
                        if (body())
                        {
                            if (check && (sentence[index] == "}"))
                            {
                                index++;
                                return true;
                            }
                        }
                        else
                        {
                            if (check && (sentence[index] == "}"))
                            {
                                index++;
                                return true;
                            }
                        }
                        index--;
                    }
                }
                index--;
            }

            return false;
        }

        bool cond()
        {
            if (check && ID_CONST(sentence[index]))
            {
                index++;
                if (cond2())
                    return true;
                return true;
            }
            return false;
        }

        bool cond2()
        {
            if (check && LO(sentence[index]))
            {
                index++;
                if (cond())
                    return true;
                index--;
            }
            return false;
        }

        //TASK
        bool task()
        {
            if (check && AM(sentence[index]))
            {
                index++;
                if (task2())
                    return true;
                index--;
            }

            else if (task2())
            {
                return true;
            }

            return false;
        }

        bool task2()
        {
            if (check && TASK(sentence[index]))
            {
                index++;
                if (check && ID(sentence[index]))
                {
                    index++;
                    if (check && (sentence[index] == "("))
                    {
                        index++;
                        if (parameters())
                        {
                            if (check && (sentence[index] == ")"))
                            {
                                index++;
                                if (RETURN(sentence[index]))
                                {
                                    index++;
                                    if (check && (sentence[index] == "{"))
                                    {
                                        index++;
                                        if (body())
                                        {
                                            if (check && (sentence[index] == "}"))
                                            {
                                                index++;
                                                return true;
                                            }
                                        }
                                        else
                                        {
                                            if (check && (sentence[index] == "}"))
                                            {
                                                index++;
                                                return true;
                                            }
                                        }
                                        index--;
                                    }
                                    index--;
                                }
                                else
                                {
                                    if (check && (sentence[index] == "{"))
                                    {
                                        index++;
                                        if (body())
                                        {
                                            if (check && (sentence[index] == "}"))
                                            {
                                                index++;
                                                return true;
                                            }
                                        }
                                        else
                                        {
                                            if (check && (sentence[index] == "}"))
                                            {
                                                index++;
                                                return true;
                                            }
                                        }
                                        index--;
                                    }
                                }
                                index--;
                            }
                        } 
                        index--;
                    }
                    index--;
                }
                index--;
            }
            return false;
        }

        bool body()
        {
            return multiplestatements();
        }

        bool multiplestatements()
        {
            if (singlestatement())
                if (multiplestatements())
                    return true;
            return false;
        }

        bool singlestatement()
        {
            if ((dec() || var_call_object() || task_call_object() || task_call() || _struct() || _class() || assignment() || elemet_in_array() ||
                    whenever() || till_loop() || repeat_till_loop() || task()))
                return true;
            return false;
        }

        bool parameters()
        {
            if (check && parameter())
            {
                if (check && parameters2())
                {
                    return true;
                }
            }

            return true;
        }

        bool parameters2()
        {
            if (check && COMMA(sentence[index]))
            {
                index++;
                if (parameter())
                {
                    if (parameters2())
                        return true;
                }
                index--;
            }

            return true;
        }

        bool parameter()
        {
            if (check && ID(sentence[index]))
            {
                index++;
                if (check && COLON(sentence[index]))
                {
                    index++;
                    if (check && DT(sentence[index]))
                    {
                        index++;
                        return true;
                    }
                    index--;
                }
                index--;
            }
            return false;
        }

        //DECLARATION
        bool dec()
        {
            if (check && AM(sentence[index]))
            {
                index++;
                if (dec2())
                {
                    return true;
                }
                index--;
            }
            else if (dec2())
            {
                return true;
            }
            return false;
        }

        bool dec2()
        {
            if (check && VAR(sentence[index]))
            {
                index++;
                return dec2sub();
            }
            return false;
        }

        bool dec2sub()
        {
            if (check && ID(sentence[index]))
            {
                index++;
                if (dec3())
                {
                    if (!check) return true;
                    if (check && COMMA(sentence[index]))
                    {
                        index++;
                        return dec2sub();
                    }
                    return true;
                }
                else if (dec4())
                {
                    return true;
                }

                index--;
            }
            return false;
        }

        bool dec3()
        {
            if (check && COLON(sentence[index]))
            {
                index++;
                if (check && DT(sentence[index]))
                {
                    index++;
                    if (!check) return true;
                    
                    if (dec4())
                        return true;

                    return true;
                }
                index--;
            }

            return false;
        }

        bool dec4()
        {
            if (check && DA(sentence[index]))
            {
                index++;
                if (check && ID_CONST/*_EXP*/(sentence[index]))
                {
                    index++;
                    return true;
                }
                index--;
            }
            return false;
        }

        bool COMMENCE(string word)
        {
            return word == "commence";
        }

        bool UNARY(string word)
        {
            return word == "unary-operators";
        }

        bool AOP(string word)
        {
            return word == "assignment-operators";
        }

        bool WHENEVER(string word)
        {
            return word == "whenever";
        }

        bool ELSE(string word)
        {
            return word == "else";
        }

        bool REPAT(string word)
        {
            return word == "repeat";
        }

        bool LO(string word)
        {
            return word == "logical-operators";
        }

        bool TILL(string word)
        {
            return word == "till";
        }

        bool RETURN(string word)
        {
            return word == "returns";
        }

        bool TASK(string word)
        {
            return word == "task";
        }

        bool ID_CONST/*_EXP*/(string word)
        {
            return (
                ID(word) ||
                CONST(word) /*||
                EXP (word)*/
                );
        }

        bool COMMA(string word)
        {
            return word == ",";
        }

        bool EXP(string word)
        {
            return true;
        }

        bool CONST(string word)
        {
            return (
                word == "integer-constant" ||
                word == "float-constant" ||
                word == "char-constant" ||
                word == "string-constant"
                );
        }

        bool DA(string word)
        {
            return word == "direct-assignment-operator";
        }

        bool DT(string word)
        {
            return word == "data-type";
        }

        bool COLON(string word)
        {
            return word == ":";
        }

        bool ID(string word)
        {
            return word == "identifier";
        }

        bool VAR(string word)
        {
            return word == "variable";
        }

        bool AM(string word)
        {
            return word == "access-modifiers";
        }

    }
}
