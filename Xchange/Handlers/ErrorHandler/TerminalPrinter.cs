using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;

namespace Xchange.Handlers.ErrorHandler
{
    public class TerminalPrinter : ITerminalPrinter
    {
        private bool _IsColorEnabled;

        public TerminalPrinter(bool isColorEnabled = true)
        {
            _IsColorEnabled = isColorEnabled;
        }


        public int GetLabelLength(string message)
        {
            return message.Length + 3;
        }

        public void PrintLabel(string message, ConsoleColor color = ConsoleColor.White)
        {
            var sb = new StringBuilder();

            sb.Append("[");
            sb.Append(message);
            sb.Append("]");

            // Colorize if enabled
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = _IsColorEnabled
                ? color
                : ConsoleColor.White;

            // Print
            Console.Write(sb.ToString());

            // Restore white console color
            Console.ForegroundColor = oldColor;
        }
    }
}
