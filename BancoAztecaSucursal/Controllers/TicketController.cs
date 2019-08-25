using BancoAztecaSucursal.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BancoAztecaSucursal.Controllers
{
    public class TicketController : Controller
    {

        // GET: Ticket
        public ActionResult Index()
        {
            PrintManager printManager = new PrintManager();
            var stringHtml = RenderViewToString(this, "Pdf", null);
            //ViewBag.qrBase64 = printManager.GenerateQRCode("url del ticket");  
            //var qr = printManager.GenerateQRCode("url del ticket");
            //qr = String.Format("data:image/jpg;base64,{0}", qr);
            //stringHtml = stringHtml.Replace("@QR", bytes.ToString());
            var pdf = printManager.GetPDF(stringHtml);
            //return View();
            return File(pdf, "application/pdf", "Ticket" + ".pdf");
        }


        public ActionResult GeneraQR() {

            PrintManager printManager = new PrintManager();
            string filePath = "D:\\QR.jpg";
            var QR64 = printManager.GenerateQRCode("url del ticket");
            ViewBag.qrBase64 = QR64;
            var bytes = Convert.FromBase64String(ViewBag.qrBase64);
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
            return View();
        }




        public ActionResult Pdf()
        {

            return View();
        }



        public static string RenderViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            // checking the view inside the controller  
            if (viewResult.View != null)
            {
                using (var sw = new StringWriter())
                {
                    var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                    var result = sw.GetStringBuilder().ToString();
                    return result;
                }
            }
            else
                return "View cannot be found.";
        }
    }
}