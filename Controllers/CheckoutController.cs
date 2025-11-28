using Microsoft.AspNetCore.Mvc;
using PaymentService.Models.Entities;

namespace PaymentService.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ILogger<CheckoutController> _logger;

    public CheckoutController(ILogger<CheckoutController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "Checkout")]
    public IActionResult Checkout([FromBody] Invoice invoice)
    {
        
    }
}
