using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;
using Xchange.Stream;

namespace Tests
{
    [TestClass]
    public class Test_Source
    {
        [TestMethod]
        public void Source_GetCharacter_CorrectCharacterSequence()
        {
            // Arrange
            var errorHandler = MockRepository.GenerateMock<IErrorHandler>();
            var reader = new Reader(errorHandler);
            var path = @"Tests\Source_GetCharacter_CorrectCharacterSequence.txt";
            var sut = new Source(reader, errorHandler);

            var correct = new List<char>() { 'f', 'u', 'n', 'c', 't', 'i', 'o', 'n', '\uffff' };
            var read = new List<char>();

            reader.Init(path);

            // Act
            for (int i=0; i<correct.Count; ++i)
            {
                var character = sut.GetCharacter();
                read.Add(character);
            }


            // Assert
            CollectionAssert.AreEqual(correct, read);
        }

        [TestMethod]
        public void Source_GetCharacter_HasFinishedCorrect()
        {
            // Arrange
            var errorHandler = MockRepository.GenerateMock<IErrorHandler>();
            var reader = new Reader(errorHandler);
            var path = @"Tests\Source_GetCharacter_CorrectCharacterSequence.txt";
            var sut = new Source(reader, errorHandler);

            var correct = new List<char>() { 'f', 'u', 'n', 'c', 't', 'i', 'o', 'n', '\uffff' };
            var read = new List<char>();

            reader.Init(path);

            // Act
            for (int i = 0; i < correct.Count; ++i)
            {
                var character = sut.GetCharacter();
                read.Add(character);
            }


            // Assert
            Assert.IsTrue(sut.HasFinished);
        }
    }
}
