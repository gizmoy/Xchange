using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Semantic.Entities
{
    public class ScopeProto
    {
        public ScopeProto UpperScope { get; set; }
        public IDictionary<string, bool> Variables { get; set; } = new Dictionary<string, bool>();
        public IList<string> VarOrder { get; set; } = new List<string>();

        public bool AddVariable(string name)
        {
            if (HasVariable(name))
            {
                return false;
            }

            Variables[name] = false;
            VarOrder.Add(name);

            return true;
        }

        public bool? GetVariable(string name)
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

        public void SetVariableDefined(string name)
        {
            if (Variables.ContainsKey(name))
            {
                Variables[name] = true;
            }
        }

        public bool HasVariable(string name)
        {
            return GetVariable(name) != null;
        }

        public bool IsVariableDefined(string name)
        {
            var variable = GetVariable(name);

            if (variable == null)
            {
                return false;
            }

            return variable.Value;
        }

        public ScopeInstance Instantiate(ScopeInstance upperScope)
        {
            var instance = new ScopeInstance();

            instance.UpperScope = upperScope;
            instance.VarOrder = VarOrder;

            foreach (var key in Variables.Keys)
            {
                    instance.Variables[key] = new Literal();
            }

            return instance;
        }
    }
}
