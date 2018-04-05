using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Common;
using Xchange.Common.Nodes;
using Xchange.Interfaces;
using Xchange.Lexical;
using System.Globalization;

namespace Xchange.Syntax
{
    public class Parser : IParser
    {
        private ILexer _Lexer;
        public ITracer _Tracer { get; private set; }
        private IErrorHandler _ErrorHandler;
        private ICurrencyHandler _CurrencyHandler;

        private Token _PreviousToken;
        public bool HasBufferedToken
        {
            get { return _PreviousToken.Type != TokenTypeEnum.Undefined; }
        }


        public Parser(ILexer lexer, ITracer tracer, IErrorHandler error, ICurrencyHandler currency)
        {
            _Lexer = lexer;
            _Tracer = tracer;
            _ErrorHandler = error;
            _CurrencyHandler = currency;

            _Tracer.Reset();
            ResetPreviousToken();
        }

        public Common.Nodes.Program Parse()
        {
            _Tracer.Enter("Starting parser tracing...");

            var syntaxTree = new Xchange.Common.Nodes.Program();
            FunDefinition lastFunction;

            while ((lastFunction = ParseFunction()) != null) 
            {
                syntaxTree.Functions.Add(lastFunction);

                if (Peek(TokenTypeEnum.EndOfFile))
                {
                    Accept(TokenTypeEnum.EndOfFile);
                    break;
                }
            }

            _Tracer.Leave("Trace ended...");

            return syntaxTree;
        }

        public bool IsAcceptable(Token token, params TokenTypeEnum[] acceptable)
        {
            foreach (var acc in acceptable)
            {
                if (token.Type == acc)
                {
                    return true;
                }
            }

            return false;
        }

        public Token Accept(params TokenTypeEnum[] acceptable)
        {
            Token token;

            if (HasBufferedToken)
            {
                token = _PreviousToken;
                ResetPreviousToken();
            }
            else
            {
                token = _Lexer.GetNextToken();
            }

            if (IsAcceptable(token, acceptable))
            {
                return token;
            }
            else
            {
                var sb = new StringBuilder();

                sb.Append("Unexpected token: ")
                    .Append(token.Type.ToString())
                    .Append(" (Line: ")
                    .Append(token.SourcePosition.Line)
                    .Append(", Pos: ")
                    .Append(token.SourcePosition.Position)
                    .Append(")")
                    .Append("\n")
                    .Append(_Lexer.GetLine(token.SourcePosition.LinePosition))
                    .Append("\n")
                    .Append(MakeErrorMarker(token.SourcePosition.Position));

                _ErrorHandler.Error(sb.ToString());

                throw new Exception();
            }
        }

        public bool Peek(params TokenTypeEnum[] acceptable)
        {
            if (HasBufferedToken == false)
            {
                _PreviousToken = _Lexer.GetNextToken();
            }

            return IsAcceptable(_PreviousToken, acceptable);
        }

        public Token GetPeeked()
        {
            if (HasBufferedToken == false)
            {
                _ErrorHandler.DebugFatal("Nothing peeked");
            }

            return _PreviousToken;
        }

        public void PeekFail()
        {
            var token = _PreviousToken;
            var sb = new StringBuilder();

            sb.Append("Unexpected token: ")
                    .Append(token.Type.ToString())
                    .Append(" (Line: ")
                    .Append(token.SourcePosition.Line)
                    .Append(", Pos: ")
                    .Append(token.SourcePosition.Position)
                    .Append(")")
                    .Append("\n")
                    .Append(_Lexer.GetLine(token.SourcePosition.LinePosition))
                    .Append("\n")
                    .Append(MakeErrorMarker(token.SourcePosition.Position));

            _ErrorHandler.Error(sb.ToString());
        }

        public string MakeErrorMarker(long position)
        {
            var sb = new StringBuilder();

            sb.Append(' ', (int)position+8)
                .Append("^");

            return sb.ToString();
        }

        public void ResetPreviousToken()
        {
            _PreviousToken.Type = TokenTypeEnum.Undefined;
            _PreviousToken.Value = "";
            _PreviousToken.SourcePosition = new SourcePosition();
        }

