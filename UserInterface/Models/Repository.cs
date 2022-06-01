using System;
using System.Collections.Generic;

namespace UserInterface.Models
{
    public abstract class Repository<T>
    {
        public abstract T GetById(int id);
        public abstract IList<T> GetAll();
        public abstract void Edit(T obj);
        public abstract void Insert(T obj);
        public abstract bool Delete(int id);

    }

    public static class MyExtension
    {
        public static string ToFinancialYear(this DateTime dateTime)
        {
            string Fy = (dateTime.Month >= 4 ? dateTime.ToString("yyyy") + "-" + dateTime.AddYears(1).ToString("yy") : dateTime.AddYears(-1).ToString("yyyy") + "-" + dateTime.ToString("yy"));
            return Fy;
        }
        public static string ToFyMonth(this DateTime dateTime)
        {
            string month = dateTime.Month < 10 ? '0' + dateTime.Month.ToString() : dateTime.Month.ToString();

            return month + "/" + dateTime.Year;
        }
        public static string ToFY(this DateTime dateTime)
        {
            string Fy = (dateTime.Month >= 4 ? dateTime.ToString("yy") + "" + dateTime.AddYears(1).ToString("yy") : dateTime.AddYears(-1).ToString("yy") + "" + dateTime.ToString("yy"));
            return Fy;
        }

        public static string ToInvoiceFY(this DateTime dateTime)
        {
            string Fy = (dateTime.Month >= 4 ? dateTime.ToString("yy") + "-" + dateTime.AddYears(1).ToString("yy") : dateTime.AddYears(-1).ToString("yy") + "-" + dateTime.ToString("yy"));
            return Fy;
        }

        public static string ToInvoiceFullFY(this DateTime dateTime)
        {
            string Fy = (dateTime.Month >= 4 ? dateTime.ToString("yyyy") + "-" + dateTime.AddYears(1).ToString("yy") : dateTime.AddYears(-1).ToString("yyyy") + "-" + dateTime.ToString("yy"));
            return Fy;
        }

        public static string SpellNumber(this string MyNumber)
        {
            string Result = "";
            try
            {
                MyNumber = "0000000000" + MyNumber;
                if (MyNumber.Contains(".") == true)
                {
                    int decimalPlace = MyNumber.IndexOf(".");
                    string afterDecimal = MyNumber.Substring(decimalPlace + 1, MyNumber.Length - (decimalPlace + 1));
                    string beforeDecimal = MyNumber.Substring(0, decimalPlace);
                    MyNumber = beforeDecimal.ToString();
                    Result = Result = GetMillion(MyNumber.ToString()) + GetThoudand(MyNumber.ToString()) + GetHundreds(MyNumber.Substring(MyNumber.Length - 3, 3));
                    MyNumber = "000" + afterDecimal.ToString();
                    if (Convert.ToDouble(MyNumber) != 0)
                    {
                        Result = Result + " And " + GetHundreds(MyNumber.Substring(MyNumber.Length - 3, 3));
                    }
                }
                else
                {
                    if (Convert.ToInt32(MyNumber) != 0)
                    {
                        Result = GetMillion(MyNumber.ToString()) + GetThoudand(MyNumber.ToString()) + GetHundreds(MyNumber.Substring(MyNumber.Length - 3, 3));
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return Result;
        }

        private static string GetMillion(string MyNumber)
        {
            string Result = "";
            if (Convert.ToInt32(MyNumber.Substring(MyNumber.Length - 9, 3)) != 0)
            {
                Result = GetHundreds(MyNumber.Substring(MyNumber.Length - 9, 3)) + " Million ";
            }
            return Result;
        }

        private static string GetThoudand(string MyNumber)
        {
            string Result = "";
            if (Convert.ToInt32(MyNumber.Substring(MyNumber.Length - 6, 3)) != 0)
            {
                Result = GetHundreds(MyNumber.Substring(MyNumber.Length - 6, 3)) + " Thousand";
            }
            return Result;
        }

        private static string GetHundreds(string MyNumber)
        {
            string Result = "";
            if (Convert.ToInt32(MyNumber) == 0)
            {
                Result = "";
            }
            if (MyNumber.Substring(MyNumber.Length - 3, 1) != "0")
            {
                Result = GetDigit(MyNumber.Substring(0, 1)) + " Hundred";
            }

            if (MyNumber.Substring(MyNumber.Length - 2, 1) != "0")
            {
                Result = Result + GetTens(MyNumber.Substring(MyNumber.Length - 2, 2));
            }
            else
            {
                Result = Result + GetDigit(MyNumber.Substring(MyNumber.Length - 1, 1));
            }

            return Result;
        }

        private static string GetDigit(string MyNumber)
        {
            switch (Convert.ToInt32(MyNumber))
            {
                case 1: return " One";
                case 2: return " Two";
                case 3: return " Three";
                case 4: return " Four";
                case 5: return " Five";
                case 6: return " Six";
                case 7: return " Seven";
                case 8: return " Eight";
                case 9: return " Nine";
                default: return "";
            }
        }

        private static string GetTens(string MyNumber)
        {
            string Result = "";
            if (MyNumber.Substring(0, 1) == "1")
            {
                switch (Convert.ToInt32(MyNumber))
                {
                    case 10: Result = " Ten";
                        break;
                    case 11: Result = " Eleven";
                        break;
                    case 12: Result = " Twelve";
                        break;
                    case 13: Result = " Thirteen";
                        break;
                    case 14: Result = " Fourteen";
                        break;
                    case 15: Result = " Fifteen";
                        break;
                    case 16: Result = " Sixteen";
                        break;
                    case 17: Result = " Seventeen";
                        break;
                    case 18: Result = " Eighteen";
                        break;
                    case 19: Result = " Nineteen";
                        break;
                    default: Result = "";
                        break;
                }
            }
            else
            {
                switch (Convert.ToInt32(MyNumber.Substring(0, 1)))
                {
                    case 2: Result = " Twenty";
                        break;
                    case 3: Result = " Thirty";
                        break;
                    case 4: Result = " Forty";
                        break;
                    case 5: Result = " Fifty";
                        break;
                    case 6: Result = " Sixty";
                        break;
                    case 7: Result = " Seventy";
                        break;
                    case 8: Result = " Eighty";
                        break;
                    case 9: Result = " Ninety";
                        break;
                    default: Result = "";
                        break;
                }
                Result = Result + GetDigit(MyNumber.Substring(1, 1));
            }
            return Result;
        }

    }


}