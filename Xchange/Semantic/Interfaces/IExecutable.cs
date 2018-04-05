using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Entities;

namespace Xchange.Semantic.Interfaces
{
    public interface IExecutable
    {
        Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions);
    }
}
