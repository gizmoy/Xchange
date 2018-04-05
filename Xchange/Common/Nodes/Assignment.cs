using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class Assignment : Node
    {
        public Variable Variable { get; set; }
        public Assignable Value { get; set; }

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.Assignment;
            }
        }
    }
}
