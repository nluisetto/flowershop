namespace FlowerShop.Domain;

public class Bundle
{
    public string ProductCode { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }

    public Bundle(string productCode, int quantity, decimal price)
    {
        ProductCode = productCode;
        Quantity = quantity;
        Price = price;
    }
}