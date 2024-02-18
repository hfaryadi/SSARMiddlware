using Newtonsoft.Json;
using SSARMiddlware.ViewModels.Payment;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SSARMiddlware.Services
{
    internal class PaymentService : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var request = JsonConvert.DeserializeObject<PaymentRequestViewModel>(e.Data);
            var response = new PaymentResponseViewModel()
            {
                Id = "1234",
                Price = request.Price,
                IsPaid = true
            };
            var responseJson = JsonConvert.SerializeObject(response);
            Send(responseJson);
        }
    }
}
