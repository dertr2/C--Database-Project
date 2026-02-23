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
        var products = new List<Product>
        {
            new Product
            {
              Id = 1,  
              Name = "Apples",
              Price = 4.50m,
              Quantity = 46,
              InStock =  true,
              CategoryId = 1,
              Category = new Category
              {
                  Id = 1,
                  Name = "Fruits"
              }
            }
        };

        return Ok(products);
    }
}