using ClamJam.Domain.DomainModels;
using ClamJam.Domain.Entities;
using CleanJam.Application.Tax;
using CleanJam.Infrastructure.Export;
using System.Text;

namespace ClamJam.Tests;

public class ExportServicesTests
{
    private static ShoppingCart SampleCart()
    {
        var product = new Product("Apple", new Money(10, EnumCurrency.USD), "d", EnumProductType.Food);
        var item = new CartItem(product, 1) { DiscountedLineTotal = new Money(10, EnumCurrency.USD) };
        return TestHelpers.CreateCart(EnumCurrency.USD, item);
    }

    private class StubTaxService : ITaxService
    {
        private readonly ITaxStrategy _strategy = new StandardVatStrategy(5) { VatType = EnumVatType.Food };
        public decimal ComputeTax(IReadOnlyCollection<CartItem> cartItems) => 0;
        public ITaxStrategy GetVatTypeForItem(CartItem cartItem) => _strategy;
    }

 [Fact]
    public void CsvExportService_ProducesCsv()
    {
    var service = new CSVExportService(new StubTaxService());
    var result = service.ExportCart(SampleCart());
    var text = Encoding.UTF8.GetString(result.FileContents);
    
    Assert.Equal("text/csv", result.ContentType);
    Assert.Equal("cart.csv", result.FileDownloadName);
    Assert.Contains("Name,Price/Piece,Quantity", text);
    Assert.Contains("Apple", text);
    Assert.Contains("TOTAL", text);
        }

 [Fact]
    public void JsonExportService_ProducesJson()
    {
    var service = new JsonExportService();
    var result = service.ExportCart(SampleCart());
    var text = Encoding.UTF8.GetString(result.FileContents);
    
    Assert.Equal("application/json", result.ContentType);
    Assert.Equal("cart.json", result.FileDownloadName);
    Assert.Contains("Apple", text);
        }

 [Fact]
    public void TextExportService_ProducesText()
    {
    var service = new TextExportService();
    var result = service.ExportCart(SampleCart());
    var text = Encoding.UTF8.GetString(result.FileContents);
    
    Assert.Equal("text/plain", result.ContentType);
    Assert.Equal("cart.txt", result.FileDownloadName);
    Assert.Contains("Apple", text);
        }
}

