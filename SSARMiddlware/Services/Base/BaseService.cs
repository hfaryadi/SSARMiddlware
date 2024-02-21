using Newtonsoft.Json;
using SSARMiddlware.Interfaces.Base;
using SSARMiddlware.ViewModels.Base;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SSARMiddlware.Services.Base
{
    internal class BaseService<TRequest, TResponse> : WebSocketBehavior, IBaseService
    {
        public TRequest Request;
        public BaseResponse<TResponse> Response;

        protected override void OnMessage(MessageEventArgs e)
        {
            
            TokenValidation(e.Data);
        }

        private void TokenValidation(string request)
        {
            var token = Context.QueryString["Authorization"];
            Request = JsonConvert.DeserializeObject<TRequest>(request);
            Response = new BaseResponse<TResponse>();
            if (!JwtHelper.IsValidToken(token))
            {
                Response.Code = System.Net.HttpStatusCode.Unauthorized;
                Response.Messages.Add("توکن نا معتبر می باشد");
                var json = JsonConvert.SerializeObject(Response);
                Send(json);
            }
            else
            {
                if (Validation())
                {
                    Execute();
                }
                var json = JsonConvert.SerializeObject(Response);
                Send(json);
            }
        }

        public virtual bool Validation()
        {
            return true;
        }

        public virtual void Execute()
        {

        }

    }
}