        public FunDefinition ParseFunction()
        {
            _Tracer.Enter("Parsing function");

            Accept(TokenTypeEnum.Function);

            // Identifier
            var tempToken = Accept(TokenTypeEnum.Identifier);
            var name = tempToken.Value;

            // Parse params
            Accept(TokenTypeEnum.ParenthesisOpen);
            IList<string> parameters = new List<string>();
            if (Peek(TokenTypeEnum.ParenthesisClose) == false)
            {
                parameters = ParseParameters();
            }
            Accept(TokenTypeEnum.ParenthesisClose);

            // Parse statement block
            var block = ParseStatementBlock();

            // Create fun definition node
            var node = new FunDefinition()
            {
                Name = name,
                Parameters = parameters,
                Block = block
            };

            _Tracer.Leave();

            return node;
        }

        public IList<string> ParseParameters()
        {
            _Tracer.Enter("Parsing parameters");

            var parametersNames = new List<string>();
            Token tempToken;

            tempToken = Accept(TokenTypeEnum.Identifier);
            parametersNames.Add(tempToken.Value);

            while (Peek(TokenTypeEnum.ParenthesisClose) == false)
            {
                Accept(TokenTypeEnum.Comma);
                tempToken = Accept(TokenTypeEnum.Identifier);
                parametersNames.Add(tempToken.Value);
            }

            _Tracer.Leave();

            return parametersNames;
        }

        public StatementBlock ParseStatementBlock()
        {
            _Tracer.Enter("Parsing statement block");

            Accept(TokenTypeEnum.CurlyBracketOpen);
  
            var node = new StatementBlock();
            var running = true;
            while (running)
            {
                if (Peek(TokenTypeEnum.If,
                        TokenTypeEnum.While,
                        TokenTypeEnum.Return,
                        TokenTypeEnum.Var,
                        TokenTypeEnum.CurlyBracketOpen,
                        TokenTypeEnum.Identifier,
                        TokenTypeEnum.Continue,
                        TokenTypeEnum.Break))
                {
                    var instruction = ParseInstruction();
                    if (instruction != null)
                    {
                        node.Instructions.Add(instruction);
                    }
                    else
                    {
                        running = false;
                    }
                }
                else
                {
                    running = false;
                }
            }

            Accept(TokenTypeEnum.CurlyBracketClose);

            _Tracer.Leave();

            return node;
        }

        public Node ParseInstruction()
        {
            var tempToken = GetPeeked();

            switch (tempToken.Type)
            {
                case TokenTypeEnum.If:
                    return ParseIfStatement();

                case TokenTypeEnum.While:
                    return ParseWhileStatement();

                case TokenTypeEnum.Return:
                    return ParseReturnStatement();

                case TokenTypeEnum.Var:
                    return ParseInitStatement();

                case TokenTypeEnum.CurlyBracketOpen:
                    return ParseStatementBlock();

                case TokenTypeEnum.Identifier:
                    return ParseAssignmentOrFunCall();

                case TokenTypeEnum.Continue:
                case TokenTypeEnum.Break:
                    return ParseLoopJump();

                default:
                    break;
            }

            return null;
        }

        public IfStatement ParseIfStatement()
        {
            _Tracer.Enter("Parsing if statement");

            Accept(TokenTypeEnum.If);

            Accept(TokenTypeEnum.ParenthesisOpen);     
            var condition = ParseCondition();
            Accept(TokenTypeEnum.ParenthesisClose);

            var trueBlock = ParseStatementBlock();
            var falseBlock = (StatementBlock) null;

            if (Peek(TokenTypeEnum.Else))
            {
                Accept(TokenTypeEnum.Else);

                falseBlock = ParseStatementBlock();
            }

            var node = new IfStatement()
            {
                Condition = condition,
                TrueBlock = trueBlock,
                FalseBlock = falseBlock
            };

            _Tracer.Leave();

            return node;
        }

        public WhileStatement ParseWhileStatement()
        {
            _Tracer.Enter("Parsing while statement");

            Accept(TokenTypeEnum.While);

            Accept(TokenTypeEnum.ParenthesisOpen);
            var condition = ParseCondition();
            Accept(TokenTypeEnum.ParenthesisClose);

            var block = ParseStatementBlock();
            var node = new WhileStatement()
            {
                Condition = condition,
                Block = block
            };

            _Tracer.Leave();

            return node;
        }

