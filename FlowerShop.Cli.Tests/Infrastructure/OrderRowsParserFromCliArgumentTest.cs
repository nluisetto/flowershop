using System.Collections.Generic;
using System.Linq;
using FlowerShop.Application.UseCases.Common;
using FlowerShop.Cli.Infrastructure;
using Xunit;

namespace FlowerShop.Cli.Tests.Infrastructure;

public class OrderRowsParserFromCliArgumentTest
{
    // Valid row format is defined as: 'quantity product-code'
    // where quantity must be an integer and product-code a string
    [Theory]
    [InlineData("1 R12", "")]
    [InlineData("1 R12", "1x R12")]
    [InlineData("1 R12", "1 ")]
    [InlineData("1 R12", "1R12")]
    public void Should_Throw_OrderRowsParsingException_When_Any_Of_String_In_Given_Argument_Is_Malformed(params string[] orderRowsAsStrings)
    {
        var orderRowsParserFromCliArgument = new OrderRowsParserFromCliArgument();
        
        Assert.Throws<OrderRowsParsingException>(() => orderRowsParserFromCliArgument.Parse(orderRowsAsStrings));
    }
    
    [Fact]
    public void Should_Return_Parsed_Rows_When_All_Strings_In_Given_Argument_Are_Correct()
    {
        var orderRowsAsStrings = new[]
        {
            "1 R17",
            "5 L01",
            "17 S32"
        };
        
        var orderRowsParserFromCliArgument = new OrderRowsParserFromCliArgument();
        var parsedRows = orderRowsParserFromCliArgument.Parse(orderRowsAsStrings) as IList<OrderRowDto>;
        
        Assert.NotNull(parsedRows);
        Assert.Equal(3, parsedRows!.Count);

        var firstRow = parsedRows.First();
        Assert.Equal(1, firstRow.Quantity);
        Assert.Equal("R17", firstRow.ProductCode);
        
        var secondRow = parsedRows.Skip(1).First();
        Assert.Equal(5, secondRow.Quantity);
        Assert.Equal("L01", secondRow.ProductCode);
        
        var thirdRow = parsedRows.Skip(2).First();
        Assert.Equal(17, thirdRow.Quantity);
        Assert.Equal("S32", thirdRow.ProductCode);
    }
}