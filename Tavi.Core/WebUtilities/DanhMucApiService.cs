using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavi.Core.Models;

namespace Tavi.Core.WebUtilities
{
    public class DanhMucApiService
    {
        private static string contentType = "application/json";
        private static string ApiUri = ConfigurationManager.AppSettings["Tavi_Api_Url"].ToString();
        private static RestClient _client = new RestClient(ApiUri);

        #region Don vi hanh chinh
        /// <summary>
        /// Get danh sach don vi hanh chinh cap 1 - cap tinh, thanh pho truc thuoc trung uong
        /// </summary>
        /// <returns></returns>
        public static List<DonViHanhChinhCap1> DanhSach_DVHC_Cap1()
        {
            var request = new RestRequest("DonViHanhChinhCap1/DanhSach", Method.GET);
            request.AddHeader("Content-Type", contentType);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var result = _client.Execute(request);
            if (result != null && result.Content != null)
            {
                try
                {
                    var resultEntity = JsonConvert.DeserializeObject<List<DonViHanhChinhCap1>>(result.Content);
                    return resultEntity;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            return new List<DonViHanhChinhCap1>();
        }

        /// <summary>
        /// Get danh sach don vi hanh chinh cap 2 - cap quan, huyen, thanh pho truc thuoc tinh
        /// </summary>
        /// <returns></returns>
        public static List<DonViHanhChinhCap2> DanhSach_DVHC_Cap2(int? parentId = 0)
        {
            if (parentId <= 0 || parentId == null) return new List<DonViHanhChinhCap2>();

            var request = new RestRequest("DonViHanhChinhCap2/DanhSach", Method.GET);
            request.AddHeader("Content-Type", contentType);
            request.AddParameter("parentId", parentId);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var result = _client.Execute(request);
            if (result != null && result.Content != null)
            {
                try
                {
                    var resultEntity = JsonConvert.DeserializeObject<List<DonViHanhChinhCap2>>(result.Content);
                    return resultEntity;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            return new List<DonViHanhChinhCap2>();
        }

        /// <summary>
        /// Get danh sach don vi hanh chinh cap 3 - cap phuong xa
        /// </summary>
        /// <returns></returns>
        public static List<DonViHanhChinhCap3> DanhSach_DVHC_Cap3(int? parentId = 0)
        {
            if (parentId <= 0 || parentId == null) return new List<DonViHanhChinhCap3>();

            var request = new RestRequest("DonViHanhChinhCap3/DanhSach", Method.GET);
            request.AddHeader("Content-Type", contentType);
            request.AddParameter("parentId", parentId);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var result = _client.Execute(request);
            if (result != null && result.Content != null)
            {
                try
                {
                    var resultEntity = JsonConvert.DeserializeObject<List<DonViHanhChinhCap3>>(result.Content);
                    return resultEntity;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            return new List<DonViHanhChinhCap3>();
        }
        #endregion

        #region Don vi tinh
        /// <summary>
        /// Get danh sach don vi tinh
        /// </summary>
        /// <returns></returns>
        public static List<DonViTinh> DanhSach_DonViTinh()
        {
            var request = new RestRequest("DonViTinh/DanhSach", Method.GET);
            request.AddHeader("Content-Type", contentType);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var result = _client.Execute(request);
            if (result != null && result.Content != null)
            {
                try
                {
                    var resultEntity = JsonConvert.DeserializeObject<List<DonViTinh>>(result.Content);
                    return resultEntity;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            return new List<DonViTinh>();
        }
        #endregion

        #region Loai Van Ban
        public static List<LoaiVanBan> DanhSach_LoaiVanBan()
        {
            var request = new RestRequest("LoaiVanBanHanhChinh/DanhSach", Method.GET);
            request.AddHeader("Content-Type", contentType);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var result = _client.Execute(request);
            if (result != null && result.Content != null)
            {
                try
                {
                    var resultEntity = JsonConvert.DeserializeObject<List<LoaiVanBan>>(result.Content);
                    return resultEntity;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            return new List<LoaiVanBan>();
        }
        #endregion

        #region Linh Vuc
        public static List<LinhVuc> DanhSach_LinhVuc()
        {
            var request = new RestRequest("LinhVuc/DanhSach", Method.GET);
            request.AddHeader("Content-Type", contentType);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var result = _client.Execute(request);
            if (result != null && result.Content != null)
            {
                try
                {
                    var resultEntity = JsonConvert.DeserializeObject<List<LinhVuc>>(result.Content);
                    return resultEntity;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            return new List<LinhVuc>();
        }
        #endregion
    }
}
