using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlowerShop.Application.UseCases.Common;
using FlowerShop.Application.UseCases.CreateOrderQuote;
using FlowerShop.Domain;
using Moq;
using Xunit;

namespace FlowerShop.Application.Tests;

public class CreateOrderQuoteHandlerTest
{
    [Fact]
    public async void Should_Propagate_Exceptions_Thrown_By_OrderQuoteService_CreateQuoteFor()
    {
        var expectedException = new Exception();

        var orderQuoteService = Mock.Of<IOrderQuoteService>();
        Mock.Get(orderQuoteService)
            .Setup((orderQuoteServiceMock) => orderQuoteServiceMock.CreateQuoteFor(It.IsAny<Order>()))
            .Throws(expectedException);

        var createOrderQuoteHandler = new CreateOrderQuoteHandler(orderQuoteService);
        
        var createOrderQuoteRequest = GetFakeCreateOrderQuoteRequest();
        var thrownException = await Assert.ThrowsAsync<Exception>(() => createOrderQuoteHandler.Handle(createOrderQuoteRequest, default ));
        
        Assert.True(expectedException == thrownException);
    }

    [Fact]
    public async void Should_Return_Created_OrderQuote_As_Dto()
    {
        var fakeQuote = GetFakeOrderQuote();
        var orderQuoteService = Mock.Of<IOrderQuoteService>((orderQuoteServiceMock) => orderQuoteServiceMock.CreateQuoteFor(It.IsAny<Order>()) == Task.FromResult(fakeQuote));

        var createOrderQuoteHandler = new CreateOrderQuoteHandler(orderQuoteService);
        
        var createOrderQuoteRequest = GetFakeCreateOrderQuoteRequest();
        var response = await createOrderQuoteHandler.Handle(createOrderQuoteRequest, default);
        
        Assert.NotNull(response);

        var quoteRowDtos = response.QuoteRows as IList<QuoteRowDto>;
        Assert.NotNull(quoteRowDtos);

        Assert.Equal(1, quoteRowDtos!.Count);
        
        var firstRowDto = quoteRowDtos.First();
        Assert.Equal(8, firstRowDto.OrderRowDto.Quantity);
        Assert.Equal("R22", firstRowDto.OrderRowDto.ProductCode);
        Assert.Equal(2, firstRowDto.BundleDetails.Count());
        Assert.Equal(14, firstRowDto.TotalPrice);

        var firstBundleDetailDto = firstRowDto.BundleDetails.First();
        Assert.Equal(2, firstBundleDetailDto.Count);
        Assert.Equal(10, firstBundleDetailDto.TotalPrice);
        Assert.Equal(3, firstBundleDetailDto.Bundle.Quantity);
        Assert.Equal(5, firstBundleDetailDto.Bundle.Price);

        var secondBundleDetailDto = firstRowDto.BundleDetails.Skip(1).First();
        Assert.Equal(1, secondBundleDetailDto.Count);
        Assert.Equal(4, secondBundleDetailDto.TotalPrice);
        Assert.Equal(2, secondBundleDetailDto.Bundle.Quantity);
        Assert.Equal(4, secondBundleDetailDto.Bundle.Price);
    }

    private CreateOrderQuoteRequest GetFakeCreateOrderQuoteRequest()
    {
        return new CreateOrderQuoteRequest { OrderRows = new[] { new OrderRowDto(8, "R22") } };
    }

    private OrderQuote GetFakeOrderQuote()
    {
        var quote = new OrderQuote();
        var orderRow = new Order.Row(8, "R22");
        
        var bundle1 = new Bundle("R22", 3, 5);
        quote.AddBundle(orderRow, 2, bundle1);
        
        var bundle2 = new Bundle("R22", 2, 4);
        quote.AddBundle(orderRow, 1, bundle2);

        return quote;
    }
}