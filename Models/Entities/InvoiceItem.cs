using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PaymentService.Models.Payment;

namespace PaymentService.Models.Entities
{
    public class InvoiceItem : IEntity
    {
        public ulong Id { get; set; }
 
        public ulong InvoiceId { get; set; }

        public ulong ItemId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity;
    }
}