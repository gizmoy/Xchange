using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Entities;
using Xchange.Semantic.Instructions;
using Xchange.Semantic.Interfaces;

using LexNodes = Xchange.Common.Nodes;

namespace Xchange.Interfaces
{
    public interface ISemanticCheck
    {
        IList<Function> Check(Xchange.Common.Nodes.Program syntaxTree);

        void ScanFunctionDefinitions();
        void CheckMain();
        IList<Function> TraverseTree();
        Function CheckFunction(LexNodes.FunDefinition functionDef);
        Block CheckBlock(ScopeProto scopeProto, LexNodes.StatementBlock blockNode);

        void CheckVarDeclaration(ScopeProto scopeProto, string name);
        Assignment CheckAssignment(ScopeProto scopeProto, string variable, LexNodes.Assignable assignable);
        Assignment CheckAssignment(ScopeProto scopeProto, LexNodes.Variable variable, LexNodes.Assignable assignable);
        IAssignable CheckAssignable(ScopeProto scopeProto, LexNodes.Assignable assignable);

        Call CheckFunctionCall(ScopeProto scopeProto, LexNodes.Call call);
        Expression CheckExpression(ScopeProto scopeProto, LexNodes.Expression expression);
        Variable CheckVariable(ScopeProto scopeProto, LexNodes.Variable variable);
        Return CheckReturnStatement(ScopeProto scopeProto, LexNodes.Assignable assignable);
        IfStatement CheckIfStatement(ScopeProto scopeProto, LexNodes.IfStatement statement);
        WhileStatement CheckWhileStatement(ScopeProto scopeProto, LexNodes.WhileStatement statement);
        Condition CheckCondition(ScopeProto scopeProto, LexNodes.Condition condition);
        Literal CheckCurrencyLiteral(LexNodes.Currency currencyLiteral);
    }
}
