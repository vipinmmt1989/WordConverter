﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordConvertor.API.Services
{
    public class WordConverter : IWordConverter
    {
        private static string ConvertDecimals(string number)
        {
            String cd = "", engOne = "";


            if (number.Length > 0)
            {
                engOne = ConvertWholeNumber(number);
            }
            else
            {
                engOne = "Zero";
            }
            cd += " " + engOne;
            return cd;
        }

        private static string ConvertOnes(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        private static string ConvertTens(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = ConvertTens(Number.Substring(0, 1) + "0") + " " + ConvertOnes(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private static string ConvertToWords(string numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "",dollar="Dollar ";
            String endStr = "Only.";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)

                {  
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents    
                        endStr = "Cents " + endStr;//Cents    
                        pointStr = ConvertDecimals(points);
                    }
                }
                if (numb.Length > 1)
                {
                    dollar = "Dollars ";
                }
                val = String.Format("{0} {1}{2} {3}{4}", ConvertWholeNumber(wholeNo).Trim(), dollar,andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }

        private static string ConvertWholeNumber(string Number)
        {

            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric    
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping    
                    String place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits)
                    {
                        case 1://ones' range    

                            word = ConvertOnes(Number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = ConvertTens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range    
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range    
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range    
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }

        public String ValidateNumber(string Number)
        {
            string isNegative = "", Word = "";
            try
            {
                Number = Convert.ToDouble(Number).ToString();

                if (Number.Contains("-"))
                {
                    isNegative = "Minus ";
                    Number = Number.Substring(1, Number.Length - 1);
                }
                if (Number == "0")
                {
                    Word = "\nZero Only";
                }
                else
                {
                    Word = isNegative + ConvertToWords(Number);
                }
            }
            catch (Exception ex)
            {
                Word = ex.Message;
            }
            return Word;
        }


    }
}
