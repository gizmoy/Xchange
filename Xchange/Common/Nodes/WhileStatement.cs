using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class WhileStatement : Node
    {
        public Condition Condition { get; set; }
        public StatementBlock Block { get; set; }

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.WhileStatement;
            }
        }
    }
}
