using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tavi.Core.UI
{
    public static class FileHelper
    {
        private static string DemoFilePath = "~/Uploads/FileDemo/";

        public static FileInfo[] GetFilesFromDirectory()
        {
            return GetFilesFromDirectory(HttpContext.Current.Server.MapPath(DemoFilePath));
        }

        public static FileInfo[] GetFilesFromDirectory(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists) return null;
            FileInfo[] files = directoryInfo.GetFiles();
            return files;
        }

        public static string UploadFile(HttpFileCollectionBase files)
        {
            return UploadFile(DemoFilePath, files);
        }

        public static string UploadFile(string path, HttpFileCollectionBase files)
        {
            var mapPath = HttpContext.Current.Server.MapPath(path);
            var result = string.Empty;

            // create directory if not exists
            DirectoryInfo directoryInfo = new DirectoryInfo(mapPath);
            if(!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            
            // add file to directory
            if (files.Count > 0)
            {
                HttpPostedFileBase file = files[0];
                if (file.FileName != null && file.FileName != "")
                {
                    string random = Guid.NewGuid().ToString();
                    string fordelUpload = mapPath + random + "_" + file.FileName;
                    if (File.Exists(fordelUpload))
                    {
                        File.Delete(fordelUpload);
                    }
                    file.SaveAs(fordelUpload);
                    result = path + random + "_" + file.FileName;
                }
            }
            return result;
        }

        public static bool RemoveFile(string path)
        {
            try
            {
                // Check if file exists with its full path    
                if (File.Exists(path))
                {
                    // If file found, delete it    
                    File.Delete(path);
                }
                return true;
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
                return false;
            }
        }
    }
}
