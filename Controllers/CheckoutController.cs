using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Database;
using PaymentService.Database.Repositories;
using PaymentService.Models.Entities;
using PaymentService.Models.Payment;

namespace PaymentService.Controllers;

[ApiController]
[Route("v1/checkout")]
public class CheckoutController : ControllerBase
{
    private readonly ILogger<CheckoutController> _logger;
    private readonly IPaymentMethodFactory _paymentFactory;
    private readonly AppDbContext _dbContext;

    public CheckoutController(ILogger<CheckoutController> logger, IPaymentMethodFactory paymentFactory, AppDbContext dbContext)
    {
        _logger = logger;
        _paymentFactory = paymentFactory;
        _dbContext = dbContext;
    }

    [HttpPost("/")]
    public IActionResult Checkout([FromBody] Invoice invoice)
    {
        IPaymentMethodHandler handler = _paymentFactory.GetHandler(invoice);

        var action = handler.InitiateInvoice(Request, invoice);

        switch (action)
        {
            case RedirectInvoiceAction redirectAction: 
                return Redirect(redirectAction.Destination);
            default: 
                throw new Exception("Unhandled invoice action");
        }
    }
}
