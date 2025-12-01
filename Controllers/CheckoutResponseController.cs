using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Database;
using PaymentService.Database.Repositories;
using PaymentService.Models.Entities;
using PaymentService.Models.Payment;
using PaymentService.Models.Payment.Methods.VnPay;

namespace PaymentService.Controllers;

[ApiController]
[Route("v1/checkout_response")]
public class CheckoutResponseController : ControllerBase
{
    private readonly ILogger<CheckoutResponseController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public CheckoutResponseController(ILogger<CheckoutResponseController> logger, IConfiguration configuration, AppDbContext dbContext)
    {
        _logger = logger;
        _configuration = configuration;
        _dbContext = dbContext;
    }

    [HttpGet("/vnpay/")]
    public IActionResult VnPayResponse()
    {
        VnPayIpnHandler handler;
        try 
        { 
            handler = new VnPayIpnHandler(Request.Query, _configuration["VnPay:HashSecret"]!);
        } 
        catch (ArgumentException)
        {
            return BadRequest(new { Message = "Validation failed" });
        }

        return Redirect($"localhost:7049/checkout/completed/?InvoiceId={handler.InvoiceId}");
    }
}
