using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Utils;
using Xchange.Lexical;
using Xchange.Stream;
using Xchange.Syntax;
using Xchange.Handlers.ErrorHandler;
using Xchange.Handlers.CurrencyHandler;
using SimpleInjector;
using Xchange.Interfaces;
using Xchange.Common;
using Xchange.Semantic;
using Xchange.Execution;

namespace Xchange
{
    class Program
    {
        static readonly Container container;

        static Program()
        {
            // Create a new Simple Injector container
            container = new Container();

            // Configure the container (register)
            container.Register<ICurrencyHandler, CurrencyHandler>(Lifestyle.Singleton);
            container.Register<IErrorHandler, ErrorHandler>(Lifestyle.Singleton);
            container.Register<IReader, Reader>(Lifestyle.Singleton);
            container.Register<ISource, Source>(Lifestyle.Singleton);
            container.Register<ILexer, Lexer>(Lifestyle.Singleton);
            container.Register<ITracer, Tracer>(Lifestyle.Singleton);
            container.Register<IParser, Parser>(Lifestyle.Singleton);
            container.Register<ISemanticCheck, SemanticCheck>(Lifestyle.Singleton);
            container.Register<IExecutor, Executor>(Lifestyle.Singleton);
            container.Register<IInterpreter, Interpreter>(Lifestyle.Singleton);

            // Verify configuration
            container.Verify();
        }

        static void Main(string[] args)
        {
            // Initialize interpreter and perform interpretation
            var interpreter = container.GetInstance<IInterpreter>();
            interpreter.Interpret(args);
        }
    }
}
