using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Models.Entities
{
    public interface IEntity 
    {
        public ulong Id { get; set; }
    }
}