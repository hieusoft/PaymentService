using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentService.Models.Entities;

namespace PaymentService.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        
        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options) {}
    }
}