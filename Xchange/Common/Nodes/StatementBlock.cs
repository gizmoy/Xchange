using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class StatementBlock : Node
    {
        public IList<Node> Instructions { get; set; } = new List<Node>();

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.StatementBlock;
            }
        }
    }
}
