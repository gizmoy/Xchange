using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Xchange.Interfaces;
using Xchange.Stream;

namespace Tests
{
    [TestClass]
    public class Test_Reader
    {
        [TestMethod]
        public void Reader_InvalidFilepath_CallErrorOnHandler()
        {
            // Arrange
            var errorHandler = MockRepository.GenerateMock<IErrorHandler>();
            errorHandler.Expect(o => o.Error(Arg<string>.Is.Anything, Arg<bool>.Is.Anything)).Repeat.Once();
            var sut = new Reader(errorHandler);
            var invalidFilepath = @"invalid:\path\to\file.txt";

            // Act
            sut.Init(invalidFilepath);

            // Assert
            errorHandler.Replay();
            errorHandler.VerifyAllExpectations();
        }

        [TestMethod]
        public void Reader_ValidFilepath_NotCallErrorOnHandler()
        {
            // Arrange
            var errorHandler = MockRepository.GenerateMock<IErrorHandler>();
            errorHandler.Expect(o => o.Error(Arg<string>.Is.Anything, Arg<bool>.Is.Anything)).Repeat.Never();
            var sut = new Reader(errorHandler);
            var validFilepath = @"Tests\test.txt";

            // Act
            sut.Init(validFilepath);

            // Assert
            errorHandler.VerifyAllExpectations();
        }
    }
}