        public ReturnStatement ParseReturnStatement()
        {
            _Tracer.Enter("Parsing return statement");

            Accept(TokenTypeEnum.Return);
            var value = ParseAssignable();
            Accept(TokenTypeEnum.Semicolon);

            var node = new ReturnStatement()
            {
                Value = value
            };

            _Tracer.Leave();

            return node;
        }

        public VarDeclaration ParseInitStatement()
        {
            _Tracer.Enter("Parsing init statement");

            Accept(TokenTypeEnum.Var);

            var nameToken = Accept(TokenTypeEnum.Identifier);
            var name = nameToken.Value;
            var value = (Assignable)null;
      
            if (Peek(TokenTypeEnum.Assignment))
            {
                Accept(TokenTypeEnum.Assignment);
                value = ParseAssignable();
            }

            Accept(TokenTypeEnum.Semicolon);

            var node = new VarDeclaration()
            {
                Name = name,
                Value = value
            };

            _Tracer.Leave();

            return node;
        }

        public Node ParseAssignmentOrFunCall()
        {
            _Tracer.Enter("Parsing assignment or function call");

            var tempToken = Accept(TokenTypeEnum.Identifier);

            var node = (Node)ParseFunCall(tempToken.Value);
            if (node == null)
            {
                var variable = ParseVariable(tempToken);
                Accept(TokenTypeEnum.Assignment);
                var value = ParseAssignable();

                node = new Assignment()
                {
                    Variable = variable,
                    Value = value
                };
            }

            Accept(TokenTypeEnum.Semicolon);

            _Tracer.Leave();

            return node;
        }

        public LoopJump ParseLoopJump()
        {
            _Tracer.Enter("Parsing loop jump");

            var token = Accept(TokenTypeEnum.Continue, TokenTypeEnum.Break);
            var isBreak = token.Type == TokenTypeEnum.Break;

            Accept(TokenTypeEnum.Semicolon);

            var node = new LoopJump()
            {
                IsBreak = isBreak
            };

            _Tracer.Leave();

            return node;
        }

        public Assignable ParseAssignable()
        {
            _Tracer.Enter("Parsing assignable");

            Assignable node;

            if (Peek(TokenTypeEnum.Identifier))
            {
                var tempToken = Accept(TokenTypeEnum.Identifier);

                node = ParseFunCall(tempToken.Value);
                if (node == null)
                {
                    node = ParseExpression(tempToken);
                }
            }
            else
            {
                var tempToken = new Token();
                tempToken.Type = TokenTypeEnum.Undefined;
                node = ParseExpression(tempToken);
            }


            _Tracer.Leave();

            return node;
        }

        public Call ParseFunCall(string identifier)
        {
            _Tracer.Enter("Parsing function call");

            if (!Peek(TokenTypeEnum.ParenthesisOpen))
             {
                _Tracer.Leave("Not a function call");
                return null;
            }

            var node = new Call();
            node.Name = identifier;

            Accept(TokenTypeEnum.ParenthesisOpen);

            if (Peek(TokenTypeEnum.ParenthesisClose) == false)
            {
                node.Arguments.Add(ParseAssignable());
                while (Peek(TokenTypeEnum.ParenthesisClose) == false)
                {
                    var tempToken = Accept(TokenTypeEnum.Comma);
                    node.Arguments.Add(ParseAssignable());
                }
            }

            Accept(TokenTypeEnum.ParenthesisClose);

            if (Peek(TokenTypeEnum.Dot))
            {
                Accept(TokenTypeEnum.Dot);
                var tempToken = Accept(TokenTypeEnum.Type);
                node.Multiplier = 1.0m / _CurrencyHandler.CurrencyValueDict[tempToken.Value];
            }

            _Tracer.Leave("  + function call");

            return node;
        }

