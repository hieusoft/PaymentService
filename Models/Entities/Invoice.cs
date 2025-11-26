using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PaymentService.Models.Payment;

namespace PaymentService.Models.Entities
{
    public class Invoice : IEntity
    {
        public ulong UserId { get; set; }

        public ulong Id { get; set; }

        public InvoiceStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public decimal TotalPrice { get; set; }

        [Length(3, 3)]
        public string Currency { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public List<InvoiceItem> Items { get; set; }



        public decimal ComputedTotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items) total += item.UnitPrice * item.Quantity;
                return total;
            }
        }

    }

    public class InvoiceItem : IEntity
    {
        public ulong Id { get; set; }

        public ulong InvoiceId { get; set; }

        public ulong ItemId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity;
    }

    public enum InvoiceStatus
    {
        Ready,
        Ongoing,
        Completed,
        UserCancelled,
        Invalid,
    }
}