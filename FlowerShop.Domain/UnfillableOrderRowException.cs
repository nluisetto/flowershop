namespace FlowerShop.Domain;

public class NoFillingBundleConfigurationExistsException : Exception
{
    public Order.Row OrderRow { get; }
    public IEnumerable<Bundle> AvailableBundles { get; }

    public NoFillingBundleConfigurationExistsException(Order.Row orderRow, IEnumerable<Bundle> availableBundles)
        : base("No combination of bundles exists that can fill the order row")
    {
        OrderRow = orderRow;
        AvailableBundles = availableBundles;
    }
}