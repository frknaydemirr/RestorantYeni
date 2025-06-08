using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRCoder;

namespace RestaurantSystem_2_.Areas.Admin.Controllers
{
    public class QRCodeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public FileResult Generate()
        {
            string domainUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(domainUrl, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(qrCodeData))
            using (Bitmap qrBitmap = qrCode.GetGraphic(20))
            using (MemoryStream ms = new MemoryStream())
            {
                qrBitmap.Save(ms, ImageFormat.Png);
                return File(ms.ToArray(), "image/png", "domain-qr.png");
            }
        }

        public ActionResult GetImage()
        {
            string domainUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(domainUrl, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(qrCodeData))
            using (Bitmap qrBitmap = qrCode.GetGraphic(20))
            using (MemoryStream ms = new MemoryStream())
            {
                qrBitmap.Save(ms, ImageFormat.Png);
                return File(ms.ToArray(), "image/png");
            }
        }
    }
}