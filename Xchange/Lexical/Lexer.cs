using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Common;
using Xchange.Interfaces;
using Xchange.Stream;

namespace Xchange.Lexical
{
    public class Lexer : ILexer
    {
        private ISource _Source;
        private IErrorHandler _ErrorHandler;
        private ICurrencyHandler _CurrencyHandler;

        private char _Character;
        private bool _IsFirstGetNextToken = true;
        private SourcePosition _TokenPosition;


        public Lexer(ISource source, IErrorHandler error, ICurrencyHandler currency)
        {
            _Source = source;
            _ErrorHandler = error;
            _CurrencyHandler = currency;
        }


        public Token GetNextToken()
        {
            // Check whether first call
            if (_IsFirstGetNextToken)
            {
                _Character = GetNextChar();
                _IsFirstGetNextToken = false;
            }

            // Skip white characters
            while (char.IsWhiteSpace(_Character))
            {
                _Character = GetNextChar();
            }

            // Save positions
            _TokenPosition = new SourcePosition()
            {
                Line = _Source.CurrentLine,
                Position = _Source.CurrentColumn - 1,
                LinePosition = _Source.CurrentLinePosition
            };

            // Check whether is end of file
            if (_Source.HasFinished)
            {
                return new Token(TokenTypeEnum.EndOfFile, _TokenPosition);
            }

            Token? token;

            // Try parse string
            if ((token = TryParseString(_TokenPosition)) != null)
            {
                return token.Value;
            }

            // Try parse digit
            if ((token = TryParseDigit(_TokenPosition)) != null)
            {
                return token.Value;
            }

            // Try parse other
            if ((token = TryParseOther(_TokenPosition)) != null)
            {
                return token.Value;
            }
            
            return new Token(TokenTypeEnum.Invalid, _TokenPosition);
        }

        public string GetLine(long linePosition)
        {
            return _Source.GetLine(linePosition);
        }

        public Token? TryParseString(SourcePosition position)
        {
            if (char.IsLetter(_Character) || _Character == '_' || _Character == '$')
            {
                // Build string token
                var sb = new StringBuilder();
                while (char.IsLetter(_Character) || _Character == '_' || _Character == '$')
                {
                    sb.Append(_Character);
                    _Character = GetNextChar();
                }
                var key = sb.ToString();


                // Check type
                if (TokenTypeDicts.KeywordsToTokenTypes.ContainsKey(key))
                {
                    return new Token(TokenTypeDicts.KeywordsToTokenTypes[key], position);
                }
                else if (_CurrencyHandler.IsCurrencyValid(key.ToLower()) || key.ToLower() == "decimal")
                {
                    return new Token(TokenTypeEnum.Type, position, key.ToLower());
                }

                return new Token(TokenTypeEnum.Identifier, position, key);
            }

            return null;
        }

        public Token? TryParseDigit(SourcePosition position)
        {
            if (char.IsDigit(_Character))
            {
                var firstCharacter = _Character;
                var sb = new StringBuilder();

                do
                {
                    sb.Append(_Character);
                    _Character = GetNextChar();
                }
                while (char.IsDigit(_Character));

                var number = sb.ToString();
                var type = firstCharacter == '0' && number != "0"
                    ? TokenTypeEnum.DecimalNumberLiteral
                    : TokenTypeEnum.NumberLiteral;

                return new Token(type, position, number);
            }

            return null;
        }

        public Token? TryParseOther(SourcePosition position)
        {
            var nextCharacter = GetNextChar();

            switch (_Character)
            {
                case '=':

                    if (nextCharacter == '=')
                    {
                        _Character = GetNextChar();
                        return new Token(TokenTypeEnum.Equality, position);
                    }
                    else
                    {
                        _Character = nextCharacter;
                        return new Token(TokenTypeEnum.Assignment, position);
                    }

                case '<':

                    if (nextCharacter == '=')
                    {
                        _Character = GetNextChar();
                        return new Token(TokenTypeEnum.LessOrEqual, position);
                    }
                    else
                    {
                        _Character = nextCharacter;
                        return new Token(TokenTypeEnum.Less, position);
                    }

                case '>':

                    if (nextCharacter == '=')
                    {
                        _Character = GetNextChar();
                        return new Token(TokenTypeEnum.GreaterOrEqual, position);
                    }
                    else
                    {
                        _Character = nextCharacter;
                        return new Token(TokenTypeEnum.Greater, position);
                    }

                case '!':

                    if (nextCharacter == '=')
                    {
                        _Character = GetNextChar();
                        return new Token(TokenTypeEnum.Inequality, position);
                    }
                    else
                    {
                        _Character = nextCharacter;
                        return new Token(TokenTypeEnum.Negation, position);
                    }

                case '&':

                    if (nextCharacter == '&')
                    {
                        _Character = GetNextChar();
                        return new Token(TokenTypeEnum.And, position);
                    }

                    break;

                case '|':

                    if (nextCharacter == '|')
                    {
                        _Character = GetNextChar();
                        return new Token(TokenTypeEnum.Or, position);
                    }

                    break;

                default:

                    if (TokenTypeDicts.CharactersToTokenTypes.ContainsKey(_Character))
                    {
                        var character = _Character;
                        _Character = nextCharacter;
                        return new Token(TokenTypeDicts.CharactersToTokenTypes[character], position);
                    }

                    break;
            }

            _Character = nextCharacter;

            return null;
        }

        public char GetNextChar()
        {
            return _Source.GetCharacter();
        }
    }
}
