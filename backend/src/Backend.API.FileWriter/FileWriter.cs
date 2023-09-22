using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using Backend.Model;
using FirebaseAdmin.Auth;
using Backend.Interfaces;
using System.Text;

namespace Backend.API.FileWriter
{
    public class FileWriter : IFileWriter
    {
        public byte[] GeneratePDFBytes(Identity identity, UserRecord user)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Add a page to the document
            PdfPage page = document.AddPage();

            // Create a graphics object for drawing on the page
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Draw text or other elements on the page using the graphics object
            gfx.DrawString("User Data:", new XFont("Arial", 12), XBrushes.Black, new XRect(10, 10, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(identity.FullName, new XFont("Arial", 10), XBrushes.Black, new XRect(10, 30, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(user.Email, new XFont("Arial", 10), XBrushes.Black, new XRect(10, 50, page.Width, page.Height), XStringFormats.TopLeft);

            // Save the PDF document to a memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    document.Save(writer.BaseStream, false);
                }
                return stream.ToArray();
            }
        }
    }
}