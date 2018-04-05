using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common
{
    public struct Token
    {
        public SourcePosition SourcePosition { get; set; }
        public TokenTypeEnum Type { get; set; }
        public string Value { get; set; }

        public Token(TokenTypeEnum type, SourcePosition position, string value = null)
        {
            Type = type;
            SourcePosition = position;
            Value = value;
        }
    }
}
