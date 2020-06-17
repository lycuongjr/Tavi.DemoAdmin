using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Mvc;
namespace Tavi.Core.UI
{
  public static  class LichViet
    {
        public static DateTime NgayDauTienNam(int nam)
        {
            return new DateTime(nam, 1, 1);
        }

        public static List<SelectListItem> LichTuan(int nam, int tuanhientai)
        {
            List<SelectListItem> lich = new List<SelectListItem>();
            DateTime ngaydau = NgayDauTienNam(nam);
            DayOfWeek thu = ngaydau.DayOfWeek;
            int i = 0;
            switch (thu)
            {
                case DayOfWeek.Monday:
                    i = 6;
                    break;
                case DayOfWeek.Tuesday:
                    i = 5;
                    break;
                case DayOfWeek.Wednesday:
                    i = 4;
                    break;
                case DayOfWeek.Thursday:
                    i = 3;
                    break;
                case DayOfWeek.Friday:
                    i = 2;
                    break;
                case DayOfWeek.Saturday:
                    i = 1;
                    break;
                case DayOfWeek.Sunday:
                    i = 0;
                    break;
            }
            //tuan dau tien
            string tuan = "";
            tuan = "Tuần 1 ( Từ: " + ngaydau.ToString("dd/MM/yyyy") + " đến " + ngaydau.AddDays(i).ToString("dd/MM/yyyy") + " )";
            lich.Add(new SelectListItem { Text = tuan, Value = "1", Selected = Equals(1, tuanhientai) });
            ngaydau = ngaydau.AddDays(i + 1);

            //tuan tieo thep
            int t = 2;
            while (ngaydau.Date <= new DateTime(nam, 12, 31).Date)
            {

                for (int x = 0; x < 6; x++)
                {
                    if (x == 5)
                    {
                        tuan = "Tuần " + t.ToString() + " ( Từ: " + ngaydau.ToString("dd/MM/yyyy") + " đến " + ngaydau.AddDays(x + 1).ToString("dd/MM/yyyy") + " )";
                        break;
                    }
                    if (ngaydau.AddDays(x).Date == new DateTime(nam, 12, 31).Date)
                    {
                        tuan = "Tuần " + t.ToString() + " ( Từ: " + ngaydau.ToString("dd/MM/yyyy") + " đến " + ngaydau.AddDays(x).ToString("dd/MM/yyyy") + " )";
                        break;
                    }
                }
                lich.Add(new SelectListItem { Text = tuan, Value = t.ToString(), Selected = Equals(t, tuanhientai) });
                ngaydau = ngaydau.AddDays(7);
                t = t + 1;
            }
            return lich;
        }
        public static int TuanHienTai()
        {
            DateTime ngaydau = new DateTime(DateTime.Now.Year, 1, 1);
            DayOfWeek thu = ngaydau.DayOfWeek;
            int i = 0;
            switch (thu)
            {
                case DayOfWeek.Monday:
                    i = 6;
                    break;
                case DayOfWeek.Tuesday:
                    i = 5;
                    break;
                case DayOfWeek.Wednesday:
                    i = 4;
                    break;
                case DayOfWeek.Thursday:
                    i = 3;
                    break;
                case DayOfWeek.Friday:
                    i = 2;
                    break;
                case DayOfWeek.Saturday:
                    i = 1;
                    break;
                case DayOfWeek.Sunday:
                    i = 0;
                    break;
            }
            if (ngaydau.AddDays(i).Date >= DateTime.Now.Date)
                return 1;
            ngaydau = ngaydau.AddDays(i + 1);
            for (int j = 2; j < 57; j++)
            {
                if (ngaydau.AddDays(6).Date >= DateTime.Now.Date)
                    return j;
                ngaydau = ngaydau.AddDays(7);
            }
            return 1;
        }
        public static DateTime NgayDauTienCuaTuan(int tuan, int nam)
        {
            if (tuan == 1)
            {
                return new DateTime(nam, 1, 1);
            }
            else
            {
                DateTime ngaydau = NgayDauTienNam(nam);
                DayOfWeek thu = ngaydau.DayOfWeek;
                int i = 0;
                switch (thu)
                {
                    case DayOfWeek.Monday:
                        i = 6;
                        break;
                    case DayOfWeek.Tuesday:
                        i = 5;
                        break;
                    case DayOfWeek.Wednesday:
                        i = 4;
                        break;
                    case DayOfWeek.Thursday:
                        i = 3;
                        break;
                    case DayOfWeek.Friday:
                        i = 2;
                        break;
                    case DayOfWeek.Saturday:
                        i = 1;
                        break;
                    case DayOfWeek.Sunday:
                        i = 0;
                        break;
                }

                ngaydau = ngaydau.AddDays(i + 1);

                //tuan tieo thep
                int t = 2;
                while (ngaydau.Date <= new DateTime(nam, 12, 31).Date)
                {
                    if (t == tuan)
                    {
                        return ngaydau;
                    }
                    ngaydau = ngaydau.AddDays(7);
                    t = t + 1;
                }
            }
            return DateTime.Now;
        }
        public static DateTime NgayCuoiCuaTuan(int tuan, int nam)
        {

            DateTime ngaydau = NgayDauTienNam(nam);
            DayOfWeek thu = ngaydau.DayOfWeek;
            int i = 0;
            switch (thu)
            {
                case DayOfWeek.Monday:
                    i = 6;
                    break;
                case DayOfWeek.Tuesday:
                    i = 5;
                    break;
                case DayOfWeek.Wednesday:
                    i = 4;
                    break;
                case DayOfWeek.Thursday:
                    i = 3;
                    break;
                case DayOfWeek.Friday:
                    i = 2;
                    break;
                case DayOfWeek.Saturday:
                    i = 1;
                    break;
                case DayOfWeek.Sunday:
                    i = 0;
                    break;
            }
            //tuan dau tien
            if (tuan == 1)
                return ngaydau.AddDays(i);

            ngaydau = ngaydau.AddDays(i + 1);

            //tuan tieo thep
            int t = 2;
            while (ngaydau.Date <= new DateTime(nam, 12, 31).Date)
            {
                if (t == tuan)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        if (x == 5)
                        {
                            return ngaydau.AddDays(x + 1);

                        }
                        if (ngaydau.AddDays(x).Date == new DateTime(nam, 12, 31).Date)
                        {
                            return ngaydau.AddDays(x);

                        }
                    }
                }
                ngaydau = ngaydau.AddDays(7);
                t = t + 1;
            }
            return DateTime.Now;
        }
        public static DateTime GetLastOccurenceOfDay(DateTime value, DayOfWeek dayOfWeek)
        {
            int daysToAdd = dayOfWeek - value.DayOfWeek;
            if (daysToAdd < 1)
            {
                daysToAdd -= 7;
            }
            return value.AddDays(daysToAdd);
        }

        public static DateTime GetFirstDayOfWeek(int year, int weekNumber, DayOfWeek dayOfWeek)
        {
            return GetLastOccurenceOfDay(new DateTime(year, 1, 1).AddDays(7 * weekNumber), dayOfWeek);
        }

        public static int GetCurrentWeek()
        {
            int iReturn = 0;
            //int dayOfYear = DateTime.Now.DayOfYear;
            double dayOfYear = DateTime.Now.Subtract(GetFirstDayOfWeek(DateTime.Now.Year, 1, DayOfWeek.Monday)).TotalDays;
            //HttpContext.Current.Response.Write(iDay);
            if ((dayOfYear % 7) == 0)
            {
                iReturn = (int)(dayOfYear / 7);
            }
            else
            {
                iReturn = (int)Math.Floor(dayOfYear / 7) + 1;
            }
            return iReturn;
        }

        public static int GetWeekNumberOfMonthOfYear(int year, int month, int day)
        {
            switch (month)
            {
                case 2:
                    if (DateTime.IsLeapYear(year))
                    {
                        if (day == 31 || day == 30)
                        {
                            day = 29;
                        }
                    }
                    else
                    {
                        if (day == 31 || day == 30 || day == 29)
                        {
                            day = 28;
                        }
                    }
                    break;
                case 4:
                    if (day == 31)
                    {
                        day = 30;
                    }
                    break;
                case 6:
                    if (day == 31)
                    {
                        day = 30;
                    }
                    break;
                case 9:
                    if (day == 31)
                    {
                        day = 30;
                    }
                    break;
                case 11:
                    if (day == 31)
                    {
                        day = 30;
                    }
                    break;
            }
            int iReturn = 0;
            DateTime dt = new DateTime(year, month, day);

            double dayOfYear = dt.Subtract(GetFirstDayOfWeek(year, 1, DayOfWeek.Monday)).TotalDays;

            if ((dayOfYear % 7) == 0)
            {
                iReturn = (int)(dayOfYear / 7);
            }
            else
            {
                iReturn = (int)Math.Floor(dayOfYear / 7) + 1;
            }
            return iReturn;
        }

        public static int DateCompare(DateTime dtimeDate1, DateTime dtimeDate2)
        {
            return dtimeDate1.CompareTo(dtimeDate2);
        }

        public static int TinhToanThoiGianHoanThanhCongViec(DateTime HanXuLy)
        {
            int SoNgay = 0;

            DateTime toDay = DateTime.Now.AddDays(1);

            while (DateTime.Compare(HanXuLy.Date, toDay.Date) >= 0)
            {

                if (KiemTraNgayLamViecTrongTuan(toDay))
                {
                    SoNgay += 1;
                }
                toDay = toDay.AddDays(1);
            }

            return SoNgay;
        }

        public static DateTime TinhToanThoiGianHoanThanhCongViec(int HanXuLy)
        {
            DateTime toDay = DateTime.Now.Date;

            while (HanXuLy > 0)
            {
                if (KiemTraNgayLamViecTrongTuan(toDay))
                {
                    HanXuLy--;
                }
                toDay = toDay.AddDays(1);
                while (!KiemTraNgayLamViecTrongTuan(toDay))
                {
                    toDay = toDay.AddDays(1);
                }
            }

            return toDay;
        }

        public static bool KiemTraNgayLamViecTrongTuan(DateTime dateTime) // ngày làm việc trong tuần tính từ thứ 2 - thứ 6 
        {
            DayOfWeek ngaytrongtuan = dateTime.DayOfWeek;

            switch (ngaytrongtuan)
            {
                case DayOfWeek.Monday:
                    return true;
                case DayOfWeek.Tuesday:
                    return true;
                case DayOfWeek.Wednesday:
                    return true;
                case DayOfWeek.Thursday:
                    return true;
                case DayOfWeek.Friday:
                    return true;
                case DayOfWeek.Saturday:
                    return false;
                case DayOfWeek.Sunday:
                    return false;
                default:
                    return false;
            }
        }

        public static int SoTuanTheoNgay(DateTime date)
        {
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            switch (month)
            {
                case 2:
                    if (DateTime.IsLeapYear(year))
                    {
                        if (day == 31 || day == 30)
                        {
                            day = 29;
                        }
                    }
                    else
                    {
                        if (day == 31 || day == 30 || day == 29)
                        {
                            day = 28;
                        }
                    }
                    break;
                case 4:
                    if (day == 31)
                    {
                        day = 30;
                    }
                    break;
                case 6:
                    if (day == 31)
                    {
                        day = 30;
                    }
                    break;
                case 9:
                    if (day == 31)
                    {
                        day = 30;
                    }
                    break;
                case 11:
                    if (day == 31)
                    {
                        day = 30;
                    }
                    break;
            }
            int iReturn = 0;
            DateTime dt = new DateTime(year, month, day);

            double dayOfYear = dt.Subtract(GetFirstDayOfWeek(year, 1, DayOfWeek.Monday)).TotalDays;

            if ((dayOfYear % 7) == 0)
            {
                iReturn = (int)(dayOfYear / 7);
            }
            else
            {
                iReturn = (int)Math.Floor(dayOfYear / 7) + 1;
            }
            return iReturn;
        }

        public static List<SelectListItem> LichThang()
        {
            List<SelectListItem> lichthang = new List<SelectListItem>();
            for (int i = 1; i < 13; i++)
            {
                lichthang.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            return lichthang;
        }

        public static List<SelectListItem> LichNam(int? Nam)
        {
            List<SelectListItem> lichnam = new List<SelectListItem>();
            for (int i = 2010; i < DateTime.Now.Year + 1; i++)
            {
                lichnam.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = Equals(i, Nam) });
            }

            return lichnam;
        }
    }
}
