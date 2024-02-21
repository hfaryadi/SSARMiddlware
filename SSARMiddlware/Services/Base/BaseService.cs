using Newtonsoft.Json;
using SSARMiddlware.Interfaces.Base;
using SSARMiddlware.ViewModels.Base;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SSARMiddlware.Services.Base
{
    internal class BaseService<TRequest,TResponse> : WebSocketBehavior, IBaseService<TResponse>
    {
        public TRequest Request;
        public BaseResponse<TResponse> Response;
        public bool IsTokenValid;
        public void TokenValidation(string token)
        {
            Response = new BaseResponse<TResponse>();
            if (!JwtHelper.IsValidToken(token))
            {
                IsTokenValid = false;
                Response.Code = System.Net.HttpStatusCode.Unauthorized;
                Response.Messages.Add("Invalid Token");
                var json = JsonConvert.SerializeObject(Response);
                Send(json);
            }
            else
            {
                IsTokenValid = true;
            }
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var token = Context.QueryString["Authorization"];
            TokenValidation(token);
            Request = JsonConvert.DeserializeObject<TRequest>(e.Data);
        }
    }
}
