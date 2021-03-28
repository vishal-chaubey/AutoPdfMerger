using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.IO;

namespace folderMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathToFolder1 = @"wwwroot\Folder1";
            string pathToFolder2 = @"wwwroot\Folder2";
            string pathToFolder3 = @"wwwroot\Folder3";
            string pathToFolder4 = @"wwwroot\Folder4";
            DirectoryInfo di = new DirectoryInfo(pathToFolder1);
            FileInfo[] firstFfiles = di.GetFiles("*.pdf");
            DirectoryInfo di2 = new DirectoryInfo(pathToFolder2);
            FileInfo[] secondFfiles = di2.GetFiles("*.pdf");
            foreach (FileInfo file in firstFfiles)
            {
                try
                {
                    bool isMatch = false;
                    foreach (FileInfo sfile in secondFfiles)
                    {
                        if (sfile.Extension == ".pdf")
                        {
                            if (file.Name.Trim() == sfile.Name.Trim())
                            {
                                string destFile = Path.Combine(pathToFolder3, file.Name.Trim());
                                CombinePDFs(file, sfile, destFile);
                                isMatch = true;
                                break;
                            }
                        }
                    }
                    if(!isMatch)
                    {
                        string sourceFile = Path.Combine(pathToFolder1, file.Name.Trim());
                        string destFile = Path.Combine(pathToFolder4, file.Name.Trim());
                        if (!Directory.Exists(pathToFolder4))
                        {
                            Directory.CreateDirectory(pathToFolder4);
                        }
                        File.Copy(sourceFile, destFile, true);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: ", e.Message);
                    throw e;
                }
            }
            foreach (FileInfo sfile in secondFfiles)
            {
                bool isMatch = false;
                foreach (FileInfo ffile in firstFfiles)
                {
                    if (ffile.Extension == ".pdf")
                    {
                        if (ffile.Name.Trim() == sfile.Name.Trim())
                        {
                            isMatch = true;
                            break;
                        }
                    }
                }
                if (!isMatch)
                {
                    string sourceFile = Path.Combine(pathToFolder2, sfile.Name.Trim());
                    string destFile = Path.Combine(pathToFolder4, sfile.Name.Trim());
                    if (!Directory.Exists(pathToFolder4))
                    {
                        Directory.CreateDirectory(pathToFolder4);
                    }
                    File.Copy(sourceFile, destFile, true);
                }
            }
            Console.WriteLine("Folder Processed : Press any key to exit...");
            Console.ReadKey();
        }
        private static void CombinePDFs(FileInfo srcPdf, FileInfo srcPdf1, string destFile)
        {
            try
            {
                FileInfo file = new FileInfo(destFile);
                file.Directory.Create();
                PdfReader reader = new PdfReader(srcPdf);
                PdfReader reader1 = new PdfReader(srcPdf1);
                reader.SetUnethicalReading(true);
                reader1.SetUnethicalReading(true);
                var SourceDocument1 = new PdfDocument(reader);
                var SourceDocument2 = new PdfDocument(reader1);
                PdfDocument pdfDoc = new PdfDocument(new PdfWriter(destFile));
                PdfMerger merge = new PdfMerger(pdfDoc);
                merge.Merge(SourceDocument1, 1, SourceDocument1.GetNumberOfPages())
                .Merge(SourceDocument2, 1, SourceDocument2.GetNumberOfPages());
                SourceDocument1.Close();
                SourceDocument2.Close();
                merge.Close();
                pdfDoc.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
