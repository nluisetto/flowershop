using FlowerShop.Application.UseCases.Common;
using FlowerShop.Domain;
using MediatR;

namespace FlowerShop.Application.UseCases.CreateOrderQuote;

public class CreateOrderQuoteHandler : IRequestHandler<CreateOrderQuoteRequest, CreateOrderQuoteResponse>
{
    private readonly Domain.IOrderQuoteService _orderQuoteService;

    public CreateOrderQuoteHandler(Domain.IOrderQuoteService orderQuoteService)
    {
        _orderQuoteService = orderQuoteService;
    }

    public async Task<CreateOrderQuoteResponse> Handle(CreateOrderQuoteRequest request, CancellationToken cancellationToken)
    {
        var order = GetOrder(request);

        var quote = await _orderQuoteService.CreateQuoteFor(order);

        return MapToResponse(quote);
    }

    
    
    private Order GetOrder(CreateOrderQuoteRequest request)
    {
        var orderRows = request.OrderRows;
        
        var order = new Order();
        foreach (var orderRow in orderRows)
        {
            order.AddRow(orderRow.Quantity, orderRow.ProductCode);
        }

        return order;
    }

    private CreateOrderQuoteResponse MapToResponse(OrderQuote orderQuote)
    {
        var quoteRowDtos = orderQuote
            .Rows
            .Select(MapToDto)
            .ToList();

        return new CreateOrderQuoteResponse(quoteRowDtos);
    }

    private QuoteRowDto MapToDto(OrderQuote.Row row)
    {
        var orderRowDto = new OrderRowDto(row.OrderRow.Quantity, row.OrderRow.ProductCode);
        var bundlesDetailsDto = row
            .BundleDetails
            .Select(bundleDetail =>
            {
                var bundleDto = new BundleDto(bundleDetail.Bundle.Quantity, bundleDetail.Bundle.Price);
                return new QuoteRowDto.BundleDetailDto(bundleDetail.Count, bundleDto, bundleDetail.TotalPrice);
            })
            .ToList();
                
        return new QuoteRowDto(orderRowDto, bundlesDetailsDto, row.TotalPrice);
    }
}