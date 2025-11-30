using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using PaymentService.Models.Entities;

namespace PaymentService.Models.Payment.Methods.VnPay
{
    // https://sandbox.vnpayment.vn/apis/docs/thanh-toan-pay/pay.html
    public class VnPayInvoiceBuilder
    {

        public readonly string Version = "2.1.0";

        public readonly string Command = "pay";

        public required string MerchantId { get; set; }

        public readonly string OrderType = "other";

        public VnPayPaymentMethod PaymentMethod { get; set; } = VnPayPaymentMethod.Auto;

        public required Invoice Invoice { get; set; }

        public string Currency { get; set; } = CurrencyCodes.VND;

        public required string IpAddress { get; set; }

        public required string TransactionInfo { get; set; }

        public required string ReturnUrl { get; set; }

        public VnPayLocale Locale { get; set; } = VnPayLocale.English;



        public string ToInvoiceUrl(string hashSecret)
        {
            SortedDictionary<string, string> queryParams = new()
            {
                { "vnp_Version", Version },
                { "vnp_Command", Command },
                { "vnp_TmnCode", MerchantId },
                { "vnp_Amount", (Invoice.TotalPrice * 100).ToString("F0") },
                { "vnp_Locale", VnPayInvoiceBuilderUtils.GetLocaleString(Locale) },
                { "vnp_IpAddr", IpAddress },
                { "vnp_TxnRef", Invoice.Id.ToString() },
                { "vnp_CreateDate", Invoice.CreatedAt.ToString("yyyyMMddHHmmss") },
                { "vnp_ExpireDate", Invoice.ExpiresAt.ToString("yyyyMMddHHmmss") },
                { "vnp_OrderType", OrderType },
                { "vnp_OrderInfo", TransactionInfo },
                { "vnp_ReturnUrl", ReturnUrl }
            };
            if (PaymentMethod != VnPayPaymentMethod.Auto) 
            {
                queryParams.Add("vnp_BankCode", VnPayInvoiceBuilderUtils.GetPaymentMethodString(PaymentMethod));
            }

            string queryString = string.Join('&', queryParams.Select(p => $"{p.Key}={WebUtility.UrlEncode(p.Value)}").ToArray());
            string hash = VnPayUtils.GetHmacSha512(hashSecret, queryString);
            
            return $"https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?{queryString}&vnp_SecureHash={hash}";
        }
    }

    public static class VnPayInvoiceBuilderUtils
    {
        public static string GetPaymentMethodString(VnPayPaymentMethod paymentMethod) => paymentMethod switch
        {
            VnPayPaymentMethod.QrCode            => "VNPAYQR",
            VnPayPaymentMethod.DomesticCard      => "VNBANK",
            VnPayPaymentMethod.InternationalCard => "INTCARD",
            _                                    => "",  
        };

        public static string GetLocaleString(VnPayLocale locale) => locale switch
        {
            VnPayLocale.English    => "&vnp_Locale=vn",
            VnPayLocale.Vietnamese => "&vnp_Locale=en",
            _                      => throw new ArgumentException("Not a valid locale"),  
        };
    }

    public enum VnPayPaymentMethod
    {
        Auto,
        QrCode,
        DomesticCard,
        InternationalCard
    }

    public enum VnPayLocale
    {
        English,
        Vietnamese
    }
}