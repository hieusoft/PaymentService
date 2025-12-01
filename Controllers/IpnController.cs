using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Database;
using PaymentService.Database.Repositories;
using PaymentService.Models.Entities;
using PaymentService.Models.Payment;
using PaymentService.Models.Payment.Methods.VnPay;

namespace PaymentService.Controllers;

[ApiController]
[Route("v1/ipn")]
public class IpnController : ControllerBase
{
    private readonly ILogger<IpnController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IConfiguration _configuration;

    public IpnController(ILogger<IpnController> logger, IRepository<Invoice> invoiceRepository, IConfiguration configuration, AppDbContext dbContext)
    {
        _logger = logger;
        _configuration = configuration;
        _invoiceRepository = invoiceRepository;
        _dbContext = dbContext;
    }
    
    [HttpGet("/vnpay/")]
    public async Task<IActionResult> VnPayIpn()
    {
        VnPayIpnHandler handler;
        try 
        { 
            handler = new VnPayIpnHandler(Request.Query, _configuration["VnPay:HashSecret"]!);
        } 
        catch (ArgumentException)
        {
            return BadRequest(new { RspCode = "97", Message = "Validation failed" });
        }
        
        Invoice? invoice = await _invoiceRepository.GetOneAsync(handler.InvoiceId);
        if (invoice == null)
        {
            return BadRequest(new { RspCode = "01", Message = "Invoice not found" });
        }
        if (invoice.TotalPrice != handler.Amount)
        {
            return BadRequest(new { RspCode = "04", Message = "Amount mismatch" });
        }
        if (invoice.Status != InvoiceStatus.Ongoing)
        {
            return BadRequest(new { RspCode = "02", Message = "Invoice alrady handled" });
        }
        
        bool isSuccess = handler.ResponseCode == VnPayResponseCode.Success && handler.TransactionStatus == VnPayTransactionStatus.Success;
        bool isUserCancelled = handler.TransactionStatus == VnPayTransactionStatus.UserCancelled;
        invoice.Status = isSuccess ? InvoiceStatus.Completed : isUserCancelled ? InvoiceStatus.UserCancelled : InvoiceStatus.Invalid;
        await _invoiceRepository.UpdateAsync(invoice);
        await _invoiceRepository.SaveAsync();

        return Ok(new { RspCode = "00", Message = "Success" });
    }
}
