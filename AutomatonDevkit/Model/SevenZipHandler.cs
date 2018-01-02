using System;
using System.Diagnostics;
using System.IO;

namespace AutomatonDevkit.Model
{
    public class SevenZipHandler : IDisposable
    {
        private static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string TempDirectory = Path.Combine(BaseDirectory, "temp");
        private static string ExePath = Path.Combine(BaseDirectory, "7za.exe");

        private static string ExtractedFilePath;

        public SevenZipHandler()
        {
            // Load the embedded resource into memory, and write to temporary file.
            var embeddedBytes = Properties.Resources._7za;

            if (File.Exists(ExePath))
            {
                File.Delete(ExePath);
            }

            using (var fileStream = new FileStream(ExePath, FileMode.CreateNew))
            {
                fileStream.Write(embeddedBytes, 0, embeddedBytes.Length);
            }
        }

        /// <summary>
        /// Will extract the archive to the temp directory and target it within the class.
        /// </summary>
        /// <param name="path">The path of the archive file (.7z, .zip, .rar)</param>
        public void ExtractArchive(string path)
        {
            if (Directory.Exists(ExtractedFilePath))
            {
                Directory.Delete(ExtractedFilePath, true);
            }

            var extractedPath = Path.Combine(TempDirectory, Path.GetFileNameWithoutExtension(path));

            if (Directory.Exists(extractedPath))
            {
                Directory.Delete(extractedPath, true);
            }

            Directory.CreateDirectory(extractedPath);

            // Spool up a process to extract the file using 7za.exe
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = ExePath,
                Arguments = $"x \"{path}\" -o\"{extractedPath}\" -y",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();

                // Note that this method must be async to prevent blocking UI calls
                process.WaitForExit();
            }

            ExtractedFilePath = extractedPath;
        }

        /// <summary>
        /// Deletes the extracted folder from the temp directory.
        /// </summary>
        /// <param name="path"></param>
        public void DeleteExtractedFiles(string path)
        {
            var extractedPath = Path.Combine(TempDirectory, Path.GetFileNameWithoutExtension(path));

            if (Directory.Exists(extractedPath))
            {
                Directory.Delete(extractedPath, true);
            }
        }

        /// <summary>
        /// Determines whether a specified path is a directory, or a file.
        /// </summary>
        /// <param name="path">A file or directory.</param>
        /// <returns></returns>
        private bool IsPathDirectory(string path)
        {
            var fileAttributes = File.GetAttributes(path);

            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a specfied path exists on the drive.
        /// </summary>
        /// <param name="path">A file or directory.</param>
        /// <returns></returns>
        private bool DoesPathExist(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }

            if (File.Exists(path))
            {
                return true;
            }

            return false;

        }

        public void Dispose()
        {
            if (File.Exists(ExePath))
            {
                File.Delete(ExePath);
            }

            if (Directory.Exists(TempDirectory))
            {
                Directory.Delete(TempDirectory, true);
            }
        }
    }
}
