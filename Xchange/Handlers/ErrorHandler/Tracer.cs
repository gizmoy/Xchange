using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;

namespace Xchange.Handlers.ErrorHandler
{
    public class Tracer : TerminalPrinter, ITracer
    {
        private IErrorHandler _ErrorHandler;

        private int _NestingLevel = 0;
        public bool Enabled { get; set;  } = false;

        public Tracer(IErrorHandler error)
        {
            _ErrorHandler = error;
        }

        public void Enter(string message = "")
        {
            if (Enabled == false)
            {
                return;
            }

            if (message.Length > 0)
            {
                PrintLabel("Tracer", ConsoleColor.Blue);
                PrintNesting();
                Console.WriteLine(message);
            }

            _NestingLevel += 1;
        }

        public void Info(string message)
        {
            if (Enabled == false)
            {
                return;
            }

            PrintLabel("Tracer", ConsoleColor.Blue);
            PrintNesting();
            Console.WriteLine(message);
        }

        public void Leave(string message = "leave")
        {
            if (Enabled == false)
            {
                return;
            }

            if (_NestingLevel == 0)
            {
                PrintLabel("Tracer", ConsoleColor.Blue);
                _ErrorHandler.Warning("Tried to leave top tracking level, leave aborted...");
                return;
            }

            _NestingLevel -= 1;

            if (message.Length > 0)
            {
                PrintLabel("Tracer", ConsoleColor.Blue);
                PrintNesting();
                Console.WriteLine(message);
            }
        }

        public void PrintNesting()
        {
            var sb = new StringBuilder();

            for (int i = _NestingLevel - 1; i > 0; --i)
            {
                sb.Append("| ");
            }

            sb.Append("|-");

            Console.Write(sb.ToString());
        }

        public void Reset()
        {
            _NestingLevel = 0;
        }
    }
}
