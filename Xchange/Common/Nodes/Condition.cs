using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class Condition : Node
    {
        public bool Negated { get; set; } = false;
        public TokenTypeEnum Operation { get; set; } = TokenTypeEnum.Undefined;
        public IList<Node> Operands { get; set; } = new List<Node>();

        public Node LeftSide
        {
            get
            {
                return Operands[0];
            }
        }

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.Condition;
            }
        }
    }
}
