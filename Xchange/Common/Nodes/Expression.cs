using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class Expression : Assignable
    {
        public IList<TokenTypeEnum> Operations { get; set; } = new List<TokenTypeEnum>();
        public IList<Node> Operands { get; set; } = new List<Node>();

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.Expression;
            }
        }
    }
}
