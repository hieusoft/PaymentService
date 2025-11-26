using PaymentService.Database.Repositories;
using PaymentService.Models.Entities;

namespace PaymentService.Models.Payment
{
    public interface IPaymentMethodHandler
    {
        public IInvoiceAction InitiateInvoice(Invoice invoice);
        public IInvoiceAction ProcessInvoiceResponse(IInvoiceResponse response, IRepository<Invoice> invoiceRepository);
    }
}