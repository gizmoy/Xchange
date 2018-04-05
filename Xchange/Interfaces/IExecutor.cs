using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Entities;

namespace Xchange.Interfaces
{
    public interface IExecutor
    {
        void Execute(IList<Function> functions);
    }
}
