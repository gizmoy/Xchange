using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Interfaces;

namespace Xchange.Semantic.Entities
{
    public class Block : IInstruction
    {
        public ScopeProto ScopeProto = new ScopeProto();
        public IList<IInstruction> Instructions { get; set; } = new List<IInstruction>();

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {

            var thisScope = ScopeProto.Instantiate(scope);

            foreach (var instruction in Instructions)
            {
                var result = instruction.Execute(thisScope, functions);
                if (result != null && (result.LoopJump || instruction.CanDoReturn()))
                {
                    return result;
                }
            }

            return null;
        }

        public bool CanDoReturn()
        {
            return true;
        }
    }
}
