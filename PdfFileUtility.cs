using PdfSharp.Pdf;
using System.Diagnostics;

namespace HelloPdf
{

    //
    // Summary:
    //     Static helper functions for file IO. These function are intended for unit test
    //     und sample in a solution code only.
    public static class PdfFileUtility
    {
        //
        // Summary:
        //     Creates a temporary name of a PDF file with the pattern '{namePrefix}-{WIN|WSL|LNX|...}-{...uuid...}_temp.pdf'.
        //     The name ends with '_temp.pdf' to make it easy to delete it using the pattern
        //     '*_temp.pdf'. No file is created by this function. The name contains 10 hex characters
        //     to make it unique. It is not tested whether the file exists, because we have
        //     no path here.
        public static string GetTempPdfFileName(string? namePrefix, bool addOS = true)
        {
            return IOUtility.GetTempFileName(namePrefix, "pdf", addOS);
        }

        //
        // Summary:
        //     Creates a temporary file and returns the full name. The name pattern is '.../temp/.../{namePrefix}-{WIN|WSL|LNX|...}-{...uuid...}_temp.pdf'.
        //     The namePrefix may contain a sub-path relative to the temp directory. The name
        //     ends with '_temp.pdf' to make it easy to delete it using the pattern '*_temp.pdf'.
        //     The file is created and immediately closed. That ensures the returned full file
        //     name can be used.
        public static string GetTempPdfFullFileName(string? namePrefix, bool addOS = true)
        {
            return IOUtility.GetTempFullFileName(namePrefix, "pdf", addOS);
        }

        //
        // Summary:
        //     Finds the latest PDF temporary file in the specified folder, including sub-folders,
        //     or null, if no such file exists.
        //
        // Parameters:
        //   name:
        //     The name.
        //
        //   path:
        //     The path.
        //
        //   recursive:
        //     if set to true [recursive].
        public static string? FindLatestPdfTempFile(string? name, string path, bool recursive = false)
        {
            return IOUtility.FindLatestTempFile(name, path, "pdf", recursive);
        }

        //
        // Summary:
        //     Save the specified document and shows it in a PDF viewer application.
        //
        // Parameters:
        //   doc:
        //
        //   name:
        //
        //   addOS:
        public static void SaveAndShowDocument(PdfDocument doc, string name, bool addOS = true)
        {
            string tempPdfFullFileName = GetTempPdfFullFileName(name, addOS);
            doc.Save(tempPdfFullFileName);
            ShowDocument(tempPdfFullFileName);
        }

        //
        // Summary:
        //     Save the specified document and shows it in a PDF viewer application only if
        //     the current program is debugged.
        //
        // Parameters:
        //   doc:
        //
        //   name:
        //
        //   addOS:
        public static void SaveAndShowDocumentIfDebugging(PdfDocument doc, string name, bool addOS = true)
        {
            string tempPdfFullFileName = GetTempPdfFullFileName(name, addOS);
            doc.Save(tempPdfFullFileName);
            ShowDocumentIfDebugging(tempPdfFullFileName);
        }
        public static void ShowDocumentIfDebugging(string pdfFilename)
        {
            if (Debugger.IsAttached)
            {
                ShowDocument(pdfFilename);
            }
        }
        //
        // Summary:
        //     Shows the specified document in a PDF viewer application.
        //
        // Parameters:
        //   pdfFilename:
        //     The PDF filename.
        public static void ShowDocument(string pdfFilename)
        {

                Process.Start(new ProcessStartInfo(pdfFilename)
                {
                    UseShellExecute = true
                });
                return;
            }

            static void CopyFile(string pdfFilename)
            {
                try
                {
                    string viewerWatchDirectory = IOUtility.GetViewerWatchDirectory();
                    if (!Directory.Exists(viewerWatchDirectory))
                    {
                        Directory.CreateDirectory(viewerWatchDirectory);
                    }

                    string fileName = Path.GetFileName(pdfFilename);
                    File.Copy(pdfFilename, Path.Combine(viewerWatchDirectory, fileName));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
        }

        //
        // Summary:
        //     Shows the specified document in a PDF viewer application only if the current
        //     program is debugged.
        //
        // Parameters:
        //   pdfFilename:
        //     The PDF filename.

    }


