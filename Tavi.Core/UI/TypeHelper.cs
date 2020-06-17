using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavi.Core.UI
{
    public static class TypeHelper
    {
        /// <summary>
        /// Function to convert an string type to Boolean.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="bool"/> value, if fail return default value of Boolean class: false</returns>
        public static bool ToBoolean(string inputValue)
        {
            bool blnReturnValue = default(bool);
            if (!string.IsNullOrEmpty(inputValue))
            {
                Boolean.TryParse(inputValue, out blnReturnValue);
            }
            return blnReturnValue;
        }

        /// <summary>
        /// Function to convert an string type to Byte.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="byte"/> value, if fail return default value of Byte class: 0</returns>
        public static byte ToByte(string inputValue)
        {
            byte bytReturnValue = default(byte);
            if (!string.IsNullOrEmpty(inputValue))
            {
                byte.TryParse(inputValue, out bytReturnValue);
            }
            return bytReturnValue;
        }

        /// <summary>
        /// Function to convert an string type to Signed Byte.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="sbyte"/> value, if fail return default value of SByte class: 0</returns>
        public static sbyte ToSByte(string inputValue)
        {
            sbyte sbytReturnValue = default(sbyte);
            if (!string.IsNullOrEmpty(inputValue))
            {
                sbyte.TryParse(inputValue, out sbytReturnValue);
            }
            return sbytReturnValue;
        }

        /// <summary>
        /// Function to convert an string type to Integer.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="short"/> value, if fail return default value of Int16 class: 0</returns>
        public static short ToInt16(string inputValue)
        {
            short shrReturnValue = default(short);
            if (!string.IsNullOrEmpty(inputValue))
            {
                short.TryParse(inputValue, out shrReturnValue);
            }
            return shrReturnValue;
        }

        /// <summary>
        /// Function to convert an string type to Unsigned Integer.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="ushort"/> value, if fail return default value of UInt16 class: 0</returns>
        public static ushort ToUInt16(string inputValue)
        {
            ushort ushrReturnValue = default(ushort);
            if (!string.IsNullOrEmpty(inputValue))
            {
                ushort.TryParse(inputValue, out ushrReturnValue);
            }
            return ushrReturnValue;
        }

        /// <summary>
        /// Function to convert an string type to Integer.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="int"/> value, if fail return default value of Int32 class: 0</returns>
        public static int ToInt32(string inputValue)
        {
            int intReturnValue = default(int);
            if (!string.IsNullOrEmpty(inputValue))
            {
                int.TryParse(inputValue, out intReturnValue);
            }
            return intReturnValue;
        }

        /// <summary>
        /// Function to convert an string type to Unsigned Integer.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="uint"/> value, if fail return default value of UInt32 class: 0</returns>
        public static uint ToUInt32(string inputValue)
        {
            uint uintReturnValue = default(uint);
            if (!string.IsNullOrEmpty(inputValue))
            {
                uint.TryParse(inputValue, out uintReturnValue);
            }
            return uintReturnValue;
        }

        /// <summary>
        /// Function to convert an string type to Long Integer.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="long"/> value, if fail return default value of Int64 class: 0</returns>
        public static long ToInt64(string inputValue)
        {
            long lngReturnValue = default(long);
            if (!string.IsNullOrEmpty(inputValue))
            {
                long.TryParse(inputValue, out lngReturnValue);
            }
            return lngReturnValue;
        }


        /// <summary>
        /// Function to convert an string type to Unsigned Long Integer.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="ulong"/> value, if fail return default value of UInt64 class: 0</returns>
        public static ulong ToUInt64(string inputValue)
        {
            ulong ulngReturnValue = default(ulong);
            if (!string.IsNullOrEmpty(inputValue))
            {
                ulong.TryParse(inputValue, out ulngReturnValue);
            }
            return ulngReturnValue;
        }


        /// <summary>
        /// Function to convert an string type to Single-Precision Floating-Point.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="float"/> value, if fail return default value of Single class: 0.0f</returns>
        public static float ToFloat(string inputValue)
        {
            float fltReturnValue = default(float);
            if (!string.IsNullOrEmpty(inputValue))
            {
                float.TryParse(inputValue, out fltReturnValue);
            }
            return fltReturnValue;
        }

        /// <summary>
        /// Function to convert an string type to Double-Precision Floating-Point.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="double"/> value, if fail return default value of Double class: 0.0d</returns>
        public static double ToDouble(string inputValue)
        {
            double dblReturnValue = default(double);
            if (!string.IsNullOrEmpty(inputValue))
            {
                double.TryParse(inputValue, out dblReturnValue);
            }
            return dblReturnValue;
        }


        /// <summary>
        /// Function to convert an string type to Decimal.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="decimal"/> value, if fail return default value of Decimal class: 0.0m<returns>
        public static decimal ToDecimal(string inputValue)
        {
            decimal decReturnValue = default(decimal);
            if (!string.IsNullOrEmpty(inputValue))
            {
                decimal.TryParse(inputValue, out decReturnValue);
            }
            return decReturnValue;
        }

        /// <summary>
        /// Function to convert an string type to DateTime.
        /// </summary>
        /// <param name="inputValue">string type, indicate value to be parsed.</param>
        /// <returns>a <see cref="DateTime"/> value, if fail return MinValue of DateTime class</returns>
        public static DateTime ToDateTime(string inputValue)
        {
            DateTime dtmReturnValue = new DateTime(1900, 1, 1);
            if (!string.IsNullOrEmpty(inputValue))
            {
                DateTime.TryParse(inputValue, out dtmReturnValue);
            }
            return dtmReturnValue;
        }
        public static DateTime ToDate(string inputValue)
        {
            DateTime dtmReturnValue = new DateTime(1900, 1, 1);
            if (!string.IsNullOrEmpty(inputValue))
            {
                DateTime.TryParse(ddmmyyyy_to_mmddyyyy(inputValue), out dtmReturnValue);
            }
            return dtmReturnValue;
        }
        public static string ddmmyyyy_to_mmddyyyy(string strValue)
        {
            if (strValue != "")
            {
                string[] strArray = strValue.Split(new char[] { '/' });
                string d = strArray[0];
                string m = strArray[1];
                string y = strArray[2];
                return (y + "/" + m + "/" + d);
            }
            return "";
        }
        public static string ToRemove(string inputValue)
        {
            return inputValue.Replace(",false", "");
        }

        public static String ToUnsign(string str)
        {
            if (str != null && str != string.Empty)
            {
                string strFormD = str.Normalize(NormalizationForm.FormD);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < strFormD.Length; i++)
                {
                    System.Globalization.UnicodeCategory uc =
                    System.Globalization.CharUnicodeInfo.GetUnicodeCategory(strFormD[i]);
                    if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(strFormD[i]);
                    }
                }
                sb = sb.Replace('Đ', 'D');
                sb = sb.Replace('đ', 'd');
                return (sb.ToString().Normalize(NormalizationForm.FormD));
            }
            return "";
        }

        public static String ToRoman(int number)
        {
            string strRet = string.Empty;
            decimal _Number = number;
            Boolean _Flag = true;
            string[] ArrLama = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            int[] ArrNumber = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            int i = 0;
            while (_Flag)
            {
                while (_Number >= ArrNumber[i])
                {
                    _Number -= ArrNumber[i];
                    strRet += ArrLama[i];
                    if (_Number < 1)
                        _Flag = false;
                }
                i++;
            }
            return strRet;
        }

        public static String ToCurrentcyVN(string number)
        {
            if (string.IsNullOrEmpty(number)) return "";
            else return String.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:N0}", ToDouble(number));
        }

        // Compare 2 unsigned string.
        public static bool CompareString(string str1, string str2)
        {
            if (!String.IsNullOrEmpty(str1) && !String.IsNullOrEmpty(str2))
            {
                if (ToUnsign(str1.Trim()).ToLower().Contains(ToUnsign(str2.Trim()).ToLower()))
                    return true;
                if (ToUnsign(str2.Trim()).ToLower().Contains(ToUnsign(str1.Trim()).ToLower()))
                    return true;
            }
            return false;
        }

        public static bool Contains(string str1, string str2)
        {
            if (!String.IsNullOrEmpty(str1) && !String.IsNullOrEmpty(str2))
            {
                return ToUnsign(str1.Trim()).IndexOf(ToUnsign(str2.Trim()), StringComparison.OrdinalIgnoreCase) > 0;
            }
            return false;
        }
    }
}
