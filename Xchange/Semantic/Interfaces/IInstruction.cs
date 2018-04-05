using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Semantic.Interfaces
{
    public interface IInstruction : IExecutable
    {
        bool CanDoReturn();
    }
}
