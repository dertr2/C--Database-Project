using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Model;
using Microsoft.Data.SqlClient;

namespace InventoryManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
        // this basically assigns a varialble to type IConfiguration
        private readonly IConfiguration _config;

    // this gives the settings in .json (where the database is)
    public ProductController(IConfiguration config) 
    {
        _config = config;
    }

    [HttpGet]
    //gets a http response where in this case, getting the database connection called "DefaultConnection"
    public async Task<IActionResult> Get()
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        // to make sure that we know theres a problem if we're unable to fetch it
        if (string.IsNullOrWhiteSpace(connStr))
            return Problem("Missing connection string: DefaultConnection");

        // executes these set of statements to retrieve the database.
        const string sql = @"
        SELECT p.Id, p.Name, p.Price, p.Quantity, p.InStock, p.CategoryId, p.CreatedDate,
        c.Id AS Category_Id, c.Name AS Category_Name
        FROM Products p
        JOIN Categories c ON p.CategoryId = c.Id 
        ORDER BY p.Id;"; 

        var results = new List<Product>();

        await using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var product = new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                InStock = reader.GetBoolean(reader.GetOrdinal("InStock")),
                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                Category = new Category
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Category_Id")),
                    Name = reader.GetString(reader.GetOrdinal("Category_Name"))
                }
            };

            results.Add(product);
        }

        return Ok(results);
    }
}