        public Node ParseFunCallOrVariable(Token identifierToken)
        {
            _Tracer.Enter("Parsing fun call or variable");

            var name = string.Empty;
            if (identifierToken.Type != TokenTypeEnum.Identifier)
            {
                var tempToken = Accept(TokenTypeEnum.Identifier);
                name = tempToken.Value;
            }
            else
            {
                name = identifierToken.Value;
            }

            var node = (Node)ParseFunCall(name);
            if (node == null)
            {
                node = new Variable()
                {
                    Name = name
                };
            }

            _Tracer.Leave();

            return node;
        }

        public Variable ParseVariable(Token identifierToken)
        {
            _Tracer.Enter("Parsing variable");

            var name = string.Empty;
            if (identifierToken.Type != TokenTypeEnum.Identifier)
            {
                var tempToken = Accept(TokenTypeEnum.Identifier);
                name = tempToken.Value;
            }
            else
            {
                name = identifierToken.Value;
            }

            var node = new Variable()
            {
                Name = name
            };

            if (Peek(TokenTypeEnum.Dot))
            {
                Accept(TokenTypeEnum.Dot);
                var tempToken = Accept(TokenTypeEnum.Type);
                node.Multiplier = 1.0m / _CurrencyHandler.CurrencyValueDict[tempToken.Value];
            }

            _Tracer.Leave();

            return node;
        }

        public Currency ParseLiteral()
        {
            _Tracer.Enter("Parsing literal");

            var negative = false;
            var currency = "decimal";
            var sb = new StringBuilder();

            if (Peek(TokenTypeEnum.Minus))
            {
                Accept(TokenTypeEnum.Minus);
                negative = true;
            }

            var tempToken = Accept(TokenTypeEnum.NumberLiteral);
            sb.Append(tempToken.Value);

            if (Peek(TokenTypeEnum.Dot))
            {
                Accept(TokenTypeEnum.Dot);
                sb.Append(".");

                if (Peek(TokenTypeEnum.NumberLiteral) || Peek(TokenTypeEnum.DecimalNumberLiteral))
                {
                    tempToken = Accept(TokenTypeEnum.NumberLiteral, TokenTypeEnum.DecimalNumberLiteral);
                    sb.Append(tempToken.Value);

                    if (Peek(TokenTypeEnum.Dot))
                    {
                        Accept(TokenTypeEnum.Dot);
                        tempToken = Accept(TokenTypeEnum.Type);
                        currency = tempToken.Value;
                    }
                }
                else
                {
                    tempToken = Accept(TokenTypeEnum.Type);
                    currency = tempToken.Value;
                }
            }

            var node = new Currency();

            node.Value = decimal.Parse(sb.ToString(), CultureInfo.InvariantCulture);

            if (negative)
            {
                node.Value *= -1;
            }

            if (_CurrencyHandler.IsCurrencyValid(currency))
            {
                node.Value *= 1.0m / _CurrencyHandler.CurrencyValueDict[currency];
            }

            _Tracer.Leave();

            return node;
        }

        public Expression ParseExpression(Token firstToken)
        {
            _Tracer.Enter("Parsing expression");

            var node = new Expression();

            node.Operands.Add(ParseMultiplicativeExpression(firstToken));

            while (Peek(TokenTypeEnum.Plus, TokenTypeEnum.Minus))
            {
                var tempToken = Accept(TokenTypeEnum.Plus, TokenTypeEnum.Minus);
                node.Operations.Add(tempToken.Type);

                var token = new Token();
                token.Type = TokenTypeEnum.Undefined;
                node.Operands.Add(ParseMultiplicativeExpression(token));
            }

            _Tracer.Leave();

            return node;
        }

        public Expression ParseMultiplicativeExpression(Token firstToken)
        {
            _Tracer.Enter("Parsing multiplicative expression");

            var node = new Expression();

            node.Operands.Add(ParsePrimaryExpression(firstToken));

            while (Peek(TokenTypeEnum.Multiply, TokenTypeEnum.Divide, TokenTypeEnum.Modulo))
            {
                var tempToken = Accept(TokenTypeEnum.Multiply, TokenTypeEnum.Divide, TokenTypeEnum.Modulo);
                node.Operations.Add(tempToken.Type);

                var token = new Token();
                token.Type = TokenTypeEnum.Undefined;
                node.Operands.Add(ParsePrimaryExpression(token));
            }

            _Tracer.Leave();

            return node;
        }

