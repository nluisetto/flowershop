namespace FlowerShop.Domain;

public interface IOrderRowFiller
{
    public IEnumerable<Tuple<Bundle, int>> Fill(Order.Row orderRow, IEnumerable<Bundle> availableBundles);
}