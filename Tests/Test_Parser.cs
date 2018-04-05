using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Xchange.Stream;
using Xchange.Interfaces;
using Xchange.Handlers.CurrencyHandler;
using Xchange.Lexical;
using System.Collections.Generic;
using Xchange.Common;
using Xchange.Syntax;
using Xchange.Handlers.ErrorHandler;
using Xchange.Common.Nodes;

namespace Tests
{
    [TestClass]
    public class Test_Parser
    {
        private IParser SetupContextAndReturnParser(string program)
        {
            var error = MockRepository.GenerateMock<IErrorHandler>();
            var currency = new CurrencyHandler();
            var reader = new StringReader();
            var source = new Source(reader, error);
            var lexer = new Lexer(source, error, currency);
            var tracer = new Tracer(error);
            reader.Init(program);

            var sut = new Parser(lexer, tracer, error, currency);

            return sut;
        }

        [TestMethod]
        public void Parse_ParseFunction_CorrectResult()
        {
            // Arrange
            var program = @"
                function x(a) { return 0; }
            ";

            var sut = SetupContextAndReturnParser(program);

            //Act
            var result = sut.ParseFunction();


            // Assert
            Assert.AreEqual(result.Name, "x");
            Assert.AreEqual(result.Type, NodeTypeEnum.FunDefinition);
            Assert.AreEqual(result.Parameters.Count, 1);
            Assert.AreEqual(result.Parameters[0], "a");
            Assert.AreEqual(result.Block.Type, NodeTypeEnum.StatementBlock);
            Assert.AreEqual(result.Block.Instructions.Count, 1);
            Assert.AreEqual(result.Block.Instructions[0].Type, NodeTypeEnum.ReturnStatement);
        }

        [TestMethod]
        public void Parse_ParseParameters_CorrectParameters_CorrectResult()
        {
            // Arrange
            var program = @"
                a,b,c,d)
            ";

            var sut = SetupContextAndReturnParser(program);

            //Act
            var result = sut.ParseParameters();


            // Assert
            Assert.AreEqual(result.Count, 4);
            Assert.AreEqual(result[0], "a");
            Assert.AreEqual(result[1], "b");
            Assert.AreEqual(result[2], "c");
            Assert.AreEqual(result[3], "d");
        }

