using SSARMiddlware.Services.Base;
using SSARMiddlware.ViewModels.Scanner;
using System.Runtime.InteropServices;
using WIA;

namespace SSARMiddlware.Services
{
    internal class ScannerService : BaseService<ScannerRequestViewModel, ScannerResponseViewModel>
    {
        public override bool Validation()
        {
            //if (Request.PageNumber < 1)
            //{
            //    Response.Code = System.Net.HttpStatusCode.BadRequest;
            //    Response.Messages.Add("شماره صفحه نا معتبر می باشد");
            //    return false;
            //}
            return true;
        }

        public override void Execute()
        {
            var imgBase64 = Scan();
            Response.Data = new ScannerResponseViewModel()
            {
                ImageHeader = "data:image/jpeg;base64,",
                Image = imgBase64
            };
        }

        private byte[] Scan()
        {
            try
            {
                var deviceManager = new DeviceManager();
                DeviceInfo firstScannerAvailable = null;

                for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
                {
                    if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType)
                    {
                        continue;
                    }
                    firstScannerAvailable = deviceManager.DeviceInfos[i];
                    break;
                }
                if (firstScannerAvailable == null)
                {
                    Response.Code = System.Net.HttpStatusCode.NotFound;
                    Response.Messages.Add("اسکنر پیدا نشد");
                    return null;
                }

                var device = firstScannerAvailable.Connect();

                var scannerItem = device.Items[1];

                CommonDialogClass dlg = new CommonDialogClass();
                object scanResult = dlg.ShowTransfer(scannerItem, WIA.FormatID.wiaFormatPNG, true);

                if (scanResult != null)
                {
                    ImageFile image = (ImageFile)scanResult;
                    byte[] imageBytes = (byte[])image.FileData.get_BinaryData();
                    return imageBytes;
                }
                return null;
            }
            catch (COMException ex)
            {
                Response.Code = System.Net.HttpStatusCode.InternalServerError;
                Response.Messages.Add(ex.ToString());

                uint errorCode = (uint)ex.ErrorCode;

                if (errorCode == 0x80210006)
                {
                    Response.Messages.Add("اسکنر مشغول است و یا آماده نمی باشد");
                }
                else if (errorCode == 0x80210064)
                {
                    Response.Messages.Add("اسکن لغو شد");
                }
                else if (errorCode == 0x8021000C)
                {
                    Response.Messages.Add("تنظیمات اسکنر نادرست می باشد");
                }
                else if (errorCode == 0x80210005)
                {
                    Response.Messages.Add("اسکنر خاموش است و یا به کامپیوتر متصل نیست");
                }
                else if (errorCode == 0x80210001)
                {
                    Response.Messages.Add("خطای نا شناخته");
                }
                return null;
            }
        }
    }
}
