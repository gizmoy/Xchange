using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Entities;
using Xchange.Semantic.Interfaces;

namespace Xchange.Semantic.Instructions
{
    public class Return : IInstruction
    {
        public IAssignable Value { get; set; }

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            return Value.Execute(scope, functions);
        }

        public bool CanDoReturn()
        {
            return true;
        }
    }
}
