namespace FlowerShop.Application.UseCases.Common;

public class QuoteRowDto
{
    public OrderRowDto OrderRowDto { get; }
    public IEnumerable<BundleDetailDto> BundleDetails { get; }
    public decimal TotalPrice { get; }

    public QuoteRowDto(OrderRowDto orderRowDto, IEnumerable<BundleDetailDto> bundleDetails, decimal totalPrice)
    {
        OrderRowDto = orderRowDto;
        BundleDetails = bundleDetails;
        TotalPrice = totalPrice;
    }

    public class BundleDetailDto
    {
        public int Count { get; }
        public BundleDto Bundle { get; }
        public decimal TotalPrice { get; }

        public BundleDetailDto(int count, BundleDto bundle, decimal totalPrice)
        {
            Count = count;
            Bundle = bundle;
            TotalPrice = totalPrice;
        }
    }
}