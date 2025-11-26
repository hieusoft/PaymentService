using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentService.Models.Entities;

namespace PaymentService.Models.Payment
{
    public interface IInvoiceAction
    {
        
    }

    public class RedirectInvoiceAction : IInvoiceAction
    {
        public string Destination { get; set; }

        public RedirectInvoiceAction(string destination)
        {
            Destination = destination;
        }
    }
}