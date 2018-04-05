using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Common;
using Xchange.Interfaces;
using Xchange.Semantic.Entities;
using Xchange.Semantic.Instructions;
using Xchange.Semantic.Interfaces;
using Xchange.StandardLibrary;
using LexNodes = Xchange.Common.Nodes;

namespace Xchange.Semantic
{
    public class SemanticCheck : ISemanticCheck
    {
        private IErrorHandler _ErrorHandler;

        private Xchange.Common.Nodes.Program _SyntaxTree;
        private IDictionary<string, Function> _DefinedFunctions = new Dictionary<string, Function>();

        public SemanticCheck(IErrorHandler error)
        {
            _ErrorHandler = error;
        }

        public IList<Function> Check(LexNodes.Program syntaxTree)
        {
            _SyntaxTree = syntaxTree;
            _DefinedFunctions.Clear();

            ScanFunctionDefinitions();
            CheckMain();
            
            return TraverseTree();
        }

        public void ScanFunctionDefinitions()
        {
            foreach (var function in _SyntaxTree.Functions)
            {
                if (StdLib.HasFunction(function.Name))
                {
                    _ErrorHandler.Error($"Redefinition of Standard Lib function: {function.Name}");
                    return;
                }

                if (_DefinedFunctions.ContainsKey(function.Name))
                {
                    _ErrorHandler.Error($"Duplicated definition of function: {function.Name}");
                    return;
                }

                var fun = new Function() { Name = function.Name };
                _DefinedFunctions[function.Name] = fun;

                foreach (var parameter in function.Parameters)
                {
                    if (fun.ScopeProto.AddVariable(parameter) == false)
                    {
                        _ErrorHandler.Error($"Duplicated definition of parameter \"{parameter}\" of function \"{function.Name}\"");
                        return;
                    }

                    fun.ScopeProto.SetVariableDefined(parameter);
                }
            }
        }

        public void CheckMain()
        {
            if (_DefinedFunctions.ContainsKey("main") == false)
            {
                _ErrorHandler.Error("No entry point (a.k.a. \"main\" function) defined");
                return;
            }
            if (_DefinedFunctions["main"].ScopeProto.Variables.Count != 0)
            {
                _ErrorHandler.Error("\"main\" function should not have parameters");
                return;
            }
        }

        public IList<Function> TraverseTree()
        {
            var functions = new List<Function>();

            foreach (var function in _SyntaxTree.Functions)
            {
                functions.Add(CheckFunction(function));
            }

            return functions;
        }

        public Function CheckFunction(LexNodes.FunDefinition functionDef)
        {
            var function = _DefinedFunctions[functionDef.Name];

            function.Instructions.Add(CheckBlock(function.ScopeProto, functionDef.Block));

            return function;
        }

        public Block CheckBlock(ScopeProto scopeProto, LexNodes.StatementBlock blockNode)
        {
            var block = new Block();
            block.ScopeProto.UpperScope= scopeProto;

            foreach (var instruction in blockNode.Instructions)
            {
                switch (instruction.Type)
                {
                    case LexNodes.NodeTypeEnum.VarDeclaration:
                        
                        var node = (LexNodes.VarDeclaration)instruction;
                        CheckVarDeclaration(block.ScopeProto, node.Name);

                        if (node.Value != null)
                        {
                            block.Instructions.Add(CheckAssignment(block.ScopeProto, node.Name, node.Value));
                        }

                        break;
                        
                    case LexNodes.NodeTypeEnum.Assignment:
                        
                        var assignment = (LexNodes.Assignment)instruction;
                        block.Instructions.Add(CheckAssignment(block.ScopeProto, assignment.Variable, assignment.Value));

                        break;
                        
                    case LexNodes.NodeTypeEnum.ReturnStatement:
                        
                        var ret = (LexNodes.ReturnStatement)instruction;
                        block.Instructions.Add(CheckReturnStatement(block.ScopeProto, ret.Value));

                        break;
                        
                    case LexNodes.NodeTypeEnum.Call:
                        
                        var call = (LexNodes.Call)instruction;
                        block.Instructions.Add(CheckFunctionCall(block.ScopeProto, call));
                        break;
                        
                    case LexNodes.NodeTypeEnum.StatementBlock:
                        
                        var statementBlock = (LexNodes.StatementBlock)instruction;
                        block.Instructions.Add(CheckBlock(block.ScopeProto, statementBlock));

                        break;
                        
                    case LexNodes.NodeTypeEnum.IfStatement:
                        
                        var ifStatement = (LexNodes.IfStatement)instruction;
                        block.Instructions.Add(CheckIfStatement(block.ScopeProto, ifStatement));
                        break;
                        
                    case LexNodes.NodeTypeEnum.WhileStatement:

                        var whileStatement = (LexNodes.WhileStatement)instruction;
                        block.Instructions.Add(CheckWhileStatement(block.ScopeProto, whileStatement));

                        break;
                        
                    case LexNodes.NodeTypeEnum.LoopJump:
                        
                        var jump = new LoopJump();
                        jump.IsBreak = ((LexNodes.LoopJump)instruction).IsBreak;
                        block.Instructions.Add(jump);

                        break;
                       
                    default:

                        _ErrorHandler.Error("Invalid instruction node type");

                        break;   
                }
            }

            return block;
        }

