using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Runtime.CompilerServices;
using static PdfSharp.Snippets.Drawing.ImageHelper;

namespace HelloPdf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            if (PdfSharp.Capabilities.Build.IsCoreBuild)
            {
                GlobalFontSettings.FontResolver = new MyFontResolver();
            }
            CreateSignatureDocument("C:\\tmp\\faqs.pdf", "C:\\tmp\\signedfaqs.pdf");
        }

        // produces the pdfDocument Signature Page
        static void CreateSignatureDocument(
            string originalDocumentFileName,
            string destinationFileName
        )
        {
            // Create a MigraDoc document.
            var document = CreateSignaturePage();

            // Create a renderer for the MigraDoc document.
            var pdfRenderer = new PdfDocumentRenderer
            {
                // Associate the MigraDoc document with a renderer.
                Document = document,
                PdfDocument = new PdfDocument()
            };

            // Layout and render document to PDF.
            pdfRenderer.RenderDocument();

            // This is so you can see it for debugging
            // var filename = PdfFileUtility.GetTempPdfFullFileName("samples-MigraDoc/HelloWorldMigraDoc");
            // pdfRenderer.PdfDocument.Save(filename);
            // PdfFileUtility.ShowDocument(filename);

            PdfDocument pdfDocument = pdfRenderer.PdfDocument;
            // pdfDocument.Save(destinationFileName);

            PdfDocument outputDocument = new PdfDocument();
            PdfDocument rootDoc = PdfReader.Open(
                originalDocumentFileName,
                PdfDocumentOpenMode.Import
            );
            int count = rootDoc.PageCount;
            for (int idx = 0; idx < count; idx++)
            {
                PdfPage page = rootDoc.Pages[idx];
                outputDocument.AddPage(page);
            }

            pdfDocument.Save("C:\\tmp\\temppdf.pdf");
            PdfDocument newPdf = PdfReader.Open("C:\\tmp\\temppdf.pdf", PdfDocumentOpenMode.Import);
            int count2 = newPdf.PageCount;
            for (int idx = 0; idx < count2; idx++)
            {
                PdfPage page = newPdf.Pages[idx];
                outputDocument.AddPage(page);
            }
            outputDocument.Save(destinationFileName);
        }

        static void DoStuff2()
        {
            Console.WriteLine("doing stuff");
            var document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";
            document.Info.Subject = "Just a simple Hello-World program.";

            // Create an empty page in this document.
            var page = document.AddPage();
            //page.Size = PageSize.Letter;

            // Get an XGraphics object for drawing on this page.
            var gfx = XGraphics.FromPdfPage(page);

            // Draw two lines with a red default pen.
            var width = page.Width;
            var height = page.Height;
            gfx.DrawLine(XPens.Red, 0, 0, width, height);
            gfx.DrawLine(XPens.Red, width, 0, 0, height);

            // Draw a circle with a red pen which is 1.5 point thick.
            var r = width / 5;
            gfx.DrawEllipse(
                new XPen(XColors.Red, 1.5),
                XBrushes.White,
                new XRect(width / 2 - r, height / 2 - r, 2 * r, 2 * r)
            );

            // Create a font.
            // var font = new XFont("Arial", 20, XFontStyleEx.);
            var font = new XFont("Times New Roman", 20, XFontStyleEx.BoldItalic);
            // Draw the text.
            gfx.DrawString(
                "Hello, PDFsharp! This is Lisa!!!",
                font,
                XBrushes.Black,
                new XRect(0, 0, page.Width, page.Height),
                XStringFormats.Center
            );

            // Save the document...
            var filename = PdfFileUtility.GetTempPdfFullFileName("samples/HelloWorldSample");
            document.Save(filename);
            // ...and start a viewer.
            PdfFileUtility.ShowDocument(filename);
        }

        static Document CreateSignaturePage()
        {
            // Create a new MigraDoc document.
            var document = new Document();

            // Add a section to the document.
            var section = document.AddSection();

            // Create the header
            var headerSection = section.Headers.Primary;
            var headerText = headerSection.AddParagraph();
            headerText.Format.Font.Color = Colors.Black;
            headerText.Format.Font.Name = "Timesnewroman";
            headerText.Format.Font.Italic = true;
            headerText.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            headerText.Format.Alignment = ParagraphAlignment.Center;
            headerText.AddLineBreak();

            var sectionTitle = section.AddParagraph();
            sectionTitle.Format.Font.Name = "Timesnewroman";
            sectionTitle.Format.Font.Color = Colors.CadetBlue;
            sectionTitle.Format.Font.Bold = true;
            sectionTitle.Format.Font.Size = 20;
            sectionTitle.AddFormattedText("e-Signatures");
            sectionTitle.AddLineBreak();
            sectionTitle.AddLineBreak();
            sectionTitle.AddLineBreak();

            // Add a paragraph to the section.
            var SignatureSection = section.AddParagraph();

            // Set font color.
            SignatureSection.Format.Font.Color = Colors.Black;
            SignatureSection.Format.Font.Name = "Timesnewroman";
            SignatureSection.Format.Font.Size = 15;
            SignatureSection.Format.Alignment = ParagraphAlignment.Left;
            SignatureSection.AddFormattedText("Timezone: US/Pacific");
            SignatureSection.AddLineBreak();
            SignatureSection.AddFormattedText("Certified & Esigned", TextFormat.Bold);
            // Add some text to the paragraph.
            SignatureSection.AddFormattedText("by Lisa Copeland 55 days ago (");
            SignatureSection.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            SignatureSection.AddFormattedText(")");
            SignatureSection.AddLineBreak();
            SignatureSection.AddFormattedText(
                "I certify that this is a true and correct copy",
                TextFormat.Italic
            );
            SignatureSection.AddLineBreak();

            var signatureLine1 = section.AddParagraph();
            signatureLine1.Format.Font.Color = Colors.Black;
            signatureLine1.Format.Font.Bold = true;
            signatureLine1.Format.Font.Name = "Queenthine";
            signatureLine1.Format.Font.Size = 40;
            signatureLine1.Format.Alignment = ParagraphAlignment.Justify;
            signatureLine1.AddFormattedText("Lisa Marie Copeland");
            signatureLine1.AddLineBreak();

            var signatureLine2 = section.AddParagraph();
            signatureLine2.Format.Font.Color = Colors.Black;
            signatureLine2.Format.Font.Bold = true;
            signatureLine2.Format.Font.Name = "AsemKandis";
            signatureLine2.Format.Font.Size = 40;
            signatureLine2.Format.Alignment = ParagraphAlignment.Justify;
            signatureLine2.AddFormattedText("Lisa Marie Copeland");
            signatureLine2.AddLineBreak();

            var signatureLine3 = section.AddParagraph();
            signatureLine3.Format.Font.Color = Colors.Black;
            signatureLine3.Format.Font.Bold = true;
            signatureLine3.Format.Font.Name = "Dehaviland";
            signatureLine3.Format.Font.Size = 40;
            signatureLine3.Format.Alignment = ParagraphAlignment.Justify;
            signatureLine3.AddFormattedText("Lisa Marie Copeland");
            signatureLine3.AddLineBreak();

            // Create the primary footer.
            var footer = section.Footers.Primary;

            var footerText = footer.AddParagraph();
            // Add content to footer.
            footerText.Format.Font.Color = Colors.Black;
            footerText.Format.Font.Name = "Arial";
            footerText.Format.Font.Italic = true;
            footerText.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            footerText.Format.Alignment = ParagraphAlignment.Center;

            return document;
        }
    }
}
