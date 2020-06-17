using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Tavi.Core.UI
{
    public static class UploadFile
    {
        public static string folderImg = "~/UploadFile/Image/";
        public static string folderOffice = "~/UploadFile/Office/";

        public static Upload UploadImage(HttpFileCollectionBase fileImage)
        {
            Upload upload = new Upload();
            bool OutPut = false;

            if (fileImage.Count >= 1)
            {
                HttpPostedFileBase file = fileImage[0];
                if (file.FileName != null && file.FileName != "")
                {
                    string fileContentType = file.ContentType;
                    byte[] tempFileBytes = new byte[file.ContentLength]; // getting filebytes
                    var data = file.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(file.ContentLength));
                    var types = ImageFile.FileType.Image;  // Setting Image type
                    OutPut = ImageFile.isValidFile(tempFileBytes, types, fileContentType); // Validat
                    if (OutPut)
                    {
                        string ramdom = Guid.NewGuid().ToString();
                        string fordelUpload = HttpContext.Current.Server.MapPath(folderImg) + ramdom + "_" + file.FileName;
                        string fileHost = folderImg + ramdom + "_" + file.FileName;
                        file.SaveAs(fordelUpload);
                        upload.FileName = file.FileName;
                        upload.FileMaps = fileHost;
                    }
                }
            }
            upload.OutPut = OutPut;
            return upload;
        }
    }
    public class Upload
    {
        public Upload()
        {

        }
        public bool OutPut { get; set; }
        public string FileName { get; set; }
        public string FileMaps { get; set; }
    }
}
