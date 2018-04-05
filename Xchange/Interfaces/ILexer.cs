using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Common;

namespace Xchange.Interfaces
{
    public interface ILexer
    {
        Token GetNextToken();
        string GetLine(long linePosition);

        Token? TryParseString(SourcePosition position);
        Token? TryParseDigit(SourcePosition position);
        Token? TryParseOther(SourcePosition position);
        
        char GetNextChar();
    }
}
