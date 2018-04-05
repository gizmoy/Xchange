using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;

namespace Xchange.Common.Nodes
{
    public abstract class Node : INode
    {
        private Node _Parent;

        public virtual NodeTypeEnum Type { get; }
    }
}
