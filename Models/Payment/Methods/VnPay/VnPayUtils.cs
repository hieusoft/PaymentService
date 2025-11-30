using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Models.Payment.Methods.VnPay
{
    public static class VnPayUtils
    {
        public static string GetHmacSha512(string key, string source)
        {
            var hash = new StringBuilder(); 
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(sourceBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
    }
}