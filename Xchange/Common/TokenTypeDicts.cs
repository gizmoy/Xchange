using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Common
{
    public static class TokenTypeDicts
    {
        public static IDictionary<string, TokenTypeEnum> KeywordsToTokenTypes = new Dictionary<string, TokenTypeEnum>()
        {
            { "function", TokenTypeEnum.Function },
            { "if", TokenTypeEnum.If},
            { "while", TokenTypeEnum.While },
            { "else", TokenTypeEnum.Else },
            { "return", TokenTypeEnum.Return },
            { "continue", TokenTypeEnum.Continue },
            { "break", TokenTypeEnum.Break },
            { "var", TokenTypeEnum.Var },
            //{ "or", TokenTypeEnum.Or },
            //{ "and", TokenTypeEnum.And },
        };

        public static IDictionary<char, TokenTypeEnum> CharactersToTokenTypes = new Dictionary<char, TokenTypeEnum>()
        {
            { '(', TokenTypeEnum.ParenthesisOpen },
            { ')', TokenTypeEnum.ParenthesisClose },
            { '{', TokenTypeEnum.CurlyBracketOpen },
            { '}', TokenTypeEnum.CurlyBracketClose },
            { '[', TokenTypeEnum.SquareBracketOpen },
            { ']', TokenTypeEnum.SquareBracketClose },
            { ',', TokenTypeEnum.Comma },
            { ';', TokenTypeEnum.Semicolon },
            { '+', TokenTypeEnum.Plus },
            { '-', TokenTypeEnum.Minus },
            { '*', TokenTypeEnum.Multiply },
            { '/', TokenTypeEnum.Divide },
            { '%', TokenTypeEnum.Modulo },
            { '.', TokenTypeEnum.Dot }
        };
    }
}
