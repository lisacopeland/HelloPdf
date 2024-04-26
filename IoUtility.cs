namespace HelloPdf
{

    //
    // Summary:
    //     Static utility functions for file IO. These function are intended for unit test
    //     und sample in a solution code only.
    public static class IOUtility
    {
        internal const char DirectorySeparatorChar = '\\';

        internal const char AltDirectorySeparatorChar = '/';

        private static string? _solutionRoot;

        private static string? _assetsPath;

        private static string? _tempPath;

        private const string AssetsVersionFileName = ".assets-version";

        private const string AssetsInfo = "Run '.\\dev\\download-assets.ps1' in solution root directory to download the assets for the repository in question. For more information see 'Download assets' https://docs.pdfsharp.net/link/download-assets.html";

        //
        // Summary:
        //     True if the given character is a directory separator.
        public static bool IsDirectorySeparator(char ch)
        {
            if (ch == '/' || ch == '\\')
            {
                return true;
            }

            return false;
        }

        //
        // Summary:
        //     Replaces all back-slashes with forward slashes in the specified path. The resulting
        //     path works under Windows and Linux if no drive names are included.
        public static void NormalizeDirectorySeparators(ref string? path)
        {
            path = path?.Replace('\\', '/');
        }

        //
        // Summary:
        //     Gets the root path of the current solution, or null, if no parent directory with
        //     a solution file exists.
        public static string? GetSolutionRoot()
        {
            if (_solutionRoot != null)
            {
                return _solutionRoot;
            }

            string text = Directory.GetCurrentDirectory();
            while (true)
            {
                if (Directory.GetFiles(text, "*.sln", SearchOption.TopDirectoryOnly).Length != 0)
                {
                    return _solutionRoot = text;
                }

                DirectoryInfo parent = Directory.GetParent(text);
                if (parent == null)
                {
                    break;
                }

                text = parent.FullName;
            }

            return null;
        }

        //
        // Summary:
        //     Gets the root path of the current assets directory if no parameter is specified,
        //     or null, if no assets directory exists in the solution root directory. If a parameter
        //     is specified gets the assets root path combined with the specified relative path
        //     or file. If only the root path is returned it always ends with a directory separator.
        //     If a parameter is specified the return value ends literally with value of the
        //     parameter.
        public static string? GetAssetsPath(string? relativePathOrFileInAsset = null)
        {
            if (_assetsPath == null)
            {
                string solutionRoot = GetSolutionRoot();
                if (solutionRoot == null)
                {
                    return null;
                }

                _assetsPath = Path.Combine(solutionRoot, "assets" + Path.DirectorySeparatorChar);
            }

            if (string.IsNullOrEmpty(relativePathOrFileInAsset))
            {
                return _assetsPath;
            }

            return Path.Combine(_assetsPath, relativePathOrFileInAsset);
        }

        //
        // Summary:
        //     Gets the root or sub path of the current temp directory. The directory is created
        //     if it does not exist. If a valid path is returned it always ends with the current
        //     directory separator.
        public static string? GetTempPath(string? relativeDirectoryInTemp = null)
        {
            if (_tempPath == null)
            {
                _tempPath = GetAssetsPath() + "temp" + Path.DirectorySeparatorChar;
                if (!Directory.Exists(_tempPath))
                {
                    Directory.CreateDirectory(_tempPath);
                }
            }

            if (string.IsNullOrEmpty(relativeDirectoryInTemp))
            {
                return _tempPath;
            }

            string text = Path.Combine(_tempPath, relativeDirectoryInTemp);
            string text2 = text;
            switch (text2[text2.Length - 1])
            {
                case '\\':
                    if (Path.DirectorySeparatorChar != '\\')
                    {
                        string text3 = text;
                        text = text3.Substring(0, text3.Length - 1) + Path.DirectorySeparatorChar;
                    }

                    break;
                default:
                    text += Path.DirectorySeparatorChar;
                    break;
                case '/':
                    break;
            }

            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }

            return text;
        }

        //
        // Summary:
        //     Gets the viewer watch directory. Which is currently just a hard-coded directory
        //     on drive C or /mnt/c
        public static string GetViewerWatchDirectory()
        {

                return "c:\\PDFsharpViewer";
        }

        //
        // Summary:
        //     Creates a temporary file name with the pattern '{namePrefix}-{WIN|WSL|LNX|...}-{...uuid...}_temp.{extension}'.
        //     The name ends with '_temp.' to make it easy to delete it using the pattern '*_temp.{extension}'.
        //     No file is created by this function. The name contains 10 hex characters to make
        //     it unique. It is not tested whether the file exists, because we have no path
        //     here.
        public static string GetTempFileName(string? namePrefix, string? extension, bool addOS = true)
        {
            string text = Guid.NewGuid().ToString("N").Substring(0, 10)
                .ToUpperInvariant();
            string oSAbbreviation = "WIN";
            string text2 = ((!string.IsNullOrEmpty(namePrefix)) ? (addOS ? $"{namePrefix}-{oSAbbreviation}-{text}_temp" : (namePrefix + "-" + text + "_temp")) : (addOS ? (oSAbbreviation + "-" + text + "_temp") : (text + "_temp")));
            if (!string.IsNullOrEmpty(extension))
            {
                text2 = text2 + "." + extension;
            }

            return text2;
        }

        //
        // Summary:
        //     Creates a temporary file and returns the full name. The name pattern is '/.../temp/.../{namePrefix}-{WIN|WSL|LNX|...}-{...uuid...}_temp.{extension}'.
        //     The namePrefix may contain a sub-path relative to the temp directory. The name
        //     ends with '_temp.' to make it easy to delete it using the pattern '*_temp.{extension}'.
        //     The file is created and immediately closed. That ensures the returned full file
        //     name can be used.
        public static string GetTempFullFileName(string? namePrefix, string? extension, bool addOS = true)
        {
            string text = null;
            string namePrefix2 = null;
            int num = namePrefix?.Length ?? 0;
            if (num > 0 && namePrefix != null)
            {
                int num2 = num - 1;
                while (num2 >= 0 && !IsDirectorySeparator(namePrefix[num2]))
                {
                    num2--;
                }

                if (num2 > 0)
                {
                    text = namePrefix.Substring(0, num2);
                }

                if (num2 < num - 1)
                {
                    int num3 = num2 + 1;
                    namePrefix2 = namePrefix.Substring(num3, namePrefix.Length - num3);
                }
            }

            string text2 = GetTempPath() ?? throw new IOException("Cannot localize temp directory. Your current directory may not be part of a solution.");
            if (text != null)
            {
                text2 = Path.Combine(text2, text);
                if (!Directory.Exists(text2))
                {
                    Directory.CreateDirectory(text2);
                }
            }

            int num4 = 3;
            while (true)
            {
                string tempFileName = GetTempFileName(namePrefix2, extension, addOS);
                tempFileName = Path.Combine(text2, tempFileName);
                try
                {
                    using FileStream fileStream = new FileStream(tempFileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
                    fileStream.Close();
                    return tempFileName;
                }
                catch
                {
                    if (num4-- > 0)
                    {
                        continue;
                    }

                    throw;
                }
            }
        }

        //
        // Summary:
        //     Finds the latest temporary file in a directory. The pattern of the file is expected
        //     to be '{namePrefix}*_temp.{extension}'.
        //
        // Parameters:
        //   namePrefix:
        //     The prefix of the file name to search for.
        //
        //   path:
        //     The path to search in.
        //
        //   extension:
        //     The file extension of the file.
        //
        //   recursive:
        //     If set to true subdirectories are included in the search.
        public static string? FindLatestTempFile(string? namePrefix, string path, string extension = "tmp", bool recursive = false)
        {
            string searchPattern2 = (string.IsNullOrEmpty(namePrefix) ? ("*_temp." + extension) : (namePrefix + "*_temp." + extension));
            (string, DateTime?) result2 = default((string, DateTime?));
            FindFile(searchPattern2, ref result2, path, recursive);
            return result2.Item1;
            static void FindFile(string searchPattern, ref (string? FileName, DateTime? LastWrite) result, string directory, bool recursive)
            {
                string[] files = Directory.GetFiles(directory, searchPattern);
                foreach (string text in files)
                {
                    DateTime lastAccessTime = File.GetLastAccessTime(text);
                    if (result.LastWrite.HasValue)
                    {
                        DateTime value = lastAccessTime;
                        DateTime? item = result.LastWrite;
                        if (!(value > item))
                        {
                            continue;
                        }
                    }

                    result.FileName = text;
                    result.LastWrite = lastAccessTime;
                }

                if (recursive)
                {
                    files = Directory.GetDirectories(directory);
                    foreach (string directory2 in files)
                    {
                        FindFile(searchPattern, ref result, directory2, recursive);
                    }
                }
            }
        }

        //
        // Summary:
        //     Ensures the assets folder exists in the solution root and an optional specified
        //     file or directory exists. If relativeFileOrDirectory is specified, it is considered
        //     to be a path to a directory if it ends with a directory separator. Otherwise,
        //     it is considered to ba a path to a file.
        //
        // Parameters:
        //   relativeFileOrDirectory:
        //     A relative path to a file or directory.
        //
        //   requiredAssetsVersion:
        //     The minimum of the required assets version.
        public static void EnsureAssets(string? relativeFileOrDirectory = null, int? requiredAssetsVersion = null)
        {
            string assetsPath = GetAssetsPath();
            if (assetsPath != null && Directory.Exists(assetsPath))
            {
                if (!Directory.Exists(assetsPath + "temp"))
                {
                    Directory.CreateDirectory(assetsPath + "temp");
                }

                if (requiredAssetsVersion.HasValue)
                {
                    EnsureAssetsVersion(requiredAssetsVersion.Value);
                }

                if (relativeFileOrDirectory != null)
                {
                    relativeFileOrDirectory = Path.Combine(assetsPath, relativeFileOrDirectory);
                    string value = "file";
                    string? text = relativeFileOrDirectory;
                    char c = text[text.Length - 1];
                    if ((c == '/' || c == '\\') ? true : false)
                    {
                        value = "directory";
                        if (!Directory.Exists(relativeFileOrDirectory))
                        {
                            throw new IOException($"The {value} '{relativeFileOrDirectory}' does not exist in the assets folder. " + "Run '.\\dev\\download-assets.ps1' in solution root directory to download the assets for the repository in question. For more information see 'Download assets' https://docs.pdfsharp.net/link/download-assets.html");
                        }
                    }
                    else if (!File.Exists(relativeFileOrDirectory))
                    {
                        throw new IOException($"The {value} '{relativeFileOrDirectory}' does not exist in the assets folder. " + "Run '.\\dev\\download-assets.ps1' in solution root directory to download the assets for the repository in question. For more information see 'Download assets' https://docs.pdfsharp.net/link/download-assets.html");
                    }
                }
                else if (Directory.GetDirectories(assetsPath).Length == 0 || !File.Exists(assetsPath + ".assets-version"))
                {
                    throw new IOException("The assets folder is not yet downloaded. Run '.\\dev\\download-assets.ps1' in solution root directory to download the assets for the repository in question. For more information see 'Download assets' https://docs.pdfsharp.net/link/download-assets.html");
                }

                return;
            }

            throw new IOException("The assets folder does not exist. Run '.\\dev\\download-assets.ps1' in solution root directory to download the assets for the repository in question. For more information see 'Download assets' https://docs.pdfsharp.net/link/download-assets.html");
        }

        //
        // Summary:
        //     Ensures the assets directory exists in the solution root and its version is at
        //     least the specified version.
        //
        // Parameters:
        //   requiredAssetsVersion:
        //     The minimum of the required assets version.
        public static void EnsureAssetsVersion(int requiredAssetsVersion)
        {
            string assetsPath = GetAssetsPath();
            if (assetsPath != null && Directory.Exists(assetsPath))
            {
                string path = Path.Combine(assetsPath, ".assets-version");
                if (File.Exists(path) && int.TryParse(File.ReadAllText(path), out var result))
                {
                    if (result >= requiredAssetsVersion)
                    {
                        return;
                    }

                    throw new IOException(FormattableString.Invariant($"The required assets version is {requiredAssetsVersion}, but the current version is just {result}. ") + "Run '.\\dev\\download-assets.ps1' in solution root directory to download the assets for the repository in question. For more information see 'Download assets' https://docs.pdfsharp.net/link/download-assets.html");
                }

                throw new IOException("The assets version file '.assets-version' does not exist in the assets folder. Run '.\\dev\\download-assets.ps1' in solution root directory to download the assets for the repository in question. For more information see 'Download assets' https://docs.pdfsharp.net/link/download-assets.html");
            }

            throw new IOException("The assets folder does not exist. Run '.\\dev\\download-assets.ps1' in solution root directory to download the assets for the repository in question. For more information see 'Download assets' https://docs.pdfsharp.net/link/download-assets.html");
        }
    }
}
