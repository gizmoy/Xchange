using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Xchange.Interfaces;
using Xchange.Stream;
using Xchange.Lexical;
using Xchange.Common;
using System.Collections.Generic;
using Xchange.Handlers.CurrencyHandler;

namespace Tests
{
    [TestClass]
    public class Test_Lexer
    {
        private ILexer SetupContextAndReturnLexer(string program)
        {
            var error = MockRepository.GenerateMock<IErrorHandler>();
            var currency = new CurrencyHandler();
            var reader = new StringReader();
            var source = new Source(reader, error);
            reader.Init(program);

            var sut = new Lexer(source, error, currency);

            return sut;
        }

        [TestMethod]
        public void Lexer_GetToken_ParseFunctionLiteral_CorrectToken()
        {
            // Arrange
            var program = @"
                function
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Function,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseParenthesisOpen_CorrectToken()
        {
            // Arrange
            var program = @"
                (
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.ParenthesisOpen,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseParenthesisClose_CorrectToken()
        {
            // Arrange
            var program = @"
                }
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.CurlyBracketClose,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseCurlyBracketOpen_CorrectToken()
        {
            // Arrange
            var program = @"
                {
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.CurlyBracketOpen,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseCurlyBracketClose_CorrectToken()
        {
            // Arrange
            var program = @"
                }
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.CurlyBracketClose,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseSquareBracketOpen_CorrectToken()
        {
            // Arrange
            var program = @"
                [
            ";
            var error = MockRepository.GenerateMock<IErrorHandler>();
            var currency = new CurrencyHandler();
            var reader = new StringReader();
            var source = new Source(reader, error);
            reader.Init(program);

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.SquareBracketOpen,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseSquareBracketClose_CorrectToken()
        {
            // Arrange
            var program = @"
                ]
            ";
            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.SquareBracketClose,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseComma_CorrectToken()
        {
            // Arrange
            var program = @"
                ,
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Comma,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseSemicolon_CorrectToken()
        {
            // Arrange
            var program = @"
                ;
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Semicolon,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseIf_CorrectToken()
        {
            // Arrange
            var program = @"
                if
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.If,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseWhile_CorrectToken()
        {
            // Arrange
            var program = @"
                while
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.While,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseElse_CorrectToken()
        {
            // Arrange
            var program = @"
                else
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Else,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseReturn_CorrectToken()
        {
            // Arrange
            var program = @"
                return
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Return,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseContinue_CorrectToken()
        {
            // Arrange
            var program = @"
                continue
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Continue,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseBreak_CorrectToken()
        {
            // Arrange
            var program = @"
                break
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Break,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseVar_CorrectToken()
        {
            // Arrange
            var program = @"
                var
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Var,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseNegation_CorrectToken()
        {
            // Arrange
            var program = @"
                !
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Negation,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseOr_CorrectToken()
        {
            // Arrange
            var program = @"
                ||
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Or,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseAnd_CorrectToken()
        {
            // Arrange
            var program = @"
                &&
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.And,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseEquality_CorrectToken()
        {
            // Arrange
            var program = @"
                ==
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Equality,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseAssignment_CorrectToken()
        {
            // Arrange
            var program = @"
                =
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Assignment,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseInequality_CorrectToken()
        {
            // Arrange
            var program = @"
                !=
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Inequality,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseLess_CorrectToken()
        {
            // Arrange
            var program = @"
                <
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Less,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseGreater_CorrectToken()
        {
            // Arrange
            var program = @"
                >
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Greater,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseLessOrEqual_CorrectToken()
        {
            // Arrange
            var program = @"
                <=
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.LessOrEqual,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseGreaterOrEqual_CorrectToken()
        {
            // Arrange
            var program = @"
                >=
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.GreaterOrEqual,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParsePlus_CorrectToken()
        {
            // Arrange
            var program = @"
                +
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Plus,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseMinus_CorrectToken()
        {
            // Arrange
            var program = @"
                -
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Minus,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseMultiply_CorrectToken()
        {
            // Arrange
            var program = @"
                *
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Multiply,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseDivide_CorrectToken()
        {
            // Arrange
            var program = @"
                /
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Divide,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseModulo_CorrectToken()
        {
            // Arrange
            var program = @"
                %
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Modulo,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseTypeUsd_CorrectToken()
        {
            // Arrange
            var program = @"
                usd
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Type,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            var value = string.Empty;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);

                if (token.Type == TokenTypeEnum.Type)
                {
                    value = token.Value;
                }

            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
            Assert.AreEqual(value, "usd");
        }

        [TestMethod]
        public void Lexer_GetToken_ParseTypeEur_CorrectToken()
        {
            // Arrange
            var program = @"
                eur
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Type,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            var value = string.Empty;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);

                if (token.Type == TokenTypeEnum.Type)
                {
                    value = token.Value;
                }

            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
            Assert.AreEqual(value, "eur");
        }

        [TestMethod]
        public void Lexer_GetToken_ParseTypePln_CorrectToken()
        {
            // Arrange
            var program = @"
                pln
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Type,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            var value = string.Empty;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);

                if (token.Type == TokenTypeEnum.Type)
                {
                    value = token.Value;
                }

            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
            Assert.AreEqual(value, "pln");
        }

        [TestMethod]
        public void Lexer_GetToken_ParseTypeGbp_CorrectToken()
        {
            // Arrange
            var program = @"
                gbp
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Type,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            var value = string.Empty;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);

                if (token.Type == TokenTypeEnum.Type)
                {
                    value = token.Value;
                }

            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
            Assert.AreEqual(value, "gbp");
        }

        [TestMethod]
        public void Lexer_GetToken_ParseDot_CorrectToken()
        {
            // Arrange
            var program = @"
                .
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Dot,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseDotMultiple_CorrectToken()
        {
            // Arrange
            var program = @"
                ...
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Dot,
                TokenTypeEnum.Dot,
                TokenTypeEnum.Dot,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseIdentifier_CorrectToken()
        {
            // Arrange
            var program = @"
                foo
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Identifier,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseNumberLiteral_CorrectToken()
        {
            // Arrange
            var program = @"
                324343291
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.NumberLiteral,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseInvalidNumberLiteral1_CorrectToken()
        {
            // Arrange
            var program = @"
                ^
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Invalid,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseInvalidNumberLiteral2_CorrectToken()
        {
            // Arrange
            var program = @"
                324343291q
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.NumberLiteral,
                TokenTypeEnum.Identifier,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseInvalidNumberLiteral3_CorrectToken()
        {
            // Arrange
            var program = @"
                32434#3291
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.NumberLiteral,
                TokenTypeEnum.Invalid,
                TokenTypeEnum.NumberLiteral,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseInvalid1_CorrectToken()
        {
            // Arrange
            var program = @"
                33~~dd
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.NumberLiteral,
                TokenTypeEnum.Invalid,
                TokenTypeEnum.Invalid,
                TokenTypeEnum.Identifier,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseInvalid2_CorrectToken()
        {
            // Arrange
            var program = @"
                ?
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Invalid,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseInvalid3_CorrectToken()
        {
            // Arrange
            var program = @"
                @
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Invalid,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseInvalid4_CorrectToken()
        {
            // Arrange
            var program = @"
                &
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Invalid,
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseEndOfFile_CorrectToken()
        {
            // Arrange
            var program = @"
            
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.EndOfFile
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }

        [TestMethod]
        public void Lexer_GetToken_ParseFunctionExample_CorrectTokens()
        {
            // Arrange
            var program = @"
                function foo(decimal a, pln b)
                {
	                return a * b - a;
                }
            ";

            var parsedTokenTypes = new List<TokenTypeEnum>();
            var correctTokenTypes = new List<TokenTypeEnum>(new TokenTypeEnum[]
            {
                TokenTypeEnum.Function,
                TokenTypeEnum.Identifier,
                TokenTypeEnum.ParenthesisOpen,
                TokenTypeEnum.Type,
                TokenTypeEnum.Identifier,
                TokenTypeEnum.Comma,
                TokenTypeEnum.Type,
                TokenTypeEnum.Identifier,
                TokenTypeEnum.ParenthesisClose,
                TokenTypeEnum.CurlyBracketOpen,
                TokenTypeEnum.Return,
                TokenTypeEnum.Identifier,
                TokenTypeEnum.Multiply,
                TokenTypeEnum.Identifier,
                TokenTypeEnum.Minus,
                TokenTypeEnum.Identifier,
                TokenTypeEnum.Semicolon,
                TokenTypeEnum.CurlyBracketClose,
                TokenTypeEnum.EndOfFile,
            });

            var sut = SetupContextAndReturnLexer(program);

            //Act
            Token token;
            do
            {
                token = sut.GetNextToken();
                parsedTokenTypes.Add(token.Type);
            } while (token.Type != TokenTypeEnum.EndOfFile);


            // Assert
            CollectionAssert.AreEqual(parsedTokenTypes, correctTokenTypes);
        }
    }
}