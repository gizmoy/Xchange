using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Interfaces;

namespace Xchange.Semantic.Entities
{
    public class Variable : IConditionOperand, IExpressionOperand
    {
        public string Name { get; set; }
        public decimal? Multiplier { get; set; }

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            var reference = scope.GetVariable(Name);
            var copy = new Literal();
            copy.Data = reference.Data;
            if (Multiplier.HasValue)
            {
                copy.Data *= Multiplier.Value;
            }

            return copy;
        }

        public bool IsTruthy()
        {
            return false;
        }
    }
}
