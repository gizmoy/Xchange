using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common.Nodes
{
    public enum NodeTypeEnum
    {
        Assignment,
        Call,
        Condition,
        Expression,
        FunDefinition,
        IfStatement,
        LoopJump,
        Currency,
        Program,
        ReturnStatement,
        StatementBlock,
        VarDeclaration,
        Variable,
        WhileStatement
    }
}
