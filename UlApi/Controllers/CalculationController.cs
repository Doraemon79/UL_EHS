using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UlApi.Models;
using UlApi.Services;

namespace UlApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationController : ControllerBase
    {
        private readonly ILogger<CalculationController> _logger;
        private readonly ICalculationService _calculationService;

        public CalculationController(
            ICalculationService calculationService,
            ILogger<CalculationController> logger
        )
        {
            _calculationService = calculationService;
            _logger = logger;
        }

        [HttpPost]
        public Output Get(Query query)
        {
            try
            {
                _logger.LogInformation("");
                _logger.LogDebug($"Query: {query}");
                var result = _calculationService.Calculate(query);
                _logger.LogDebug($"Result: {result}");
                return new Output(result);
            }
            catch (FormatException exception)
            {
                return new Output(exception.Message, -1);
            }
            //Other implementations of ICalculation Service can throw DivideByZeroException instead of using Infinity value.
            //catch (DivideByZeroException exception)
            //{
            //    return new Output(exception.Message, -2);
            //}
            catch (Exception exception)
            {
                _logger.LogError(exception.StackTrace);
                throw;
            }
        }
    }
}