        public void CheckVarDeclaration(ScopeProto scopeProto, string name)
        {
            if (!scopeProto.AddVariable(name))
            {
                _ErrorHandler.Error($"Redeclaration of variable: {name}");
            }
        }

        public Assignment CheckAssignment(ScopeProto scopeProto, string variable, LexNodes.Assignable assignable)
        {
            var node = new Assignment();

            if (!scopeProto.HasVariable(variable))
            {
                _ErrorHandler.Error($"Assignment to undefined variable: {variable}");
                return null;
            }

            node.Variable.Name = variable;
            node.Value = CheckAssignable(scopeProto, assignable);

            scopeProto.SetVariableDefined(variable);

            return node;
        }

        public Assignment CheckAssignment(ScopeProto scopeProto, LexNodes.Variable variable, LexNodes.Assignable assignable)
        {
            var node = new Assignment();

            if (!scopeProto.HasVariable(variable.Name))
            {
                _ErrorHandler.Error($"Assignment to undefined variable: {variable.Name}");
                return null;
            }

            if (!scopeProto.IsVariableDefined(variable.Name))
            {
                _ErrorHandler.Error($"Indexed assignment to empty variable: {variable.Name}");
                return null;
            }

            node.Variable.Name = variable.Name;
            node.Value = CheckAssignable(scopeProto, assignable);

            scopeProto.SetVariableDefined(variable.Name);

            return node;
        }

        public IAssignable CheckAssignable(ScopeProto scopeProto, LexNodes.Assignable assignable)
        {
            if (assignable.Type == LexNodes.NodeTypeEnum.Call)
            {
                return CheckFunctionCall(scopeProto, (LexNodes.Call)assignable);
            }
            else if (assignable.Type == LexNodes.NodeTypeEnum.Expression)
            {
                return CheckExpression(scopeProto, (LexNodes.Expression)assignable);
            }

            _ErrorHandler.Error("Invalid assignable value");

            return null;
        }

        public Call CheckFunctionCall(ScopeProto scopeProto, LexNodes.Call call)
        {
            if (_DefinedFunctions.ContainsKey(call.Name) == false && StdLib.HasFunction(call.Name) == false)
            {
                _ErrorHandler.Error($"Call to undefined function: {call.Name}");
                return null;
            }

            if (_DefinedFunctions.ContainsKey(call.Name))
            {
                var functionDef = _DefinedFunctions[call.Name];
                if (functionDef.ScopeProto.Variables.Count != call.Arguments.Count)
                {
                    _ErrorHandler.Error($"Invalid arguments count for function \"{call.Name}\", expected {functionDef.ScopeProto.Variables.Count}, got {call.Arguments.Count}");
                    return null;
                }
            }
            else
            {
                int requiredArgsNum = StdLib.GetFunctionParamsCount(call.Name);
                if (requiredArgsNum != call.Arguments.Count)
                {
                    _ErrorHandler.Error($"Invalid arguments count for function \"{call.Name}\", expected {requiredArgsNum}, got {call.Arguments.Count}");
                    return null;
                }
            }

            var obj = new Call();
            obj.Name = call.Name;
            obj.Multiplier = call.Multiplier;

            foreach (var argument in call.Arguments)
            {
                obj.Arguments.Add(CheckAssignable(scopeProto, argument));
            }

            return obj;
        }

