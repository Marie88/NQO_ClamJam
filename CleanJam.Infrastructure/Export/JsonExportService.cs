using ClamJam.Domain.Entities;
using CleanJam.Application.Export;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace CleanJam.Infrastructure.Export
{
    public class JsonExportService : IExportService
    {
        public EnumExportFormat Format => EnumExportFormat.Json;
        public FileContentResult ExportCart(ShoppingCart cart)
        {
            string json = JsonSerializer.Serialize(cart, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            byte[] bytes = Encoding.UTF8.GetBytes(json);

            return new FileContentResult(bytes, "application/json")
            {
                FileDownloadName = "cart.json"
            };
        }
    }
}
