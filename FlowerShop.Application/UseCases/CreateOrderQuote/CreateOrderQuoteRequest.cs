using FlowerShop.Application.UseCases.Common;
using MediatR;

namespace FlowerShop.Application.UseCases.CreateOrderQuote;

public class CreateOrderQuoteRequest : IRequest<CreateOrderQuoteResponse>
{
    public IEnumerable<OrderRowDto> OrderRows { get; init; }
}