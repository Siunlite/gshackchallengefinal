using BancoAztecaSucursal.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BancoAztecaSucursal.Controllers
{
    public class MessageController : Controller
    {

        const string accountSid = "ACbed16a991f503ac57ccf049b083d1e73";
        const string authToken = "90d65c8c7772f51b3ca3584a6aa5afd5";

        // GET: Message
        public ActionResult Index()
        {
            PrintManager printManager = new PrintManager();

            ViewBag.qrBase64 = printManager.GenerateQRCode("CULHUAPOS");
            string yourMessage = ViewBag.qrBase64;
            string yourId = "V6mFTJgzqUGsBevOqtPg+WNhcmxvc2phdmllcmp2X2F0X2dtYWlsX2RvdF9jb20=";
            string yourMobile = "+5215569164571";
            //string yourMessage = "Su operación se realizo correctamente";

            try
            {
                string url = "https://NiceApi.net/API";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("X-APIId", yourId);
                request.Headers.Add("X-APIMobile", yourMobile);
                using (StreamWriter streamOut = new StreamWriter(request.GetRequestStream()))
                {
                    streamOut.Write(yourMessage);
                }
                using (StreamReader streamIn = new StreamReader(request.GetResponse().GetResponseStream()))
                {

                    return View();

                    //Console.WriteLine(streamIn.ReadToEnd());
                }
            }
            catch (SystemException se)
            {
                Console.WriteLine(se.Message);
            }

            return View();
        }

        public ActionResult TwilioMessage()
        {


            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Hola Putines",
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                to: new Twilio.Types.PhoneNumber("whatsapp:+5215569164571")

            );

            return View();
        }


        public ActionResult TwilioMediaMessage()
        {
            TwilioClient.Init(accountSid, authToken);

            var mediaUrl = new[] {
            new Uri("https://images.unsplash.com/photo-1545093149-618ce3bcf49d?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=668&q=80")
        }.ToList();

            var message = MessageResource.Create(
                mediaUrl: mediaUrl,
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                body: "It's taco time!",
                to: new Twilio.Types.PhoneNumber("whatsapp:+5215569164571")
            );
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
                    return sw.GetStringBuilder().ToString();
                }
            }
            else
                return "View cannot be found.";
        }


    }
}