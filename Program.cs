using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

using PdfSharp.Snippets.Font;
using PdfSharp.Drawing;
using System.Net.NetworkInformation;

namespace HelloPdf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            if (PdfSharp.Capabilities.Build.IsCoreBuild)
                GlobalFontSettings.FontResolver = new FailsafeFontResolver();
            DoStuff1();
            DoStuff2();

        }

        static void DoStuff1()
        {
            // Create a MigraDoc document.
            var document = CreateDocument();

            var style = document.Styles[StyleNames.Normal]!;
            style.Font.Name = "Arial";

            // Create a renderer for the MigraDoc document.
            var pdfRenderer = new PdfDocumentRenderer
            {
                // Associate the MigraDoc document with a renderer.
                Document = document,
                PdfDocument = new PdfDocument()
            };

            // Layout and render document to PDF.
            pdfRenderer.RenderDocument();

            // Save the document...
            var filename = PdfFileUtility.GetTempPdfFullFileName("samples-MigraDoc/HelloWorldMigraDoc");
            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            PdfFileUtility.ShowDocument(filename);
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
            gfx.DrawEllipse(new XPen(XColors.Red, 1.5), XBrushes.White, new XRect(width / 2 - r, height / 2 - r, 2 * r, 2 * r));

            // Create a font.
            var font = new XFont("Times New Roman", 20, XFontStyleEx.BoldItalic);

            // Draw the text.
            gfx.DrawString("Hello, PDFsharp! This is Lisa!!!", font, XBrushes.Black,
                new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

            // Save the document...
            var filename = PdfFileUtility.GetTempPdfFullFileName("samples/HelloWorldSample");
            document.Save(filename);
            // ...and start a viewer.
            PdfFileUtility.ShowDocument(filename);
        }

        static Document CreateDocument()
        {
            // Create a new MigraDoc document.
            var document = new Document();

            // Add a section to the document.
            var section = document.AddSection();

            // Add a paragraph to the section.
            var header = section.AddParagraph();

            // Set font color.
            header.Format.Font.Color = Colors.DarkBlue;
            header.Format.Alignment = ParagraphAlignment.Left;

            // Add some text to the paragraph.
            header.AddFormattedText("Hi from Lisa! I am signing this document!", TextFormat.Bold);
            header.AddLineBreak();
            header.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            header.AddLineBreak();
            string imagePath = "C:\\tmp\\signature.png";
            Image signature = header.AddImage(imagePath);
            header.AddLineBreak();

            // Set the width and height of the image
            signature.Width = "15cm";

            var ipsum = section.AddParagraph();

            ipsum.Format.Font.Color = Colors.Black;
            ipsum.Format.Alignment = ParagraphAlignment.Justify;

            ipsum.AddFormattedText("Nyan nyan goes the cat, scraaaaape scraaaape goes the walls when the cat murders them with its claws bite plants sit on human so furrier and even more furrier hairball. Rub my belly hiss cat sit like bread be a nyan cat, feel great about it, be annoying 24/7 poop rainbows in litter box all day for scratch the furniture tweeting a baseball. Catch small lizards, bring them into house, then unable to find them on carpet spend six hours per day washing, but still have a crusty butthole but love yet play time white cat sleeps on a black shirt eat and than sleep on your face tuxedo cats always looking dapper. Stand with legs in litter box, but poop outside good now the other hand, too. Chirp at birds meow stare at ceiling light yowling nonstop the whole night eat plants, meow, and throw up because i ate plants lick the plastic bag yet drink water out of the faucet. Thinking longingly about tuna brine sit in a box for hours. Cat gets stuck in tree firefighters try to get cat down firefighters get stuck in tree cat eats firefighters' slippers purrr purr littel cat, little cat purr purr so i want to go outside let me go outside nevermind inside is better yet hide when guests come over love you, then bite you. Demand to be let outside at once, and expect owner to wait for me as i think about it. Steal mom's crouton while she is in the bathroom groom forever, stretch tongue and leave it slightly out, blep meowzer. Eat grass, throw it back up. Drink from the toilet stare at guinea pigs, yet meow meow we are 3 small kittens sleeping most of our time, we are around 15 weeks old i think, i donâ€™t know i canâ€™t count where is my slave? I'm getting hungry. Meow loved it, hated it, loved it, hated it. Adventure always toy mouse squeak roll over. Drink from the toilet munch on tasty moths, is good you understand your place in my world make meme, make cute face but ask to go outside and ask to come inside and ask to go outside and ask to come inside. It's 3am, time to create some chaos check cat door for ambush 10 times before coming in flop over, yet drink from the toilet claw drapes, so kitty kitty. Meow ask to go outside and ask to come inside and ask to go outside and ask to come inside. Annoy owner until he gives you food say meow repeatedly until belly rubs, feels good steal mom's crouton while she is in the bathroom so rub face on everything leave dead animals as gifts, for paw your face to wake you up in the morning. Catching very fast laser pointer. Intently stare at the same spot i want to go outside let me go outside nevermind inside is better meowzer so touch my tail, i shred your hand purrrr so in the middle of the night i crawl onto your chest and purr gently to help you sleep just going to dip my paw in your coffee and do a taste test - oh never mind i forgot i don't like coffee - you can have that back now. Plop down in the middle where everybody walks demand to be let outside at once, and expect owner to wait for me as i think about it caticus cuteicus so naughty running cat somehow manage to catch a bird but have no idea what to do next, so play with it until it dies of shock prow?? ew dog you drink from the toilet, yum yum warm milk hotter pls, ouch too hot, or ears back wide eyed. Ignore the squirrels, you'll never catch them anyway put butt in owner's face but loved it, hated it, loved it, hated it.");
            // Create the primary footer.
            var footer = section.Footers.Primary;

            var footerText = footer.AddParagraph();
            // Add content to footer.

            footerText.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            footerText.Format.Alignment = ParagraphAlignment.Center;

            // Add MigraDoc logo.
            // string imagePath = IOUtility.GetAssetsPath(@"migradoc\images\helpdesk.png")!;
            // document.LastSection.AddImage(imagePath);

            return document;
        }

    }
}
