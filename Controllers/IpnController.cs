using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Database;
using PaymentService.Database.Repositories;
using PaymentService.Models.Entities;
using PaymentService.Models.Payment;

namespace PaymentService.Controllers;

[ApiController]
[Route("v1/ipn")]
public class IpnController : ControllerBase
{
    private readonly ILogger<IpnController> _logger;
    private readonly IPaymentMethodFactory _paymentFactory;
    private readonly AppDbContext _dbContext;

    public IpnController(ILogger<IpnController> logger, IPaymentMethodFactory paymentFactory, AppDbContext dbContext)
    {
        _logger = logger;
        _paymentFactory = paymentFactory;
        _dbContext = dbContext;
    }

    [HttpGet("/vnpay/")]
    public IActionResult VnPayIpn()
    {
        new VnPayIpnHandler(Request.Query);
    }
}
