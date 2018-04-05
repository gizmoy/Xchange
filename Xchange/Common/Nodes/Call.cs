using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public class Call : Assignable
    {
        public string Name { get; set; }
        public decimal? Multiplier { get; set; }
        public IList<Assignable> Arguments { get; set; } = new List<Assignable>();

        public override NodeTypeEnum Type
        {
            get
            {
                return NodeTypeEnum.Call;
            }
        }
    }
}
