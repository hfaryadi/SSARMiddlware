using SSARMiddlware.Services.Base;
using SSARMiddlware.ViewModels.Scanner;

namespace SSARMiddlware.Services
{
    internal class ScannerService : BaseService<ScannerRequestViewModel, ScannerResponseViewModel>
    {
        public override bool Validation()
        {
            if (Request.PageNumber < 1)
            {
                Response.Code = System.Net.HttpStatusCode.BadRequest;
                Response.Messages.Add("شماره صفحه نا معتبر می باشد");
                return false;
            }
            return true;
        }

        public override void Execute()
        {
            Response.Data = new ScannerResponseViewModel()
            {
                PageScanned = true
            };
        }
    }
}
