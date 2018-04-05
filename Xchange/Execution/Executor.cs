using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;
using Xchange.Semantic.Entities;

namespace Xchange.Execution
{
    public class Executor : IExecutor
    {
        public Executor()
        {
            // empty
        }

        public void Execute(IList<Function> functions)
        {
            var definedFunctions = new Dictionary<string, Function>();
            foreach (var function in functions)
            {
                definedFunctions[function.Name] = function;
            }

            var main = definedFunctions["main"];

            main.Execute(null, definedFunctions, new List<Literal>());
        }
    }
}
