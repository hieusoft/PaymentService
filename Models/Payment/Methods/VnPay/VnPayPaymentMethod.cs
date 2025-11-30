using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentService.Database.Repositories;
using PaymentService.Models.Entities;

namespace PaymentService.Models.Payment.Methods.VnPay
{
    public class VnPayPaymentMethodHandler : PaymentMethodHandler
    {
        string MerchantId;
        string HashSecret;
        string ReturnUrl;

        public VnPayPaymentMethodHandler(string merchantId, string hashSecret, string returnUrl)
        {
            MerchantId = merchantId;
            HashSecret = hashSecret;
            ReturnUrl = returnUrl;
        }

        public override IInvoiceAction InitiateInvoice(HttpRequest request, Invoice invoice)
        {
            base.InitiateInvoice(request, invoice);

            VnPayInvoiceBuilder builder = new ()
            {
                MerchantId = MerchantId,
                Invoice = invoice,
                IpAddress = request.HttpContext.Connection.RemoteIpAddress!.ToString(),
                TransactionInfo = $"java Floral checkout ${invoice.TotalPrice} ${invoice.Currency}",
                ReturnUrl = ReturnUrl,
            };

            return new RedirectInvoiceAction(builder.ToInvoiceUrl(HashSecret));
            
        }

        public IInvoiceAction ProcessInvoiceResponse(HttpRequest request, IInvoiceResponse response, IRepository<Invoice> invoiceRepository)
        {
            throw new NotImplementedException();
        }
    }
}