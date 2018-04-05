using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Handlers.ErrorHandler;

namespace Xchange.Semantic.Entities
{
    public class ScopeInstance
    {
        public ScopeInstance UpperScope { get; set; }
        public IDictionary<string, Literal> Variables { get; set; } = new Dictionary<string, Literal>();
        public IList<string> VarOrder { get; set; } = new List<string>();

        public Literal GetVariable(string name)
        {
            if (Variables.ContainsKey(name))
            {
                return Variables[name];
            }

            if (UpperScope != null)
            {
                return UpperScope.GetVariable(name);
            }

            return null;
        }

        public void SetVariable(string name, Literal literal)
        {
            if (Variables.ContainsKey(name))
            {
                Variables[name] = literal;
                return;
            }

            if (UpperScope != null)
            {
                UpperScope.SetVariable(name, literal);
                return;
            }

            ErrorHandlerStatic.Error("Setting undefined variable");
            
            return;
        }
    }
}
