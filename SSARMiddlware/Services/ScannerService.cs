using SSARMiddlware.Services.Base;
using SSARMiddlware.ViewModels.Scanner;
using System.Runtime.InteropServices;
using WIA;

namespace SSARMiddlware.Services
{
    internal class ScannerService : BaseService<ScannerRequestViewModel, ScannerResponseViewModel>
    {
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

                AdjustScannerSettings(scannerItem, 0, 0, 1, 150,  1250, 1700, 0, 0);

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

        private static void AdjustScannerSettings(IItem scannnerItem, int scanStartLeftPixel, int scanStartTopPixel, int colorMode, int scanResolutionDPI, int scanWidthPixels, int scanHeightPixels, int brightnessPercents, int contrastPercents)
        {
            const string WIA_HORIZONTAL_SCAN_START_PIXEL = "6149";
            const string WIA_VERTICAL_SCAN_START_PIXEL = "6150";
            const string WIA_SCAN_COLOR_MODE = "6146";
            const string WIA_HORIZONTAL_SCAN_RESOLUTION_DPI = "6147";
            const string WIA_VERTICAL_SCAN_RESOLUTION_DPI = "6148";
            const string WIA_HORIZONTAL_SCAN_SIZE_PIXELS = "6151";
            const string WIA_VERTICAL_SCAN_SIZE_PIXELS = "6152";
            const string WIA_SCAN_BRIGHTNESS_PERCENTS = "6154";
            const string WIA_SCAN_CONTRAST_PERCENTS = "6155";
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_START_PIXEL, scanStartLeftPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_START_PIXEL, scanStartTopPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_COLOR_MODE, colorMode);
            //SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            //SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            //SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_SIZE_PIXELS, scanWidthPixels);
            //SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_SIZE_PIXELS, scanHeightPixels);
            //SetWIAProperty(scannnerItem.Properties, WIA_SCAN_BRIGHTNESS_PERCENTS, brightnessPercents);
            //SetWIAProperty(scannnerItem.Properties, WIA_SCAN_CONTRAST_PERCENTS, contrastPercents);
        }

        private static void SetWIAProperty(IProperties properties, object propName, object propValue)
        {
            Property prop = properties.get_Item(ref propName);
            prop.set_Value(ref propValue);
        }
    }
}
