using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common
{
    public enum TokenTypeEnum
    {
        Function = 0,
        ParenthesisOpen = 1,
        ParenthesisClose = 2,
        CurlyBracketOpen = 3, 
        CurlyBracketClose = 4,
        SquareBracketOpen = 5,
        SquareBracketClose = 6,
        Comma = 7,
        Semicolon = 8,
        If = 9,
        While = 10,
        Else = 11,
        Return = 12,
        Continue = 13,
        Break = 14,
        Var = 15, 
        Negation = 16,
        Assignment = 17,
        Or = 18,
        And = 19,
        Equality = 20,
        Inequality = 21,
        Less = 22,
        Greater = 23,
        LessOrEqual = 24,
        GreaterOrEqual = 25,
        Plus = 26,
        Minus = 27,
        Multiply = 28,
        Divide = 29,
        Modulo = 30,
        Type = 31,
        Dot = 32,
        Identifier = 33,
        NumberLiteral = 34,
        Invalid = 35,
        EndOfFile = 36,
        Undefined = 37,
        DecimalNumberLiteral = 38,
    }
}
