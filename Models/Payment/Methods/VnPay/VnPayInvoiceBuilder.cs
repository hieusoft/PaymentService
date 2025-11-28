using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PaymentService.Models.Entities;

namespace PaymentService.Models.Payment.Handlers.VnPay
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



        public string ToInvoiceUrl()
        {
            // TODO implement checksum
            string Arguments = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?";
              = $"vnp_Version={WebUtility.UrlEncode(Version)}"
              + $"&vnp_Command={WebUtility.UrlEncode(Command)}"
              + $"&vnp_TmnCode={WebUtility.UrlEncode(MerchantId)}"
              + $"&vnp_Amount={WebUtility.UrlEncode((Invoice.TotalPrice * 100).ToString("F0"))}"
              + VnPayInvoiceBuilderUtils.GetPaymentMethodString(PaymentMethod)
              + VnPayInvoiceBuilderUtils.GetLocaleString(Locale)
              + $"&vnp_IpAddr={WebUtility.UrlEncode(IpAddress)}"
              + $"&vnp_TxnRef={WebUtility.UrlEncode(Invoice.Id.ToString())}"
              + $"&vnp_CreateDate={WebUtility.UrlEncode(Invoice.CreatedAt.ToString("yyyyMMddHHmmss"))}"
              + $"&vnp_ExpireDate={WebUtility.UrlEncode(Invoice.ExpiresAt.ToString("yyyyMMddHHmmss"))}"
              + $"&vnp_OrderType={WebUtility.UrlEncode(OrderType)}"
              + $"&vnp_OrderInfo={WebUtility.UrlEncode(TransactionInfo)}"
              + $"&vnp_ReturnUrl={WebUtility.UrlEncode(ReturnUrl)}"
            ;
        }
    }

    public static class VnPayInvoiceBuilderUtils
    {
        public static string GetPaymentMethodString(VnPayPaymentMethod paymentMethod) => paymentMethod switch
        {
            VnPayPaymentMethod.QrCode            => "&vnp_BankCode=VNPAYQR",
            VnPayPaymentMethod.DomesticCard      => "&vnp_BankCode=VNBANK",
            VnPayPaymentMethod.InternationalCard => "&vnp_BankCode=INTCARD",
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