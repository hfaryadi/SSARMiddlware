using Newtonsoft.Json;
using SSARMiddlware.ViewModels.Base;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SSARMiddlware.Services.Base
{
    internal class BaseService<TRequest, TResponse> : WebSocketBehavior
    {
        public TRequest Request;
        public BaseResponse<TResponse> Response;

        protected override void OnMessage(MessageEventArgs e)
        {
            var token = Context.QueryString["Authorization"];
            TokenValidation(token, e.Data);
        }

        public void TokenValidation(string token, string request)
        {
            Request = JsonConvert.DeserializeObject<TRequest>(request);
            Response = new BaseResponse<TResponse>();
            if (!JwtHelper.IsValidToken(token))
            {
                Response.Code = System.Net.HttpStatusCode.Unauthorized;
                Response.Messages.Add("Invalid Token");
                var json = JsonConvert.SerializeObject(Response);
                Send(json);
            }
            else
            {
                Execute();
                var json = JsonConvert.SerializeObject(Response);
                Send(json);
            }
        }

        public virtual void Execute()
        {

        }

    }
}
