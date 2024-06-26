﻿using SSARMiddlware.Services.Base;
using SSARMiddlware.ViewModels.Scanner;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using WIA;

namespace SSARMiddlware.Services
{
    internal class ScannerService : BaseService<ScannerRequestViewModel, ScannerResponseViewModel>
    {
        public override void Execute()
        {
            Response.Data = new ScannerResponseViewModel();
            var image = ScanDoc();
            if (image != null)
            {
                ConvertTiffToJpeg(image);
            }
            Send();
        }

        private ImageFile ScanDoc()
        {
            try
            {
                CommonDialogClass commonDialogClass = new CommonDialogClass();
                Device scannerDevice = commonDialogClass.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, true);
                if (scannerDevice == null)
                {
                    Response.Code = System.Net.HttpStatusCode.BadRequest;
                    Response.Messages.Add("اسکنری انتخاب نشد.");
                    return null;
                }
                Item scannnerItem = scannerDevice.Items[1];
                AdjustScannerSettings(scannnerItem, 150, 0, 0, 1250, 1700, 0, 0, (int)ColorType.Color);
                object scanResult = commonDialogClass.ShowTransfer(scannnerItem, WIA.FormatID.wiaFormatTIFF, false);
                if (scanResult == null)
                {
                    Response.Code = System.Net.HttpStatusCode.NotFound;
                    Response.Messages.Add("اسکن لغو شد.");
                    return null;
                }
                ImageFile image = (ImageFile)scanResult;
                //var convertedImage = ConvertImage(image, WIA.FormatID.wiaFormatJPEG);
                return image;
            }
            catch (COMException ex)
            {
                uint errorCode = (uint)ex.ErrorCode;
                switch (errorCode)
                {
                    case 0x80210001:
                        Response.Code = System.Net.HttpStatusCode.InternalServerError;
                        Response.Messages.Add("خطای نا شناخته.");
                        break;
                    case 0x80210003:
                        Response.Code = System.Net.HttpStatusCode.NotFound;
                        Response.Messages.Add("موردی برای اسکن وجود ندارد.");
                        break;
                    case 0x80210005:
                        Response.Code = System.Net.HttpStatusCode.NotFound;
                        Response.Messages.Add("اسکنر خاموش است و یا به رایانه متصل نیست.");
                        break;
                    case 0x80210006:
                        Response.Code = System.Net.HttpStatusCode.NotFound;
                        Response.Messages.Add("اسکنر مشغول است و یا آماده نمی باشد.");
                        break;
                    case 0x80210064:
                        Response.Code = System.Net.HttpStatusCode.BadRequest;
                        Response.Messages.Add("اسکن لغو شد.");
                        break;
                    case 0x8021000C:
                        Response.Code = System.Net.HttpStatusCode.BadRequest;
                        Response.Messages.Add("تنظیمات اسکنر نادرست می باشد.");
                        break;
                    case 0x80072EFD:
                    case 2149646357:
                        Response.Code = System.Net.HttpStatusCode.NotFound;
                        Response.Messages.Add("اسکنری به رایانه متصل نیست.");
                        break;
                    default:
                        Response.Code = System.Net.HttpStatusCode.InternalServerError;
                        Response.Messages.Add(ex.ToString());
                        break;
                }
                return null;
            }
        }

        private static void AdjustScannerSettings(IItem scannnerItem, int scanResolutionDPI, int scanStartLeftPixel, int scanStartTopPixel,
                    int scanWidthPixels, int scanHeightPixels, int brightnessPercents, int contrastPercents, int colorMode)
        {
            const string WIA_SCAN_COLOR_MODE = "6146";
            const string WIA_HORIZONTAL_SCAN_RESOLUTION_DPI = "6147";
            const string WIA_VERTICAL_SCAN_RESOLUTION_DPI = "6148";
            const string WIA_HORIZONTAL_SCAN_START_PIXEL = "6149";
            const string WIA_VERTICAL_SCAN_START_PIXEL = "6150";
            const string WIA_HORIZONTAL_SCAN_SIZE_PIXELS = "6151";
            const string WIA_VERTICAL_SCAN_SIZE_PIXELS = "6152";
            const string WIA_SCAN_BRIGHTNESS_PERCENTS = "6154";
            const string WIA_SCAN_CONTRAST_PERCENTS = "6155";
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_START_PIXEL, scanStartLeftPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_START_PIXEL, scanStartTopPixel);
            //SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_SIZE_PIXELS, scanWidthPixels);
            //SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_SIZE_PIXELS, scanHeightPixels);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_BRIGHTNESS_PERCENTS, brightnessPercents);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_CONTRAST_PERCENTS, contrastPercents);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_COLOR_MODE, colorMode);
        }

        private static void SetWIAProperty(IProperties properties, object propName, object propValue)
        {
            Property prop = properties.get_Item(ref propName);
            prop.set_Value(ref propValue);
        }

        private ImageFile ConvertImage(ImageFile image, string formatId)
        {
            if (image == null || string.IsNullOrWhiteSpace(formatId))
            {
                return null;
            }
            ImageProcess imgProcess = new ImageProcess();
            object convertFilter = "Convert";
            string convertFilterID = imgProcess.FilterInfos.get_Item(ref convertFilter).FilterID;
            imgProcess.Filters.Add(convertFilterID, 0);
            SetWIAProperty(imgProcess.Filters[imgProcess.Filters.Count].Properties, "FormatID", formatId);
            image = imgProcess.Apply(image);
            return image;
        }

        private void ConvertTiffToJpeg(ImageFile image)
        {
            if (image == null)
            {
                return;
            }
            var imageBytes = (byte[])image.FileData.get_BinaryData();
            var ms = new MemoryStream(imageBytes);
            var imageFile = Image.FromStream(ms);
            FrameDimension frameDimensions = new FrameDimension(imageFile.FrameDimensionsList[0]);
            int frameNum = imageFile.GetFrameCount(frameDimensions);
            for (int frame = 0; frame < frameNum; frame++)
            {
                imageFile.SelectActiveFrame(frameDimensions, frame);
                using (Bitmap bmp = new Bitmap(imageFile))
                {
                    ImageConverter converter = new ImageConverter();
                    Response.Data.Images.Add(new Page()
                    {
                        Name = Guid.NewGuid().ToString().Replace("-", "") + ".jpg",
                        Header = "data:image/jpeg;base64,",
                        Image = (byte[])converter.ConvertTo(bmp, typeof(byte[]))
                    });
                }
            }
        }

        private enum ColorType
        {
            BlackAndWhite,
            Color,
            GrayScale
        }
    }
}
