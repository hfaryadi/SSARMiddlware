using SSARMiddlware.Services.Base;
using SSARMiddlware.ViewModels.Payment;

namespace SSARMiddlware.Services
{
    internal class PaymentService : BaseService<PaymentRequestViewModel, PaymentResponseViewModel>
    {
        public override bool Validation()
        {
            if (Request.Price < 1)
            {
                Response.Code = System.Net.HttpStatusCode.BadRequest;
                Response.Messages.Add("مبلغ نا معتبر می باشد");
                return false;
            }
            return true;
        }

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
