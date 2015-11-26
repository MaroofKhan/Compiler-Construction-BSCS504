using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Compiler
{
    public abstract class Record
    {
        public string _identifier, _type;
        public int _scope;
    }
    public class VariableRecord: Record
    {
        public string identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }
        public int scope
        {
            get { return _scope; }
            set { _scope = value; }
        }
        
        public bool constant;
        
    }

    public class FunctionRecord: Record
    {
        public string identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public int scope
        {
            get { return _scope; }
            set { _scope = value; }
        }
        public string signature
        {
            get { return _type; }
            set
            { if (Regex.IsMatch(value, @"^([a-zA-Z]+)#([a-zA-Z]+,)*([a-zA-Z]+)*$")) _type = value;
                  else throw new Exception("Signature not vaid for a function."); }
        }

        public string returnType
        {
            get { return (_type.Split('#')[0]); }
        }

    }

    public class Request
    {
        public string type_1, _operator, type_2;
    }
}
