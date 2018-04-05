using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Entities;
using Xchange.Semantic.Interfaces;

namespace Xchange.Semantic.Instructions
{
    public class WhileStatement : IInstruction
    {
        public Condition Condition { get; set; }
        public Block Block { get; set; }

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            while (Condition.Execute(scope, functions).IsTruthy())
            {
                var result = Block.Execute(scope, functions);

                if (result != null && result.LoopJump)
                {
                    if (result.IsBreak)
                    {
                        break;
                    }

                    continue;
                }

                if (result != null && Block.CanDoReturn())
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
