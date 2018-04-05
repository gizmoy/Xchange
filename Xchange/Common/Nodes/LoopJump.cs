using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class LoopJump : Node
    {
        public bool IsBreak { get; set; }

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.LoopJump;
            }
        }

        public void SetTypeByTokenType(TokenTypeEnum tokenType)
        {
            IsBreak = (tokenType == TokenTypeEnum.Break);
        }
    }
}
