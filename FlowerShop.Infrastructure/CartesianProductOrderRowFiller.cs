using FlowerShop.Domain;

namespace FlowerShop.Infrastructure;

public class CartesianProductOrderRowFiller : IOrderRowFiller
{
    public IEnumerable<Tuple<Bundle, int>> Fill(Order.Row orderRow, IEnumerable<Bundle> availableBundles)
    {
        var minMaxCountRangeForEachBundle = availableBundles
            .Select((bundle) => new { value = bundle, minCount = 0, maxCount = orderRow.Quantity / bundle.Quantity })
            .Select((bundle) => 
                Enumerable
                    .Range(bundle.minCount, bundle.maxCount + (bundle.maxCount == 0 ? 0 : 1))
                    .Select((bundleCount) => new BundleState(bundle.value, bundleCount))
                    .ToList()
            )
            .ToList();
        
        var allPermutations = minMaxCountRangeForEachBundle
            .CartesianProduct()
            .ToList();

        var solution = allPermutations
            .Where(TotalQuantityMatch(orderRow.Quantity))
            .OrderBy(TotalBundleCount)
            .FirstOrDefault();

        if (solution == null)
            throw new NoFillingBundleConfigurationExistsException(orderRow, availableBundles);
                
        return solution
            .Select(bundleState => new Tuple<Bundle, int>(bundleState.Bundle, bundleState.BundleCount))
            .ToList();
    }

    private Func<IEnumerable<BundleState>, bool> TotalQuantityMatch(int targetQuantity)
    {
        return (bundleStates) => bundleStates.Sum(bundleState => bundleState.TotalQuantity) == targetQuantity;
    }

    private int TotalBundleCount(IEnumerable<BundleState> bundleStates)
    {
        return bundleStates.Sum(bundleState => bundleState.BundleCount);
    }

    private class BundleState
    {
        public Bundle Bundle { get; }
        public int BundleCount { get; }
        public int TotalQuantity => Bundle.Quantity * BundleCount;

        public BundleState(Bundle bundle, int bundleCount)
        {
            Bundle = bundle;
            BundleCount = bundleCount;
        }
    }
}

internal static class IEnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        var seed = Enumerable.Empty<T>().AsSingleton();
        
        return sequences
            .Aggregate(seed,
                (accumulator, sequence) =>
                {
                    return accumulator
                        .SelectMany(
                            _ => sequence,
                            (accumulatorSequence, item) => accumulatorSequence.Append(item)
                        )
                        .ToList();
                }
            )
            .ToList();
    }

    private static IEnumerable<T> AsSingleton<T>(this T item) => new[] { item };
}