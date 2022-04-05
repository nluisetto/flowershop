namespace FlowerShop.Application.UseCases.Common;

public class BundleDto
{
    public int Quantity { get; }
    public decimal Price { get; }

    public BundleDto(int quantity, decimal price)
    {
        Quantity = quantity;
        Price = price;
    }
}