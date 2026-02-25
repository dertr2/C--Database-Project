using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Model;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Dynamic;

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
    public async Task<IActionResult> GetAllData()
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        // to make sure that we know theres a problem if we're unable to fetch it
        if (string.IsNullOrWhiteSpace(connStr))
            return Problem("Missing connection string: DefaultConnection");

        // executes these set of statements to retrieve the database.
        const string sql = @"
        SELECT p.Id, p.Name, p.Price, p.Quantity, p.InStock, p.CategoryId, p.CreatedDate,
        c.Id AS Category_Id, c.Category AS Category_Name
        FROM Products p
        JOIN Categories c ON p.CategoryId = c.Id 
        ORDER BY p.Id;"; 

        var results = new List<Product>();

        await using var conn = new SqlConnection(connStr); // instantiates connection to sql database
        await conn.OpenAsync(); // this authenticates the connection and "starts"

        await using var cmd = new SqlCommand(sql, conn); // instantiates sql commmands to execute inside the sql server
        await using var reader = await cmd.ExecuteReaderAsync(); // reads the first row returned from SQLt

        while (await reader.ReadAsync()) // returns entire query for us to add a product
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
                    CategoryId = reader.GetInt32(reader.GetOrdinal("Category_Id")),
                    CategoryName = reader.GetString(reader.GetOrdinal("Category_Name"))
                }
            };

            results.Add(product);
        }

        return Ok(results);

        
    }
    
[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id)
{
    if (id <= 0)
        return BadRequest("Invalid id number, must be greater than 0");

    var connStr = _config.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(connStr))
        return Problem("Missing connection string: DefaultConnection");

    const string sql = @"
        SELECT p.Id, p.Name, p.Price, p.Quantity, p.InStock, p.CategoryId, p.CreatedDate,
               c.Id AS Category_Id, c.Category AS Category_Name
        FROM Products p
        JOIN Categories c ON p.CategoryId = c.Id
        WHERE p.Id = @Id;";

    await using var conn = new SqlConnection(connStr);
    await conn.OpenAsync();

    await using var cmd = new SqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@Id", id);

    await using var reader = await cmd.ExecuteReaderAsync();

    if (!await reader.ReadAsync())
        return NotFound($"Product with Id {id} not found.");

    var product = new
    {
        Id = reader.GetInt32(reader.GetOrdinal("Id")),
        Name = reader.GetString(reader.GetOrdinal("Name")),
        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
        InStock = reader.GetBoolean(reader.GetOrdinal("InStock")),
        Category = new
        {
            Id = reader.GetInt32(reader.GetOrdinal("Category_Id")),
            Name = reader.GetString(reader.GetOrdinal("Category_Name"))
        }
    };

    return Ok(product);
}
    // Requires: valid string
    // Effects: Returns product, otherwise return error
    [HttpGet("by-name/{name}")]
    public async Task<IActionResult> GetByName(String name)
    {
        // TODO
        return null;
    }
    // Requires: not invalid product
    // Modifies: changes products table and adds a new one
    // Effects: New row appears, otherwise return error including duplicates
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        // TODO
        return null;
    }

    // Requires: id > 0
    // Modifies: changes the product table
    // Effects: Updates a product in table, potentially avoid duplicates?
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateById(int id, Product product)
    {
        // TODO
        return null;
    }

    // Requires: valid string
    // Modifies: changes the product table
    // Effects: Updates a product in the table, potentially avoid duplicates?
    [HttpPut("{name}")]
    public async Task<IActionResult> UpdateByName(string name, Product product)
    {
        // TODO
        return null;
    }

    // Requires: id > 0
    // Modifies: changes the product table
    // Effects: Removes a product in the table unless already removed
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        // TODO
        return null;
    }

    // Requires: valid string
    // Modifies: changes the product table
    // Effects: Removes a product in the table unless already removed
    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteByName(string name)
    {
        // TODO
        return null;
    }
}
    



