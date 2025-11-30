using Microsoft.AspNetCore.Http.HttpResults;
using PaymentService.Database.Repositories;
using PaymentService.Models.Entities;

namespace PaymentService.Models.Payment
{
    public interface IPaymentMethodHandler
    {
        public IInvoiceAction InitiateInvoice(HttpRequest request, Invoice invoice);
        public IInvoiceAction ProcessInvoiceResponse(HttpRequest request, IInvoiceResponse response, IRepository<Invoice> invoiceRepository);
    }

    public abstract class PaymentMethodHandler : IPaymentMethodHandler
    {
        public virtual IInvoiceAction InitiateInvoice(HttpRequest request, Invoice invoice)
        {
            if (invoice.Status != InvoiceStatus.Ready) 
            {
                throw new ArgumentException("Invoice is already initiated or completed");
            }
            invoice.Status = InvoiceStatus.Ongoing;
            return null;
        }

        public virtual IInvoiceAction ProcessInvoiceResponse(HttpRequest request, IInvoiceResponse response, IRepository<Invoice> invoiceRepository)
        {
            throw new NotImplementedException();
        }
    }
}