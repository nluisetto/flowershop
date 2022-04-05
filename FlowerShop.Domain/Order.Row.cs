namespace FlowerShop.Domain;

public partial class Order
{
    public class Row
    {
        public string ProductCode { get; }
        public int Quantity { get; }

        public Row(int quantity, string productCode)
        {
            ProductCode = productCode;
            Quantity = quantity;
        }
    }
}