using Newtonsoft.Json;
using SSARMiddlware.Interfaces.Base;
using SSARMiddlware.ViewModels.Base;
using System;
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
            try
            {
                Response = new BaseResponse<TResponse>();
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    Request = JsonConvert.DeserializeObject<TRequest>(e.Data);
                }
                TokenValidation();
            }
            catch (Exception ex)
            {
                Response.Code = System.Net.HttpStatusCode.InternalServerError;
                Response.Messages.Add(ex.Message.ToString());
                var json = JsonConvert.SerializeObject(Response);
                Send(json);
            }
        }

        private void TokenValidation()
        {
            var token = Context.QueryString["Authorization"];
            if (!JwtHelper.IsValidToken(token))
            {
                Reject();
            }
            else
            {
                Accept();
            }
        }

        private void Reject()
        {
            Response.Code = System.Net.HttpStatusCode.Unauthorized;
            Response.Messages.Add("توکن نا معتبر می باشد");
            var json = JsonConvert.SerializeObject(Response);
            Send(json);
        }

        private void Accept()
        {
            if (Validation())
            {
                Execute();
            }
            var json = JsonConvert.SerializeObject(Response);
            Send(json);
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
