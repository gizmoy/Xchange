using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Entities;
using Xchange.Semantic.Interfaces;

namespace Xchange.Semantic.Instructions
{
    public class IfStatement : IInstruction
    {
        public Condition Condition { get; set; }
        public Block TrueBlock { get; set; }
        public Block FalseBlock { get; set; }

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            if (Condition.Execute(scope, functions).IsTruthy())
            {
                return TrueBlock.Execute(scope, functions);
            }
            else if (FalseBlock != null)
            {
                return FalseBlock.Execute(scope, functions);
            }

            return null;
        }

        public bool CanDoReturn()
        {
            return true;
        }
    }
}
