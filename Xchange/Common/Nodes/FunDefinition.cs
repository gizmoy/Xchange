using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class FunDefinition : Node
    {
        public string Name { get; set; }
        public StatementBlock Block { get; set; }
        public IList<string> Parameters { get; set; } = new List<string>();

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.FunDefinition;
            }
        }
    }
}
