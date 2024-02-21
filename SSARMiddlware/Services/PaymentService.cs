using Newtonsoft.Json;
using SSARMiddlware.Services.Base;
using SSARMiddlware.ViewModels.Payment;
using WebSocketSharp;

namespace SSARMiddlware.Services
{
    internal class PaymentService : BaseService<PaymentRequestViewModel, PaymentResponseViewModel>
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            if (IsTokenValid)
            {
                var paymentResponse = new PaymentResponseViewModel()
                {
                    Id = "1234",
                    Price = Request.Price,
                    IsPaid = true
                };
                Response.Data = paymentResponse;
                var json = JsonConvert.SerializeObject(Response);
                Send(json);
            }
        }
    }
}
