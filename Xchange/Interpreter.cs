using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;
using Xchange.Utils;

namespace Xchange
{
    public class Interpreter : IInterpreter
    {
        private IParser _Parser;
        private ISemanticCheck _SemanticCheck;
        private IExecutor _Executor;
        private IReader _Reader;
        private IErrorHandler _ErrorHandler;

        private CommandLineOptions _Options = new CommandLineOptions();

        public Interpreter(IParser parser, ISemanticCheck check, IExecutor executor, IReader reader, IErrorHandler error)
        {
            _Parser = parser;
            _SemanticCheck = check;
            _Executor = executor;
            _Reader = reader;
            _ErrorHandler = error;
        }

        public void Interpret(string[] args)
        {
            // Parse command line parameters
            if (CommandLine.Parser.Default.ParseArguments(args, _Options))
            {
                // Show configuration if verbosity is enabled
                if (_Options.Verbose)
                {
                    Console.WriteLine("Filename: {0}", _Options.InputFile);
                    _Parser._Tracer.Enabled = true;
                }

                // Init file stream with given input file's path
                _Reader.Init(_Options.InputFile);
            }
            else
            {
                _ErrorHandler.Error("Command line parameters are wrong - run with '-m' to see manual.");
            }

            try
            {
                var tree = _Parser.Parse();
                var functions = _SemanticCheck.Check(tree);
                _Executor.Execute(functions);
            }
            catch (Exception)
            {
                _ErrorHandler.Error("Terminating...", true);
            }

            Console.ReadKey();
        }
    }
}
