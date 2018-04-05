using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Common;
using Xchange.Common.Nodes;

namespace Xchange.Interfaces
{
    public interface IParser
    {
        ITracer _Tracer { get; }
        Xchange.Common.Nodes.Program Parse();

        bool IsAcceptable(Token token, params TokenTypeEnum[] acceptable);
        Token Accept(params TokenTypeEnum[] acceptable);
        bool Peek(params TokenTypeEnum[] acceptable);
        Token GetPeeked();
        void PeekFail();
        string MakeErrorMarker(long pos);

        bool HasBufferedToken { get; }
        void ResetPreviousToken();

        // Decomposition procedures
        FunDefinition ParseFunction();
        IList<string> ParseParameters();
        StatementBlock ParseStatementBlock();

        IfStatement ParseIfStatement();
        WhileStatement ParseWhileStatement();
        ReturnStatement ParseReturnStatement();
        VarDeclaration ParseInitStatement();
        Node ParseAssignmentOrFunCall();
        LoopJump ParseLoopJump();

        Assignable ParseAssignable();
        Call ParseFunCall(string identifier);
        Variable ParseVariable(Token firstToken);
        Currency ParseLiteral();

        Expression ParseExpression(Token identifierToken);
        Expression ParseMultiplicativeExpression(Token firstToken);
        Node ParsePrimaryExpression(Token firstToken);

        Condition ParseCondition();
        Condition ParseAndCondition();
        Condition ParseEqualityCondition();
        Condition ParseRelationalCondition();
        Node ParsePrimaryCondition();
    }
}
