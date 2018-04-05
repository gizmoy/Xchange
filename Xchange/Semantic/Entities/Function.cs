using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Handlers.ErrorHandler;

namespace Xchange.Semantic.Entities
{
    public class Function : Block
    {
        public string Name { get; set; }

        public new Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            ErrorHandlerStatic.Error("Cannot execute function without parameters, fatal error");

            return null;
        }

        public virtual Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions, IList<Literal> arguments)
        {
            var thisScope = ScopeProto.Instantiate(scope);
            int argIdx = 0;

            foreach (var argument in arguments)
            {
                var copy = new Literal();
                copy.Data = argument.Data;

                thisScope.Variables[thisScope.VarOrder[argIdx]] = copy;
                argIdx +=1;
            }

            foreach (var instruction in Instructions)
            {
                var result = instruction.Execute(thisScope, functions);

                if (result != null && result.LoopJump)
                {
                    ErrorHandlerStatic.Error("Break/continue outside of loop");
                }

                if (result != null && instruction.CanDoReturn())
                {
                    return result;
                }
            }

            ErrorHandlerStatic.Error("No return, fatal error");

            return null;
        }

    }
}
