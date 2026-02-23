using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Model;

namespace InventoryManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
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
    }
}
}
