namespace FlowerShop.Domain;

public interface IOrderQuoteService
{
    public Task<OrderQuote> CreateQuoteFor(Order order);
}