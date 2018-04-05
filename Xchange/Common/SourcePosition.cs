using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common
{
    public struct SourcePosition
    {
        public long Line { get; set; }
        public long Position { get; set; }
        public long LinePosition { get; set; }
    }
}
