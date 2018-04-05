using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Interfaces
{
    public interface ISource
    {
        long CurrentLine { get; }
        long CurrentColumn { get; }
        long CurrentPosition { get; }
        long CurrentLinePosition { get; }
        bool HasFinished { get; }

        char GetCharacter();
        void UngetCharacter();
        string GetLine(long linePosition);
    }
}
