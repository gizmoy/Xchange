using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Common;
using Xchange.Handlers.ErrorHandler;
using Xchange.Semantic.Interfaces;

namespace Xchange.Semantic.Entities
{
    public class Expression : IAssignable, IExpressionOperand
    {
        public IList<TokenTypeEnum> Operations { get; set; } = new List<TokenTypeEnum>();
        public IList<IExpressionOperand> Operands { get; set; } = new List<IExpressionOperand>();

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            if(Operations.Count == 0)
            {
                return Operands[0].Execute(scope, functions);
            }

            var result = Operands[0].Execute(scope, functions);
            int i = 0;

            foreach (var op in Operations)
            {
                var it = Operands[i + 1];
                i += 1;

                if (op == TokenTypeEnum.Plus)
                {
                    result += it.Execute(scope, functions);
                }
                else if (op == TokenTypeEnum.Minus)
                {
                    result -= it.Execute(scope, functions);
                }
                else if (op == TokenTypeEnum.Multiply)
                {
                    result *= it.Execute(scope, functions);
                }
                else if (op == TokenTypeEnum.Divide)
                {
                    result /= it.Execute(scope, functions);
                }
                else if (op == TokenTypeEnum.Modulo)
                {
                    result %= it.Execute(scope, functions);
                }
                else
                {
                    ErrorHandlerStatic.Error("Invalid expression operator");

                    return null;
                }
            }

            return result;
        }
    }
}
