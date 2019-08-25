using QRCoder;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace BancoAztecaSucursal.Managers
{
    public class PrintManager
    {
        //Genera el arreglo de bits para el base64 del PDF
        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;
            MemoryStream ms = new MemoryStream();
            HtmlToPdf converter = new HtmlToPdf();
            string pdf_page_size = "Letter";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);
            //  page settings
            converter.Options.PdfPageSize = pageSize;
            converter.Options.WebPageWidth = 1024;
            converter.Options.WebPageHeight = 0;
            converter.Header.Height = 50;
            converter.Options.MarginLeft = 15;
            converter.Options.MarginRight = 15;
            converter.Options.MarginTop = 30;
            converter.Options.MarginBottom = 30;
            //// header settings
            //converter.Options.DisplayHeader = true;
            //converter.Header.DisplayOnFirstPage = true;
            //converter.Header.DisplayOnOddPages = false;
            //converter.Header.DisplayOnEvenPages = false;
            //converter.Header.Height = 90;
            //// footer settings
            //converter.Options.DisplayFooter = true;
            //converter.Footer.DisplayOnFirstPage = true;
            //converter.Footer.DisplayOnOddPages = false;
            //converter.Footer.DisplayOnEvenPages = false;
            //converter.Footer.Height = 60;


            var Texto_FormatHTML = pHTML;
            PdfDocument doc = converter.ConvertHtmlString(Texto_FormatHTML);
            doc.Save(ms);
            doc.Close();
            bPDF = ms.ToArray();
            return bPDF;
        }

        //Genera el codigo de QR en base 64
        public string GenerateQRCode(string any)
        {
            string apiUrl = any;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(apiUrl, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);

            var imgType = Base64QRCode.ImageType.Jpeg;
            var qrCodeImageAsBase64 = qrCode.GetGraphic(50,
                Color.Black, Color.White, true, imgType);

            return qrCodeImageAsBase64;
        }


    }
}