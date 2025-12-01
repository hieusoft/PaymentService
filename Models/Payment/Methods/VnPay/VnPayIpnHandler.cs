using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace PaymentService.Models.Payment.Methods.VnPay
{
    public class VnPayIpnHandler
    {
        public readonly ulong InvoiceId;

        public readonly decimal Amount;

        public readonly VnPayResponseCode ResponseCode;

        public readonly VnPayTransactionStatus TransactionStatus;

        public VnPayIpnHandler(IQueryCollection queries, string hashSecret)
        {
            string targetHash = queries["vnp_SecureHash"]!;

            SortedDictionary<string, string> sortedQuery = new(queries.Where(q => q.Key.StartsWith("vnp_")).ToDictionary(q => q.Key, q => q.Value.ToString()));
            sortedQuery.Remove("vnp_SecureHash");
            string sortedString = string.Join('&', sortedQuery.Select(x => $"${x.Key}={WebUtility.UrlEncode(x.Value)}"));
            string actualHash = VnPayUtils.GetHmacSha512(hashSecret, sortedString);
            if (!actualHash.Equals(targetHash))
            {
                throw new ArgumentException("Checksum validation failed");
            }

            InvoiceId = ulong.Parse(queries["vnp_TxnRef"]!);
            Amount = decimal.Parse(queries["vnp_Amount"]!) / 100;
            ResponseCode = (VnPayResponseCode)int.Parse(queries["vnp_ResponseCode"]!);
            TransactionStatus = (VnPayTransactionStatus)int.Parse(queries["vnp_TransactionStatus"]!);
        }
    }

    public enum VnPayResponseCode
    {
        Success = 0,
    }

    public enum VnPayTransactionStatus
    {
        Success = 0,
        Blocked = 7,
        UserCancelled = 24,
        Unavailable = 75,
        UnknownError = 99,
    }
}