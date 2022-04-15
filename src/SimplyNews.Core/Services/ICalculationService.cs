namespace SimplyNews.Core.services
{
    public interface ICalculationService
    {
        double TipAmount(double billTotal, int generosity);
    }
}
