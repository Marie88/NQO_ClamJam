using ClamJam.Domain.Entities;
using CleanJam.Application.Export;
using Microsoft.AspNetCore.Mvc;

namespace ClamJam.Tests;

public class ExportServiceFactoryTests
{
    private class FakeExportService : IExportService
    {
        public EnumExportFormat Format { get; }
        public FakeExportService(EnumExportFormat format) => Format = format;
        public FileContentResult ExportCart(ShoppingCart cart) => throw new NotImplementedException();
    }

    [Fact]
    public void GetExporter_ReturnsCorrectService()
    {
        var csv = new FakeExportService(EnumExportFormat.Csv);
        var json = new FakeExportService(EnumExportFormat.Json);
        var factory = new ExportServiceFactory(new[] { csv, json });

        var result = factory.GetExporter(EnumExportFormat.Json);

        Assert.Same(json, result);
    }

    [Fact]
    public void GetExporter_Unsupported_Throws()
    {
        var factory = new ExportServiceFactory(new List<IExportService>());
        Assert.Throws<ArgumentException>(() => factory.GetExporter(EnumExportFormat.Text));
    }
}

