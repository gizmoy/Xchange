using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Interfaces
{
    public interface ITerminalPrinter
    {
        void PrintLabel(string message, ConsoleColor color);
        int GetLabelLength(string message);
    }
}
