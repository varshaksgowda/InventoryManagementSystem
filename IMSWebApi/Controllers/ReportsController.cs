﻿using IMSWebApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IMSWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly InventoryDBaseContext _context;

        // Injecting the DbContext via constructor for dependency injection
        public ReportsController(InventoryDBaseContext context)
        {
            _context = context;
        }

        // GET api/reports/inventoryreport
        [HttpGet("inventoryreport")]
        public async Task<ActionResult> GetReport()
        {
            try
            {
                // LINQ query to get SupplierId, Supplier Name, ProductId, StockLevel, ReorderLevel, and Quantity from related tables
                var reportData = await (from supplier in _context.Suppliers
                                        join product in _context.Products on supplier.SupplierId equals product.SupplierId
                                        join order in _context.Orders on product.ProductId equals order.ProductId
                                        select new
                                        {
                                            supplier.SupplierId,
                                            SupplierName = supplier.Name,
                                            product.ProductId,
                                            StockLevel = product.StockLevel,
                                            ReorderLevel = product.ReorderLevel,
                                            Quantity = order.Quantity
                                        }).ToListAsync();

                // If no data is returned, return a specific message
                if (reportData == null || reportData.Count == 0)
                {
                    return NotFound("No report data available.");
                }

                // Return the result as JSON
                return Ok(reportData);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                // In a production environment, you could use a logging framework like Serilog or NLog
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
