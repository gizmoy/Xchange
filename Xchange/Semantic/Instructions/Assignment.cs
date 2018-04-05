using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Entities;
using Xchange.Semantic.Interfaces;

namespace Xchange.Semantic.Instructions
{
    public class Assignment : IInstruction
    {
        public Variable Variable { get; set; } = new Variable();
        public IAssignable Value { get; set; }

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            scope.SetVariable(Variable.Name, Value.Execute(scope, functions));

            return null;
        }

        public bool CanDoReturn()
        {
            return false;
        }
    }
}
