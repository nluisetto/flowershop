namespace FlowerShop.Domain;

public interface IBundleRepository
{
    public Task<IEnumerable<Bundle>> GetBundlesFor(string productCode);
}