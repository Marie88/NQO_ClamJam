using ClamJam.Domain.Entities;
using CleanJam.Application.Export;
using CleanJam.Application.Tax;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CleanJam.Infrastructure.Export
{
    public class CSVExportService : IExportService
    {
        private readonly ITaxService _taxService;
        public CSVExportService(ITaxService taxService)
        {
            _taxService = taxService;
        }

        public EnumExportFormat Format => EnumExportFormat.Csv;
        public FileContentResult ExportCart(ShoppingCart cart)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Name,Price/Piece,Quantity,VAT Type,VAT percentage,Total");

            foreach (var item in cart.Items)
               
            {   var taxStrategyForItem = _taxService.GetVatTypeForItem(item);
                sb.AppendLine($"{item.Product.Name},{item.Product.Price.Amount},{item.Quantity},{taxStrategyForItem.VatType},{taxStrategyForItem.Rate.Value},{item.LineTotal.Amount}");
            }

            sb.AppendLine($"TOTAL,{cart.Subtotal.Amount}");

            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());

            return new FileContentResult(bytes, "text/csv")
            {
                FileDownloadName = "cart.csv"
            };
        }
    }
}
