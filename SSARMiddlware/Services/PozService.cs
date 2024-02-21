using SSARMiddlware.Services.Base;
using SSARMiddlware.ViewModels.Poz;

namespace SSARMiddlware.Services
{
    internal class PozService : BaseService<PozRequestViewModel, PozResponseViewModel>
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
            Response.Data = new PozResponseViewModel()
            {
                Id = "1234",
                Price = Request.Price,
                IsPaid = true
            };
        }
    }
}
