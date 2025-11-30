using PaymentService.Models.Entities;
using PaymentService.Models.Payment.Methods.VnPay;

namespace PaymentService.Models.Payment
{
    public interface IPaymentMethodFactory
    {
        public IPaymentMethodHandler GetHandler(Invoice invoice);
    }

    public class PaymentMethodFactory : IPaymentMethodFactory
    {
        readonly IConfiguration _configuration; 

        public PaymentMethodFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IPaymentMethodHandler GetHandler(Invoice invoice)
        {
            return invoice.PaymentMethod switch
            {
                PaymentMethod.VnPay => new VnPayPaymentMethodHandler(
                        _configuration["VnPay:MerchantId"]!,
                        _configuration["VnPay:HashSecret"]!,
                        _configuration["VnPay:ReturnUrl"]!
                    ),
                _ => throw new ArgumentException("Invalid invoice"),
            };
        }
    }
}