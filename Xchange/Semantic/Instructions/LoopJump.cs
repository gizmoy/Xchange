using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Entities;
using Xchange.Semantic.Interfaces;

namespace Xchange.Semantic.Instructions
{
    public class LoopJump : IInstruction
    {
        public bool IsBreak { get; set; }

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            var result = new Literal();
            result.LoopJump = true;
            result.IsBreak = IsBreak;

            return result;
        }

        public bool CanDoReturn()
        {
            return false;
        }
    }
}