        [TestMethod]
        public void Parse_ParseStatementBlock_CorrectInstructions_CorrectResult()
        {
            // Arrange
            var program = @"
            {
                var a;
                a = b;
                if (a > 5)
                {
                    a = a + 1;
                }
                return a;
            }
            ";

            var sut = SetupContextAndReturnParser(program);

            //Act
            var result = sut.ParseStatementBlock();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.StatementBlock);
            Assert.AreEqual(result.Instructions.Count, 4);
            Assert.AreEqual(result.Instructions[0].Type, NodeTypeEnum.VarDeclaration);
            Assert.AreEqual(result.Instructions[1].Type, NodeTypeEnum.Assignment);
            Assert.AreEqual(result.Instructions[2].Type, NodeTypeEnum.IfStatement);
            Assert.AreEqual(result.Instructions[3].Type, NodeTypeEnum.ReturnStatement);
        }

        [TestMethod]
        public void Parse_ParseIfStatement_CorrectBuildWithoutElse_CorrectResult()
        {
            // Arrange
            var program = @"
                if (a > 5)
                {
                    a = a + 1;
                }
            ";

            var sut = SetupContextAndReturnParser(program);

            //Act
            var result = sut.ParseIfStatement();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.IfStatement);
            Assert.AreEqual(result.Condition.Negated, false);
            Assert.AreEqual(result.TrueBlock.Type, NodeTypeEnum.StatementBlock);
            Assert.AreEqual(result.FalseBlock, null);
        }

        [TestMethod]
        public void Parse_ParseIfStatement_CorrectBuildWithElse_CorrectResult()
        {
            // Arrange
            var program = @"
                if (a > 5)
                {
                    a = a + 1;
                }
                else
                {
                    a = a + 2;
                }
            ";

            var sut = SetupContextAndReturnParser(program);

            //Act
            var result = sut.ParseIfStatement();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.IfStatement);
            Assert.AreEqual(result.Condition.Negated, false);
            Assert.AreEqual(result.TrueBlock.Type, NodeTypeEnum.StatementBlock);
            Assert.AreEqual(result.FalseBlock.Type, NodeTypeEnum.StatementBlock);
        }

        [TestMethod]
        public void Parse_ParseWhileStatement_CorrectBuild_CorrectResult()
        {
            // Arrange
            var program = @"
                while (a > 5)
                {
                    a = a + 1;
                }
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseWhileStatement();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.WhileStatement);
            Assert.AreEqual(result.Condition.Negated, false);
            Assert.AreEqual(result.Block.Type, NodeTypeEnum.StatementBlock);
        }

        [TestMethod]
        public void Parse_ParseReturnStatement_CorrectBuild_CorrectResult()
        {
            // Arrange
            var program = @"
                return a;
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseReturnStatement();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.ReturnStatement);
        }

        [TestMethod]
        public void Parse_ParseInitStatement_CorrectBuildWithoutAssignment_CorrectResult()
        {
            // Arrange
            var program = @"
                var a;
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseInitStatement();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.VarDeclaration);
            Assert.AreEqual(result.Name, "a");
        }

        [TestMethod]
        public void Parse_ParseInitStatement_CorrectBuildWithAssignment_CorrectResult()
        {
            // Arrange
            var program = @"
                var a = 5;
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseInitStatement();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.VarDeclaration);
            Assert.AreEqual(result.Name, "a");
        }

        [TestMethod]
        public void Parse_ParseAssignmentOrFunCall_CorrectBuildWithAssignment_CorrectResult()
        {
            // Arrange
            var program = @"
                a = 5;
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseAssignmentOrFunCall();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.Assignment);
        }

        [TestMethod]
        public void Parse_ParseAssignmentOrFunCall_CorrectBuildWithFunCall_CorrectResult()
        {
            // Arrange
            var program = @"
                a();
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseAssignmentOrFunCall();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.Call);
        }

        [TestMethod]
        public void Parse_ParseLoopJump_CorrectBuildWithBreak_CorrectResult()
        {
            // Arrange
            var program = @"
                break;
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseLoopJump();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.LoopJump);
            Assert.AreEqual(result.IsBreak, true);
        }

        [TestMethod]
        public void Parse_ParseLoopJump_CorrectBuildWithContinue_CorrectResult()
        {
            // Arrange
            var program = @"
                continue;
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseLoopJump();


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.LoopJump);
            Assert.AreEqual(result.IsBreak, false);
        }

        [TestMethod]
        public void Parse_ParseAssignable_CorrectBuildWithContinue_CorrectResult()
        {
            // Arrange
            var program = @"
                a
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseAssignable();


            // Assert
            // Not throwing exception
        }

        [TestMethod]
        public void Parse_ParseFunCall_CorrectBuildWithoutType_CorrectResult()
        {
            // Arrange
            var program = @"
                ();
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseFunCall("a");


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.Call);
            Assert.AreEqual(result.Name, "a");
            Assert.AreEqual(result.Multiplier.HasValue, false);
        }

        [TestMethod]
        public void Parse_ParseFunCall_CorrectBuildWithType_CorrectResult()
        {
            // Arrange
            var program = @"
                ().usd;
            ";

            var sut = SetupContextAndReturnParser(program);


            //Act
            var result = sut.ParseFunCall("a");


            // Assert
            Assert.AreEqual(result.Type, NodeTypeEnum.Call);
            Assert.AreEqual(result.Name, "a");
            Assert.AreEqual(result.Multiplier.HasValue, true);
        }
    }
}
