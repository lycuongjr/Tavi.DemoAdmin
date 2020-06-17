using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavi.Core.Constants
{
    public static class Constants
    {
        /// <summary>
        /// Default page size for display grid
        /// </summary>
        public const int Default_PageSize = 10;

        /// <summary>
        /// Constants for role of user
        /// </summary>
        public static class Roles
        {
            public const string BO_MAY_TO_CHUC = "BO_MAY_TO_CHUC";
            public const string DANH_MUC_CO_QUAN = "DANH_MUC_CO_QUAN";
            public const string DANH_MUC_CAN_BO = "DANH_MUC_CAN_BO";
            public const string DANH_MUC_CHUC_VU = "DANH_MUC_CHUC_VU";
            public const string PHAN_QUYEN_SU_DUNG = "PHAN_QUYEN_SU_DUNG";
            public const string NHOM_NGUOI_DUNG = "NHOM_NGUOI_DUNG";
            public const string PHAN_QUYEN_CA_NHAN = "PHAN_QUYEN_CA_NHAN";

            // thong tin tong hop
            public const string CAU_HINH_HIEN_THI = "CAU_HINH_HIEN_THI";
            public const string CAU_HINH_HIEN_THI_ADMIN = "CAU_HINH_HIEN_THI_ADMIN";
        }
    }
}
