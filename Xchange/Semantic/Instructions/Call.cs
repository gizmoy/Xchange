using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Entities;
using Xchange.Semantic.Interfaces;
using Xchange.StandardLibrary;

namespace Xchange.Semantic.Instructions
{
    public class Call : IInstruction, IAssignable, IExpressionOperand
    {
        public string Name { get; set; }
        public decimal? Multiplier { get; set; }
        public IList<IAssignable> Arguments { get; set; } = new List<IAssignable>();

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            var concreteArguments = new List<Literal>();

            foreach (var argument in Arguments)
            {
                concreteArguments.Add(argument.Execute(scope, functions));
            }

            if (functions.ContainsKey(Name))
            {
                var result = functions[Name].Execute(null, functions, concreteArguments);

                if (Multiplier.HasValue)
                {
                    result.Data *= Multiplier.Value;
                }

                return result;
            }
            else
            {
                return StdLib.CallFunction(Name, concreteArguments);
            }
        }

        public bool CanDoReturn()
        {
            return false;
        }
    }
}
