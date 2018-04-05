using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Common.Nodes;

namespace Xchange.Interfaces
{
    public interface INode
    {
        NodeTypeEnum Type { get; }
    }
}
