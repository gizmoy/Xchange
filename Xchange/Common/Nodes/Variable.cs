using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Common.Nodes;

namespace Xchange.Common.Nodes
{
    public class Variable : Node
    {
        public string Name { get; set; }
        public decimal? Multiplier { get; set; }

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.Variable;
            }
        }
    }
}
