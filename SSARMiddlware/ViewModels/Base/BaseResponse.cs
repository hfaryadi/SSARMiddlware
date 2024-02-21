using System.Collections.Generic;
using System.Net;

namespace SSARMiddlware.ViewModels.Base
{
    internal class BaseResponse
    {
        public BaseResponse() { }
        public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;
        public ICollection<string> Messages { get; set; } = new List<string>();
    }

    internal class BaseResponse<TData>
    {
        public BaseResponse() { }
        public BaseResponse(TData data)
        {
            this.Data = data;
        }
        public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;
        public ICollection<string> Messages { get; set; } = new List<string>();
        public TData Data { get; set; }
    }
}
