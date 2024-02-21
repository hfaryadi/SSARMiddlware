using SSARMiddlware.Services.Base;
using SSARMiddlware.ViewModels.Payment;

namespace SSARMiddlware.Services
{
    internal class PaymentService : BaseService<PaymentRequestViewModel, PaymentResponseViewModel>
    {
        public override void Execute()
        {
            var paymentResponse = new PaymentResponseViewModel()
            {
                Id = "1234",
                Price = Request.Price,
                IsPaid = true
            };
            Response.Data = paymentResponse;
        }
    }
}
