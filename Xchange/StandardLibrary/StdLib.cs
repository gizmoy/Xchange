using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;
using Xchange.Semantic.Entities;

namespace Xchange.StandardLibrary
{
    public static class StdLib
    {
        public static IDictionary<string, int> StandardFunctions { get; set; } = new Dictionary<string, int>()
        {
            { "print", 1 },
            { "printLine", 1 }
        };

        public static int GetFunctionParamsCount(string name)
        {
            if (StandardFunctions.ContainsKey(name))
            {
                return StandardFunctions[name];
            }

            return -1;
        }

        public static bool HasFunction(string name)
        {
            return StandardFunctions.ContainsKey(name);
        }

        public static Literal CallFunction(string functionName, IList<Literal> arguments)
        {
            switch(functionName)
            {
                case "print":
                    return Print(arguments);

                case "printLine":
                    return PrintLine(arguments);

                default:
                    return null;
            }
        }

        public static Literal Print(IList<Literal> arguments)
        {
            if (arguments.Count > 0)
            {
                var text = arguments[0].Data.ToString();
                Console.Write(text);
            }

            return null;
        }

        public static Literal PrintLine(IList<Literal> arguments)
        {
            if (arguments.Count > 0)
            {
                var text = arguments[0].Data.ToString();
                Console.WriteLine(text);
            }

            return null;
        }
    }
}
