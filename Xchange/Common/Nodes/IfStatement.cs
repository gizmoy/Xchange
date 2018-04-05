using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class IfStatement : Node
    {
        public Condition Condition { get; set; }
        public StatementBlock TrueBlock { get; set; }
        public StatementBlock FalseBlock { get; set; }

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.IfStatement;
            }
        }
    }
}
