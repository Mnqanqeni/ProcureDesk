using Microsoft.AspNetCore.Mvc;
using ProcureDesk.Application;
using ProcureDesk.Domain;

namespace ProcureDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductApplicationService _productService;

    public ProductController(ProductApplicationService productService)
    {
        _productService = productService;
    }

    [HttpGet(Name = "GetAllProducts")]
    public IActionResult GetAll()
    {
        var products = _productService.GetAll();
        return Ok(products);
    }

    [HttpGet("{code}", Name = "GetProductByCode")]
    public IActionResult Get(string code)
    {
        var product = _productService.Get(code);
        if (product is null)
            return NotFound(new { message = "Product not found." });

        return Ok(product);
    }

    [HttpPost(Name = "CreateProduct")]
    public IActionResult Create([FromBody] string code, [FromBody] string name)
    {
        var user = HttpContext.User?.Identity?.Name ?? "System";
        var (isCreated, errors, product) = _productService.Create(code, name, user);

        if (!isCreated)
            return BadRequest(new { errors });

        return CreatedAtAction("GetProductByCode", new { code }, product);
    }

    [HttpPut("{code}", Name = "UpdateProductName")]
    public IActionResult UpdateName(string code, [FromBody] string newName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = HttpContext.User?.Identity?.Name ?? "System";
        var (isUpdated, errors) = _productService.UpdateName(code, newName, user);

        if (!isUpdated)
            return BadRequest(new { errors });

        return NoContent();
    }

    [HttpDelete("{code}", Name = "DeleteProduct")]
    public IActionResult Delete(string code)
    {
        var (isDeleted, errors) = _productService.Delete(code);

        if (!isDeleted)
            return NotFound(new { errors });

        return NoContent();
    }
}