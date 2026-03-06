using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcureDesk.Api.Contracts.Products;
using ProcureDesk.Application;

namespace ProcureDesk.Api.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly ProductApplicationService _productService;

    public ProductController(ProductApplicationService productService)
    {
        _productService = productService;
    }

    [HttpGet(Name = "GetAllProducts")]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
    public ActionResult GetAll()
    {
        var products = _productService.GetAll();
        return Ok(products);
    }

    [HttpGet("{code}", Name = "GetProductByCode")]
    public ActionResult Get(string code)
    {
        var product = _productService.Get(code);
        if (product is null)
            return NotFound(new { message = "Product not found." });

        return Ok(product);
    }

    [HttpPost(Name = "CreateProduct")]
    public ActionResult Create([FromBody] CreateProductRequest request)
    {
        var user = HttpContext.User?.Identity?.Name ?? "System";

        var (isCreated, errors, createdProduct) =
            _productService.Create(request.Code, request.Name, user);

        if (!isCreated)
            return BadRequest(new { errors });

        return CreatedAtAction(nameof(Get), new { code = createdProduct!.Code }, createdProduct);
    }

    [HttpPut("{code}", Name = "UpdateProductName")]
    public ActionResult UpdateName(string code, [FromBody] UpdateProductRequest request)
    {
        var user = HttpContext.User?.Identity?.Name ?? "System";

        var (isUpdated, errors) = _productService.UpdateName(code, request.Name, user);

        if (!isUpdated)
            return BadRequest(new { errors });

        return NoContent();
    }

    [HttpDelete("{code}", Name = "DeleteProduct")]
    public ActionResult Delete(string code)
    {
        var (isDeleted, errors) = _productService.Delete(code);

        if (!isDeleted)
            return NotFound(new { errors });

        return NoContent();
    }
}