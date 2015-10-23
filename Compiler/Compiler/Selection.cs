using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Compiler
{
    class Selection
    {

        public string name;
        public string[] selections;

        public Selection(string name, string[] selections)
        {
            this.name = name;
            this.selections = selections;
        }

        public bool selectionExists(string selection)
        {
            foreach (string _selection in selections)
                if (_selection == selection)
                    return true;
            return false;
        }

        public bool selectionExists(ClassPart selection)
        {
            return selectionExists(selection.name);
        }

        public
        static
        Selection selection(ClassPart classpart, Selection[] set)
        {
            foreach (Selection selection in set)
                if (selection.name == classpart.name)
                    return selection;

            return null;
        }
        
    }

    enum SelectionType
    {
        Terminal, NonTerminal
    }
}
