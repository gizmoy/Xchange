using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class Program : Node
    {
        public IList<FunDefinition> Functions { get; set; } = new List<FunDefinition>();

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.Program;
            }
        }
    }
}
