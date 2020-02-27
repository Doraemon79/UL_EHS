using System;
using Microsoft.Extensions.Logging;
using PeterO.Numbers;
using UlApi.Models;
using UlApi.Services.Helpers;

namespace UlApi.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly ILogger<CalculationService> _logger;

        public CalculationService(ILogger<CalculationService> logger)
        {
            _logger = logger;
        }

        public string Calculate(Query query)
        {
            _logger.LogDebug($"Calculate");

            const int maxPrecision = 1000;
            var context = EContext.ForPrecision(Math.Min(query.Precision, maxPrecision));
            
            State state = new State(context);
            foreach (var c in query.Expression)
            {
                state = state.Next(c);
            }

            var result = state.Result;

            return result.ToString();

            //Another way how to deal with division by zero.
            //if (result.IsFinite) return result.ToString();
            //else
            //{
            //    throw new DivideByZeroException("Attempted to divide by zero.");
            //}
        }
    }
}