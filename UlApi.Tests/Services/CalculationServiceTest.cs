using System;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UlApi.Models;
using UlApi.Services;

namespace UlApi.Tests.Services
{
    [TestFixture]
    class CalculationServiceTest
    {
        private ICalculationService _calculationService;

        [SetUp]
        public void Setup()
        {
            var logger = Mock.Of<ILogger<CalculationService>>();
            _calculationService = new CalculationService(logger);
        }

        [Test]
        public void AdditionTest()
        {
            var query = new Query()
            {
                Expression = "2+3"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("5", result);
        }

        [Test]
        public void SubtractionTest()
        {
            var query = new Query()
            {
                Expression = "2-3"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("-1", result);
        }


        [Test]
        public void MultiplicationTest()
        {
            var query = new Query()
            {
                Expression = "2*3"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("6", result);
        }

        [Test]
        public void IntegerDivisionTest()
        {
            var query = new Query()
            {
                Expression = "6/2"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("3", result);
        }
        
        [Test]
        public void FloatDivisionTest()
        {
            var query = new Query()
            {
                Expression = "5/2"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("2.5", result);
        }

        [Test]
        public void DefaultPrecisionTest()
        {
            var query = new Query()
            {
                Expression = "1/3"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("0.333333333", result);
        }

        [Test]
        public void ExplicitPrecisionTest()
        {
            var query = new Query()
            {
                Expression = "1/3",
                Precision = 12
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("0.333333333333", result);
        }
        
        [Test]
        public void DivisionByZeroTest()
        {
            var query = new Query()
            {
                Expression = "1/0"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("Infinity", result);
        }

        [Test]
        public void NegativeDivisionByZeroTest()
        {
            var query = new Query()
            {
                Expression = "2-3/0"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("-Infinity", result);
        }

        [Test]
        public void NanTest()
        {
            var query = new Query()
            {
                Expression = "0/0"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("NaN", result);
        }
        
        [Test]
        public void PriorityTest()
        {
            var query = new Query()
            {
                Expression = "2+3*4"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("14", result);
        }

        [Test]
        public void DivisionPriorityTest()
        {
            var query = new Query()
            {
                Expression = "16+12/4"
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("19", result);
        }

        [Test]
        public void BigNumbersTest()
        {
            var query = new Query()
            {
                Expression = "100000000000000000000000000000000000000000000000*10000000000000000000000*123+345",
                Precision = 1000
            };
            var result = _calculationService.Calculate(query);
            Assert.AreEqual("123000000000000000000000000000000000000000000000000000000000000000000345", result);
        }

        [Test]
        public void IncompleteTest()
        {
            var query = new Query()
            {
                Expression = "3+2+"
            };
            var exception = Assert.Throws<FormatException>(() => { _calculationService.Calculate(query); });
            Assert.AreEqual("Incomplete expression", exception.Message);
        }

        [Test]
        public void UnexpectedCharacterTest()
        {
            var query = new Query()
            {
                Expression = "3x+2"
            };
            var exception = Assert.Throws<FormatException>(() => { _calculationService.Calculate(query); });
            Assert.AreEqual("Unexpected character 'x'", exception.Message);
        }


        [Test]
        public void MisplacedCharacterTest()
        {
            var query = new Query()
            {
                Expression = "3++2"
            };
            var exception = Assert.Throws<FormatException>(() => { _calculationService.Calculate(query); });
            Assert.AreEqual("Misplaced character '+'", exception.Message);
        }
    }
}
