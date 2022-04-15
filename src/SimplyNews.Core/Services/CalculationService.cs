
namespace SimplyNews.Core.services
{
    public class CalculationService : ICalculationService
    {
        public double TipAmount(double billTotal, int generosity)
        {
            return billTotal * ((double)generosity / 100);
        }
    }
}
