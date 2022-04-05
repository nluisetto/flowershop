using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace FlowerShop.Domain.Tests;

public class OrderQuoteServiceTest
{
    [Fact]
    public void Should_Create_The_Quote_For_The_Given_Order()
    {
        var bundles = new List<Bundle>
        {
            new ("R11", 5, 7),
            new ("R11", 2, 2),
            new ("L06", 10, 14),
            new ("L06", 7, 14)
        } as IEnumerable<Bundle>;
        
        var bundleRepository = Mock.Of<IBundleRepository>((bundleRepositoryMock) => bundleRepositoryMock.GetBundlesFor(It.IsAny<string>()) == Task.FromResult(new List<Bundle> {} as IEnumerable<Bundle>));
        
        // var orderQuoteService = new OrderQuoteService(bundleRepository);
    }
}