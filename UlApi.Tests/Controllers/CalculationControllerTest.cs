using System;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using UlApi.Services;
using UlApi.Controllers;
using UlApi.Models;

namespace UlApi.Tests.Controllers
{
    [TestFixture]
    class CalculationControllerTest
    {
        private CalculationController _calculationController;

        [SetUp]
        public void Setup()
        {
            var logger = Mock.Of<ILogger<CalculationController>>();
            var serviceLogger = Mock.Of<ILogger<CalculationService>>();
            var calculationService = new CalculationService(serviceLogger);
            _calculationController = new CalculationController(calculationService, logger);
        }

        [Test]
        public void GetTest()
        {
            var query = new Query()
            {
                Expression = "3+2"
            };
            var output = _calculationController.Get(query);

            Assert.AreEqual("5", output.Result);
            Assert.AreEqual(0, output.ErrorCode);
            Assert.AreEqual(null, output.ErrorMessage);
        }


        [Test]
        public void IncompleteTest()
        {
            var query = new Query()
            {
                Expression = "3+2+"
            };
            var output = _calculationController.Get(query);
            Assert.AreEqual(null, output.Result);
            Assert.AreEqual(-1, output.ErrorCode);
            Assert.AreEqual("Incomplete expression", output.ErrorMessage);
        }
    }
}
