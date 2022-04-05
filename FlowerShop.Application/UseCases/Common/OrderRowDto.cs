namespace FlowerShop.Application.UseCases.Common;

public class OrderRowDto
{
    public int Quantity { get; }
    public string ProductCode { get; }

    public OrderRowDto(int quantity, string productCode)
    {
        Quantity = quantity;
        ProductCode = productCode;
    }
}