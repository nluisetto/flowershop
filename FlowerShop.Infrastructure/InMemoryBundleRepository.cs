using FlowerShop.Domain;

namespace FlowerShop.Infrastructure;

public class InMemoryBundleRepository : IBundleRepository
{
    private Dictionary<string, HashSet<Bundle>> _bundles;
    
    public InMemoryBundleRepository(ISet<Bundle> bundles)
    {
        _bundles = bundles.GroupBy((bundle) => bundle.ProductCode)
            .ToDictionary((group) => group.Key, (group) => group.ToHashSet());
    }
    
    public Task<IEnumerable<Bundle>> GetBundlesFor(string productCode)
    {
        var result = _bundles.ContainsKey(productCode)
            ? _bundles[productCode].ToList()
            : new List<Bundle>();

        return Task.FromResult(result as IEnumerable<Bundle>);
    }
}