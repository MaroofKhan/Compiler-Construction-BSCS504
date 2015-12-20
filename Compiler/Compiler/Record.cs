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
        protected string _identifier, _type;
        public int _scope;

    }

    /*
    public class Global : Record
    {
        public static Global OnlyInstance = new Global();

        private Global()
        {
            variableTable = new List<VariableRecord>();
            functionTable = new List<FunctionRecord>();
            classTable = new List<ClassRecord>();
            scopeStack = new Stack<int>();
        }

        List<VariableRecord> variableTable;
        List<FunctionRecord> functionTable;
        List<ClassRecord> classTable;
        public Stack<int> scopeStack;

        string identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }
        string type
        {
            get { return _type; }
            set { _type = value; }
        }

        int currentScope { get { return scopeStack.Peek(); } }

        public VariableRecord lookupVariable(string identifier)
        {
            foreach (VariableRecord record in variableTable)
                if (record.identifier == identifier) return record;
            return null;
        }

        public FunctionRecord lookupFunction(string identifier)
        {
            foreach (FunctionRecord record in functionTable)
                if (record.identifier == identifier) return record;
            return null;
        }

        public ClassRecord lookupClass(string identifier)
        {
            foreach (ClassRecord record in classTable)
                if (record.identifier == identifier) return record;
            return null;
        }

        public VariableRecord scopeOfVariable(string identifier)
        {
            foreach (VariableRecord record in variableTable)
            {
                if (record.identifier == identifier)
                    return record;
            }
            return null;
        }

        public int scopeOfFunction(string identifier)
        {
            foreach (FunctionRecord record in functionTable)
            {
                if (record.identifier == identifier)
                    return record.scope;
            }
            return -1;
        }

        bool existsInCurrentScope(Record record) { return (record._scope == currentScope); }
        void addVariable(VariableRecord variable) { variableTable.Add(variable); }
        void addFunction(FunctionRecord function) { functionTable.Add(function); }
        void addClass(ClassRecord _class) { classTable.Add(_class); }

    }
    */
    public class ErrorRecord : Record
    {

        internal ErrorRecord(string identifier, string type, Token token)
        {
            this.identifier = identifier;
            this.type = type;
            this.token = token;
        }

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

        internal Token _token { get { return token; } }

        Token token;

    }

    public class ClassRecord : Record
    {
        public string accessModifier;

        List<VariableRecord> variableTable;
        List<FunctionRecord> functionTable;
        List<ClassRecord> classTable;
        public Stack<int> scopeStack;

        public ClassRecord()
        {
            variableTable = new List<VariableRecord>();
            functionTable = new List<FunctionRecord>();
            classTable = new List<ClassRecord>();
            scopeStack = new Stack<int>();
        }

        public string identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }
        public string parent
        {
            get { return _type; }
            set { _type = value; }
        }
        public int scope
        {
            get { return _scope; }
            set { _scope = value; }
        }

        int currentScope { get { return scopeStack.Peek(); } }

        public VariableRecord lookupVariable(string identifier)
        {
            foreach (VariableRecord record in variableTable)
                if (record.identifier == identifier) return record;
            return null;
        }

        public FunctionRecord lookupFunction(string identifier)
        {
            foreach (FunctionRecord record in functionTable)
                if (record.identifier == identifier) return record;
            return null;
        }

        public ClassRecord lookupClass(string identifier)
        {
            foreach (ClassRecord record in classTable)
                if (record.identifier == identifier) return record;
            return null;
        }

        public VariableRecord scopeOfVariable(string identifier)
        {
            foreach (VariableRecord record in variableTable)
            {
                if (record.identifier == identifier)
                    return record;
            }
            return null;
        }

        public int scopeOfFunction(string identifier)
        {
            foreach (FunctionRecord record in functionTable)
            {
                if (record.identifier == identifier)
                    return record.scope;
            }
            return -1;
        }

        bool existsInCurrentScope(Record record) { return (record._scope == currentScope); }
        public void addVariable(VariableRecord variable) { variableTable.Add(variable); }
        public void addFunction(FunctionRecord function) { functionTable.Add(function); }
        public void addClass(ClassRecord _class) { classTable.Add(_class); }

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
