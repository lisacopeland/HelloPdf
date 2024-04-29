using PdfSharp.Fonts;
using System.Reflection;
using static PdfSharp.Snippets.Font.SegoeWpFontResolver;
using System.Runtime.CompilerServices;

namespace HelloPdf
{
    class MyFontResolver : IFontResolver
    {
        internal static MyFontResolver OurGlobalFontResolver = null;

        static KnownFonts[] OurFonts = new KnownFonts[]
        {
            new KnownFonts("arial", false, false, "Arial#", "HelloPdf.assets.ARIAL.TTF"),
            new KnownFonts("arial", true, false, "Arial#b", "HelloPdf.assets.ARIALBD.TTF"),
            new KnownFonts("arial", false, true, "Arial#i", "HelloPdf.assets.ARIALI.TTF"),
            new KnownFonts("arial", true, true, "Arial#bi", "HelloPdf.assets.ARIALBI.TTF"),
            new KnownFonts("timesnewroman", false, false, "Times#", "HelloPdf.assets.times.ttf"),
            new KnownFonts("timesnewroman", true, false, "Times#b", "HelloPdf.assets.timesbd.ttf"),
            new KnownFonts("timesnewroman", false, true, "Times#i", "HelloPdf.assets.timesi.ttf"),
            new KnownFonts("timesnewroman", true, true, "Times#bi", "HelloPdf.assets.timesbi.ttf"),
            new KnownFonts("queenthine", false, false, "Queenthine#", "HelloPdf.assets.Queenthine.ttf"),
            new KnownFonts("dehaviland", false, false, "Dehaviland#", "HelloPdf.assets.Dehaviland.ttf"),
            new KnownFonts("asemkandis", false, false, "AsemKandis#", "HelloPdf.assets.AsemKandis.ttf"),
        };

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Ignore case of font names.
            var name = familyName.ToLower().TrimEnd('#');

            foreach (KnownFonts kf in OurFonts)
            {
                if (kf.Name == name && kf.IsBold == isBold && kf.IsItalic == isItalic)
                {
                    return new FontResolverInfo(kf.ResolverName);
                }
            }

            foreach (KnownFonts kf in OurFonts)
            {
                if (kf.Name == name)
                {
                    return new FontResolverInfo(kf.ResolverName);
                }
            }


            // Deal with the fonts we know.
            //switch (name)
            //{
            //    case "arial":
            //        if (isBold)
            //        {
            //            if (isItalic)
            //                return new FontResolverInfo("Arial#bi");
            //            return new FontResolverInfo("Arial#b");
            //        }
            //        if (isItalic)
            //            return new FontResolverInfo("Arial#i");
            //        return new FontResolverInfo("Arial#");
            //    case "times new roman":
            //        if (isBold)
            //        {
            //            if (isItalic)
            //                return new FontResolverInfo("Times New Roman#bi");
            //            return new FontResolverInfo("Times New Roman#b");
            //        }
            //        if (isItalic)
            //            return new FontResolverInfo("Times New Roman#i");
            //        return new FontResolverInfo("Times New Roman#");

            //}

            // We pass all other font requests to the default handler.
            // When running on a web server without sufficient permission, you can return a default font at this stage.
            return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
        }

        /// <summary>
        /// Return the font data for the fonts.
        /// </summary>
        public byte[] GetFont(string faceName)
        {
            foreach (KnownFonts kf in OurFonts)
            {
                if (kf.ResolverName == faceName)
                {
                    return LoadFontData(kf.AssetName);
                }
            }


            //switch (faceName)
            //{
            //    case "Arial#":
            //        return FontHelper.Arial;

            //    case "Arial#b":
            //        return FontHelper.ArialBold;

            //    case "Arial#i":
            //        return FontHelper.ArialItalic;

            //    case "Arial#bi":
            //        return FontHelper.ArialBoldItalic;
            //}

            return null;
        }

        static byte[] LoadFontData(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Test code to find the names of embedded fonts
            //var ourResources = assembly.GetManifestResourceNames();

            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + name);

                int count = (int)stream.Length;
                byte[] data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }



        /// <summary>
        /// Ensure the font resolver is only applied once (or an exception is thrown)
        /// </summary>
        internal static void Apply()
        {
            if (OurGlobalFontResolver == null || GlobalFontSettings.FontResolver == null)
            {
                if (OurGlobalFontResolver == null)
                    OurGlobalFontResolver = new MyFontResolver();

                GlobalFontSettings.FontResolver = OurGlobalFontResolver;
            }
        }
    }


    /// <summary>
    /// Helper class that reads font data from embedded resources.
    /// </summary>
    //public static class FontHelper
    //{
    //    public static byte[] Arial
    //    {
    //        get { return LoadFontData("HelloPdf.assets.ARIAL.TTF"); }
    //    }

    //    public static byte[] ArialBold
    //    {
    //        get { return LoadFontData("HelloPdf.assets.ARIALBD.TTF"); }
    //    }

    //    public static byte[] ArialItalic
    //    {
    //        get { return LoadFontData("HelloPdf.assets.ARIALI.TTF"); }
    //    }

    //    public static byte[] ArialBoldItalic
    //    {
    //        get { return LoadFontData("HelloPdf.assets.ARIALBI.TTF"); }
    //    }

    //    /// <summary>
    //    /// Returns the specified font from an embedded resource.
    //    /// </summary>
    //    static byte[] LoadFontData(string name)
    //    {
    //        var assembly = Assembly.GetExecutingAssembly();

    //        // Test code to find the names of embedded fonts
    //        //var ourResources = assembly.GetManifestResourceNames();

    //        using (Stream stream = assembly.GetManifestResourceStream(name))
    //        {
    //            if (stream == null)
    //                throw new ArgumentException("No resource with name " + name);

    //            int count = (int)stream.Length;
    //            byte[] data = new byte[count];
    //            stream.Read(data, 0, count);
    //            return data;
    //        }
    //    }
    //}

    internal class KnownFonts
    {
        public string Name { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public string ResolverName { get; set; }
        public string AssetName { get; set; }

        public KnownFonts(string name, bool isBold, bool isItalic, string resolverName, string assetName)
        {
            Name = name;
            IsBold = isBold;
            IsItalic = isItalic;
            ResolverName = resolverName;
            AssetName = assetName;
        }
    }
}
