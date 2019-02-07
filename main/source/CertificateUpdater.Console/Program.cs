using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace CertificateUpdater.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Check().ContinueWith((shouldRenew) =>
            {
                if (shouldRenew.Result)
                {
                    System.Console.WriteLine("Should renew certificate");
                    Renew().ContinueWith((isRenewed) => { }).Wait();
                }
                else
                {
                    System.Console.WriteLine("Should not renew certificate");
                }
            }).Wait();
            
        }

        static async Task<bool> Check()
        {
            var checker = new CertificateChecker("c:\\temp\\key.txt", "c:\\temp\\certificate.pem", "stevenderoover@gmail.com");
            return await checker.CheckShouldRenewCertificate();
        }

        static async Task<bool> Renew()
        {
            var checker = new CertificateChecker("c:\\temp\\key.txt", "c:\\temp\\certificate.pem", "stevenderoover@gmail.com", (files) =>
            {
                using (var zipStream = ZipFiles(files))
                {
                    SaveZipFile(zipStream);
                }                
                System.Console.WriteLine("Files are saved to c:\\temp\\challenges.zip.  Copy files to server, and then press enter");
                System.Console.Read();
            }, SavePemToRouter);

            return await checker.RenewCertificate(new string[] { "cloud.stovem.com", "cp.stovem.com", "plex.stovem.com", "radarr.stovem.com", "sab.stovem.com", "sonarr.stovem.com", "transmission.stovem.com", "www.stovem.com" }, "www.stovem.com");
        }

        private static void SavePemToRouter(string pem)
        {
            
        }

        private static MemoryStream ZipFiles(Dictionary<string, byte[]> files)
        {
            var memStream = new MemoryStream();
            using (var archive = new ZipArchive(memStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    var entry = archive.CreateEntry(file.Key, CompressionLevel.Optimal);
                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(file.Value);
                    }
                }
            }
            return memStream;
        }
        private static void SaveZipFile(MemoryStream zipStream)
        {
            using (var fileStream = new FileStream(@"C:\Temp\challenges.zip", FileMode.OpenOrCreate))
            {
                zipStream.Seek(0, SeekOrigin.Begin);
                zipStream.CopyTo(fileStream);
            }
        }
    }
}
