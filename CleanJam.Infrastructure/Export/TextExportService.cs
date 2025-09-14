using ClamJam.Domain.Entities;
using CleanJam.Application.Export;
using Microsoft.AspNetCore.Mvc;
using System.Text;


namespace CleanJam.Infrastructure.Export
{
    public class TextExportService : IExportService
    {
        public EnumExportFormat Format => EnumExportFormat.Text;
        public FileContentResult ExportCart(ShoppingCart cart)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Shopping Cart Summary ===");

            foreach (var item in cart.Items)
            {
                sb.AppendLine($"{item.Product.Name},Price/Unit: {item.Product.Price.Amount}, Quantity: {item.Quantity}, TotalPerLine: {item.LineTotal.Amount}");
            }

            sb.AppendLine("-----------------------------");
            sb.AppendLine($"Grand Total: {cart.Subtotal.Amount}");

            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());

            return new FileContentResult(bytes, "text/plain")
            {
                FileDownloadName = "cart.txt"
            };
        }
    }
}
