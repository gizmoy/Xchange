using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class ReturnStatement : Node
    {
        public Assignable Value { get; set; }

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.ReturnStatement;
            }
        }
    }
}
