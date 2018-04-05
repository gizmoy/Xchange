using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;

namespace Xchange.Handlers.ErrorHandler
{
    public static class ErrorHandlerStatic
    {
        static private IErrorHandler _ErrorHandler = new ErrorHandler();

        public static void DebugFatal(string message)
        {
            _ErrorHandler.DebugFatal(message);
        }

        public static void Error(string message, bool noThrow = false)
        {
            _ErrorHandler.Error(message);
        }

        public static void Warning(string message)
        {
            _ErrorHandler.Warning(message);
        }

        public static void Notice(string message)
        {
            _ErrorHandler.Notice(message);
        }

        public static void Error(int labelShift, string message)
        {
            _ErrorHandler.Error(message);
        }
    }
}