        public Node ParsePrimaryExpression(Token firstToken)
        {
            _Tracer.Enter("Parsing primary expression");
            _Tracer.Info($"First Token type = {firstToken.Type.ToString()}");

            Node node;

            if (firstToken.Type != TokenTypeEnum.Undefined)
            {
                node = ParseVariable(firstToken);

                _Tracer.Leave();

                return node;
            }

            var token = new Token();
            token.Type = TokenTypeEnum.Undefined;

            if (Peek(TokenTypeEnum.ParenthesisOpen))
            {
                Accept(TokenTypeEnum.ParenthesisOpen );
                node = ParseExpression(token);
                Accept(TokenTypeEnum.ParenthesisClose);

                _Tracer.Leave();

                return node;
            }

            if (Peek( TokenTypeEnum.Identifier))
            {
                node = ParseFunCallOrVariable(token);

                _Tracer.Leave();
                return node;
            }

            node = ParseLiteral();

            _Tracer.Leave();

            return node;
        }

        public Condition ParseCondition()
        {
            _Tracer.Enter("Parsing condition");

            var node = new Condition();

            node.Operands.Add(ParseAndCondition());

            while (Peek(TokenTypeEnum.Or))
             {
                Accept(TokenTypeEnum.Or);
                node.Operation = TokenTypeEnum.Or;

                node.Operands.Add(ParseAndCondition());
            }

            _Tracer.Leave();

            return node;
        }

        public Condition ParseAndCondition()
        {
            _Tracer.Enter("Parsing and condition");

            var node = new Condition();

            node.Operands.Add(ParseEqualityCondition());

            while (Peek(TokenTypeEnum.And))
            {
                Accept(TokenTypeEnum.And);
                node.Operation = TokenTypeEnum.And;

                node.Operands.Add(ParseEqualityCondition());
            }

            _Tracer.Leave();

            return node;
        }

        public Condition ParseEqualityCondition()
        {
            _Tracer.Enter("Parsing equality condition");

            var node = new Condition();

            node.Operands.Add(ParseRelationalCondition());

            while (Peek(TokenTypeEnum.Equality, TokenTypeEnum.Inequality))
            {
                var tempToken = Accept(TokenTypeEnum.Equality, TokenTypeEnum.Inequality);
                node.Operation = tempToken.Type;

                node.Operands.Add(ParseEqualityCondition());
            }

            _Tracer.Leave();

            return node;
        }

        public Condition ParseRelationalCondition()
        {
            _Tracer.Enter("Parsing relational condition");

            var node = new Condition();

            node.Operands.Add(ParsePrimaryCondition());

            while (Peek(TokenTypeEnum.Less, TokenTypeEnum.Greater, TokenTypeEnum.LessOrEqual, TokenTypeEnum.GreaterOrEqual))
            {
                var tempToken = Accept(TokenTypeEnum.Less, TokenTypeEnum.Greater, TokenTypeEnum.LessOrEqual, TokenTypeEnum.GreaterOrEqual);
                node.Operation = tempToken.Type;

                node.Operands.Add(ParsePrimaryCondition());
            }

            _Tracer.Leave();

            return node;
        }

        public Node ParsePrimaryCondition()
        {
            _Tracer.Enter("Parsing primary condition");

            var node = new Condition();

            if (Peek(TokenTypeEnum.Negation))
            {
                Accept(TokenTypeEnum.Negation);

                node.Negated = true;
            }

            if (Peek(TokenTypeEnum.ParenthesisOpen))
             {
                Accept(TokenTypeEnum.ParenthesisOpen);
                node.Operands.Add(ParseCondition());
                Accept(TokenTypeEnum.ParenthesisClose);
            }
            else
            {
                if (Peek(TokenTypeEnum.Identifier))
                {
                    var token = new Token();
                    token.Type = TokenTypeEnum.Undefined;
                    node.Operands.Add(ParseFunCallOrVariable(token));
                }
                else
                {
                    node.Operands.Add(ParseLiteral());
                }
            }

            if (node.Negated == true)
            {
                _Tracer.Leave();

                return node.LeftSide;
            }

            _Tracer.Leave();

            return node;
        }
    }
}
