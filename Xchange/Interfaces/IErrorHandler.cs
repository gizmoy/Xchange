using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Interfaces
{
    public interface IErrorHandler
    {
        void DebugFatal(string message);
        void Error(string message, bool noThrow = false);
        void Warning(string message);
        void Notice(string message);

        void Print(int labelShift, string message);
    }
}
