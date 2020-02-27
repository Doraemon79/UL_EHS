using UlApi.Models;

namespace UlApi.Services
{
    public interface ICalculationService
    {
        string Calculate(Query query);
    }
}