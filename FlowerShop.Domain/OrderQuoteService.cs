namespace FlowerShop.Domain;

public class OrderQuoteService : IOrderQuoteService
{
    private readonly IBundleRepository _bundleRepository;
    private readonly IOrderRowFiller _orderRowFiller;
    
    public OrderQuoteService(IBundleRepository bundleRepository, IOrderRowFiller orderRowFiller)
    {
        _bundleRepository = bundleRepository;
        _orderRowFiller = orderRowFiller;
    }
    
    public async Task<OrderQuote> CreateQuoteFor(Order order)
    {
        var quote = new OrderQuote();

        foreach (var orderRow in order.Rows)
        {
            var productBundles = await _bundleRepository.GetBundlesFor(orderRow.ProductCode);
            var bundlesAndQuantities = _orderRowFiller.Fill(orderRow, productBundles);

            foreach (var bundleAndQuantity in bundlesAndQuantities)
            {
                var bundle = bundleAndQuantity.Item1;
                var bundleCount = bundleAndQuantity.Item2;
                
                if (bundleCount > 0)
                    quote.AddBundle(orderRow, bundleCount, bundle);
            }
        }

        return quote;
    }

}