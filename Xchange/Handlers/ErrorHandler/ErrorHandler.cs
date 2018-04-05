using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xchange.Interfaces;

namespace Xchange.Handlers.ErrorHandler
{
    public class ErrorHandler : TerminalPrinter, IErrorHandler
    {
        public void DebugFatal(string message)
        {
            PrintLabel("Fatal", ConsoleColor.Magenta);
            Print(GetLabelLength("Fatal"), message);

            throw new Exception(message);
        }

        public void Error(string message, bool noThrow = false)
        {
            PrintLabel("Error", ConsoleColor.Red);
            Print(GetLabelLength("Error"), message);

            if (noThrow)
            {
                return;
            }

            //throw new Exception(message);
        }

        public void Warning(string message)
        {
            PrintLabel("Warn", ConsoleColor.Yellow);
            Print(GetLabelLength("Warn"), message);
        }

        public void Notice(string message)
        {
            PrintLabel("Note", ConsoleColor.Cyan);
            Print(GetLabelLength("Note"), message);
        }

        public void Print(int labelShift, string message)
        {
            var lines = Regex.Split(message, "\r\n|\r|\n");
            var shift = new String(' ', labelShift);

            foreach (var line in lines)
            {
                var sb = new StringBuilder();

                sb.Append(shift);
                sb.Append(line);

                Console.WriteLine(sb.ToString());
            }
        }
    }
}
