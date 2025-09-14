using ClamJam.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CleanJam.Application.Export
{
    public enum EnumExportFormat
    {
        Csv, Json, Text
    }
    public interface IExportService
    {
        EnumExportFormat Format { get;}
        /// <summary>
        /// Export the shopping cart to file
        /// </summary>
        /// <param name="cart">Cart to be exported</param>
        /// <returns>Export of the shopping cart</returns>
        FileContentResult ExportCart(ShoppingCart cart);
    }
}
