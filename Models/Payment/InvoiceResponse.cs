using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentService.Models.Entities;

namespace PaymentService.Models.Payment
{
    public interface IInvoiceResponse
    {
        public string this[string key] { get; }
    }

    public class URLInvoiceResponse : IInvoiceAction
    {
        
    }
}