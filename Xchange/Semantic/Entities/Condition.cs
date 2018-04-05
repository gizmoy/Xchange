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
    public class Condition : IConditionOperand
    {
        public bool Negated { get; set; } = false;
        public TokenTypeEnum Operation { get; set; } = TokenTypeEnum.Undefined;
        public IList<IConditionOperand> Operands = new List<IConditionOperand>();

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            if (Operation == TokenTypeEnum.Undefined)
            {
                if (!Negated)
                {
                    return Operands[0].Execute(scope, functions);
                }
                else
                {
                    var result = new Literal();
                    result.CastedToBool = true;
                    result.Data =  Operands[0].Execute(scope, functions).IsTruthy() ? 0.0m : 1.0m;
                    return result;
                }
            }

            else if (Operation == TokenTypeEnum.Or)
            {
                var result = new Literal();
                result.CastedToBool = true;

                foreach (var it in Operands)
                {
                    if (it.Execute(scope, functions).IsTruthy())
                    {
                        result.Data = 1.0m;
                        return result;
                    }
                }

                result.Data = 0.0m;

                return result;
            }

            else if (Operation == TokenTypeEnum.And)
            {
                var result = new Literal();
                result.CastedToBool = true;

                foreach (var it in Operands)
                {
                    if (!it.Execute(scope, functions).IsTruthy())
                    {
                        result.Data = 0.0m;
                        return result;
                    }
                }

                result.Data = 1.0m;

                return result;
            }

            else if (Operation == TokenTypeEnum.Equality)
            {
                var result = new Literal();
                result.CastedToBool = true;

                var left = Operands[0].Execute(scope, functions);
                var right = Operands[1].Execute(scope, functions);

                if (left.CastedToBool && right.CastedToBool)
                {
                    result.Data = left.IsTruthy() == right.IsTruthy() ? 1.0m : 0.0m;
                }
                else if (!left.CastedToBool && !right.CastedToBool)
                {
                    result.Data = left == right ? 1.0m : 0.0m;
                }
                else
                {
                    ErrorHandlerStatic.Error(("Tried to compare currency with bool currency"));

                    return null;
                }

                return result;
            }

            else if (Operation == TokenTypeEnum.Inequality)
            {
                var result = new Literal();
                result.CastedToBool = true;

                var left = Operands[0].Execute(scope, functions);
                var right = Operands[1].Execute(scope, functions);

                if (left.CastedToBool && right.CastedToBool)
                {
                    result.Data = left.IsTruthy() != right.IsTruthy() ? 1.0m : 0.0m;
                }
                else if (!left.CastedToBool && !right.CastedToBool)
                {
                    result.Data = left != right ? 1.0m : 0.0m;
                }
                else
                {
                    ErrorHandlerStatic.Error(("Tried to compare currency with bool currency"));

                    return null;
                }

                return result;
            }

            else if (Operation == TokenTypeEnum.Less)
            {
                var result = new Literal();
                result.CastedToBool = true;

                var left = Operands[0].Execute(scope, functions);
                var right = Operands[1].Execute(scope, functions);

                if (!left.CastedToBool && !right.CastedToBool)
                {
                    result.Data = left < right ? 1.0m : 0.0m;
                }
                else
                {
                    ErrorHandlerStatic.Error("Cannot compare bool currencies");

                    return null;
                }

                return result;
            }
            
            else if (Operation == TokenTypeEnum.LessOrEqual)
            {
                var result = new Literal();
                result.CastedToBool = true;

                var left = Operands[0].Execute(scope, functions);
                var right = Operands[1].Execute(scope, functions);

                if (!left.CastedToBool && !right.CastedToBool)
                {
                    result.Data = left <= right ? 1.0m : 0.0m;
                }
                else
                {
                    ErrorHandlerStatic.Error("Cannot compare bool currencies");

                    return null;
                }

                return result;
            }
            
            else if (Operation == TokenTypeEnum.Greater)
            {
                var result = new Literal();
                result.CastedToBool = true;

                var left = Operands[0].Execute(scope, functions);
                var right = Operands[1].Execute(scope, functions);

                if (!left.CastedToBool && !right.CastedToBool)
                {
                    result.Data = left > right ? 1.0m : 0.0m;
                }
                else
                {
                    ErrorHandlerStatic.Error("Cannot compare bool currencies");

                    return null;
                }

                return result;
            }
            
           else if (Operation == TokenTypeEnum.GreaterOrEqual)
            {
                var result = new Literal();
                result.CastedToBool = true;

                var left = Operands[0].Execute(scope, functions);
                var right = Operands[1].Execute(scope, functions);

                if (!left.CastedToBool && !right.CastedToBool)
                {
                    result.Data = left >= right ? 1.0m : 0.0m;
                }
                else
                {
                    ErrorHandlerStatic.Error("Cannot compare bool currencies");

                    return null;
                }

                return result;
            }

            else
            {
                ErrorHandlerStatic.Error("Invalid condition operator");

                return null;
            }
        }

        public bool IsTruthy()
        {
            return false;
        }
    }
}