        public Expression CheckExpression(ScopeProto scopeProto, LexNodes.Expression expression)
        {
            var obj = new Expression();

            obj.Operations = expression.Operations;

            foreach (var operand in expression.Operands)
            {
                if (operand.Type == LexNodes.NodeTypeEnum.Currency)
                {
                    obj.Operands.Add(CheckCurrencyLiteral((LexNodes.Currency)operand));
                }
                else if (operand.Type == LexNodes.NodeTypeEnum.Expression)
                {
                    obj.Operands.Add(CheckExpression(scopeProto, (LexNodes.Expression)operand));
                }
                else if (operand.Type == LexNodes.NodeTypeEnum.Variable)
                {
                    obj.Operands.Add(CheckVariable(scopeProto, (LexNodes.Variable)operand));
                }
                else if (operand.Type == LexNodes.NodeTypeEnum.Call)
                {
                    obj.Operands.Add(CheckFunctionCall(scopeProto, (LexNodes.Call)operand));
                }
                else
                {
                    _ErrorHandler.Error("Invalid expression operand");
                }
            }

            return obj;
        }

        public Variable CheckVariable(ScopeProto scopeProto, LexNodes.Variable variable)
        {
            var obj = new Variable();

            if (!scopeProto.HasVariable(variable.Name))
            {
                _ErrorHandler.Error($"Usage of undefined variable: {variable.Name}");
                return null;
            }

            if (!scopeProto.IsVariableDefined(variable.Name))
            {
                _ErrorHandler.Error($"Usage of empty variable: {variable.Name}");
                return null;
            }

            obj.Name = variable.Name;
            obj.Multiplier = variable.Multiplier;

            return obj;
        }

        public Return CheckReturnStatement(ScopeProto scopeProto, LexNodes.Assignable assignable)
        {
            var obj = new Return();

            obj.Value = CheckAssignable(scopeProto, assignable);

            return obj;
        }

        public IfStatement CheckIfStatement(ScopeProto scopeProto, LexNodes.IfStatement statement)
        {
            var obj = new IfStatement();

            obj.Condition = CheckCondition(scopeProto, statement.Condition);
            obj.TrueBlock = CheckBlock(scopeProto, statement.TrueBlock);
            if (statement.FalseBlock != null)
            {
                obj.FalseBlock = CheckBlock(scopeProto, statement.FalseBlock);
            }

            return obj;
        }

        public WhileStatement CheckWhileStatement(ScopeProto scopeProto, LexNodes.WhileStatement statement)
        {
            var obj = new WhileStatement();

            obj.Condition = CheckCondition(scopeProto, statement.Condition);

            obj.Block = CheckBlock(scopeProto, statement.Block);

            return obj;
        }

        public Condition CheckCondition(ScopeProto scopeProto, LexNodes.Condition condition)
        {
            var obj = new Condition();

            obj.Operation = condition.Operation;
            obj.Negated = condition.Negated;

            foreach (var operand in condition.Operands)
            {
                if (operand.Type == LexNodes.NodeTypeEnum.Currency)
                {
                    obj.Operands.Add(CheckCurrencyLiteral((LexNodes.Currency)operand));
                }
                else if (operand.Type == LexNodes.NodeTypeEnum.Condition)
                {
                    obj.Operands.Add(CheckCondition(scopeProto, (LexNodes.Condition)operand));
                }
                else if (operand.Type == LexNodes.NodeTypeEnum.Variable)
                {
                    obj.Operands.Add(CheckVariable(scopeProto, (LexNodes.Variable)operand));
                }
                else
                {
                    _ErrorHandler.Error("Invalid condition operand");
                }
            }

            return obj;
        }

        public Literal CheckCurrencyLiteral(LexNodes.Currency currencyLiteral)
        {
            var obj = new Literal();

            obj.Data = currencyLiteral.Value;

            return obj;
        }
    }
}