using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace Tavi.Core.UI
{
    public static class EncryptHelper
    {
        public static string EncryptMD5(string key)
        {
            UTF8Encoding Unic = new UTF8Encoding();
            byte[] bytes = Unic.GetBytes(key);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] ketqua = md5.ComputeHash(bytes);

            return BitConverter.ToString(ketqua);
        }

        public static string EncryptSHA(string Password)
        {
            System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
            byte[] hashBytes = encoding.GetBytes(Password);

            //Compute the SHA-1 hash
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] cryptPassword = sha1.ComputeHash(hashBytes);

            return BitConverter.ToString(cryptPassword);
        }
    }
}
