using JVMSharp;
using System;
using System.Globalization;

namespace EditorUI
{
    public static class MathHelper
    {
        public static sbyte ParseSByte(string String)
        {
            sbyte Value;
            sbyte.TryParse(String, out Value);
            return Value;
        }

        public static byte ParseByte(string String)
        {
            byte Value;
            byte.TryParse(String, out Value);
            return Value;
        }

        public static short ParseShort(string String)
        {
            short Value;
            short.TryParse(String, out Value);
            return Value;
        }

        public static ushort ParseUShort(string String)
        {
            ushort Value;
            ushort.TryParse(String, out Value);
            return Value;
        }

        public static int ParseInt(string String)
        {
            int Value;
            int.TryParse(String, out Value);
            return Value;
        }

        public static uint ParseUInt(string String)
        {
            if (String.Contains("u"))
            {
                String = String.Replace("u", "");
            }
            if (String.Contains("U"))
            {
                String = String.Replace("U", "");
            }
            uint Value;
            uint.TryParse(String, out Value);
            return Value;
        }

        public static long ParseLong(string String)
        {
            if (String.Contains("l"))
            {
                String = String.Replace("l", "");
            }
            if (String.Contains("L"))
            {
                String = String.Replace("L", "");
            }
            long Value;
            long.TryParse(String, out Value);
            return Value;
        }

        public static ulong ParseULong(string String)
        {
            if (String.Contains("u"))
            {
                String = String.Replace("u", "");
            }
            if (String.Contains("U"))
            {
                String = String.Replace("U", "");
            }
            if (String.Contains("l"))
            {
                String = String.Replace("l", "");
            }
            if (String.Contains("L"))
            {
                String = String.Replace("L", "");
            }
            ulong Value;
            ulong.TryParse(String, out Value);
            return Value;
        }

        public static ulong ParseHex(string String)
        {
            if (String.Contains("u"))
            {
                String = String.Replace("u", "");
            }
            if (String.Contains("U"))
            {
                String = String.Replace("U", "");
            }
            if (String.Contains("l"))
            {
                String = String.Replace("l", "");
            }
            if (String.Contains("L"))
            {
                String = String.Replace("L", "");
            }
            return Convert.ToUInt64(String, 16);
        }

        public static int ParseHexInt(string String)
        {
            return (int)ParseHex(String);
        }

        public static uint ParseHexUInt(string String)
        {
            return (uint)ParseHex(String);
        }

        public static long ParseHexLong(string String)
        {
            return (long)ParseHex(String);
        }

        public static ulong ParseHexULong(string String)
        {
            return ParseHex(String);
        }

        public static ulong ParseOct(string String)
        {
            if (String.Contains("u"))
            {
                String = String.Replace("u", "");
            }
            if (String.Contains("U"))
            {
                String = String.Replace("U", "");
            }
            if (String.Contains("l"))
            {
                String = String.Replace("l", "");
            }
            if (String.Contains("L"))
            {
                String = String.Replace("L", "");
            }
            return Convert.ToUInt64(String, 8);
        }

        public static int ParseOctInt(string String)
        {
            return (int)ParseOct(String);
        }

        public static uint ParseOctUInt(string String)
        {
            return (uint)ParseOct(String);
        }

        public static long ParseOctLong(string String)
        {
            return (long)ParseOct(String);
        }

        public static ulong ParseOctULong(string String)
        {
            return ParseOct(String);
        }

        public static float ParseFloat(string String)
        {
            if (String.Contains("f"))
            {
                String = String.Replace("f", "");
            }
            if (String.Contains("F"))
            {
                String = String.Replace("F", "");
            }
            float Value;
            float.TryParse(String, out Value);
            return Value;
        }

        public static double ParseDouble(string String)
        {
            if (String.Contains("d"))
            {
                String = String.Replace("d", "");
            }
            if (String.Contains("D"))
            {
                String = String.Replace("D", "");
            }
            double Value;
            double.TryParse(String, out Value);
            return Value;
        }

        public static decimal ParseDecimal(string String)
        {
            if (String.Contains("m"))
            {
                String = String.Replace("m", "");
            }
            if (String.Contains("M"))
            {
                String = String.Replace("M", "");
            }
            decimal Value;
            decimal.TryParse(String, out Value);
            return Value;
        }

        public static object ParseNumber(string String, Type NumericType)
        {
            object Value = null;
            if (NumericType == typeof(sbyte))
            {
                Value = MathHelper.ParseSByte(String);
            }
            else if (NumericType == typeof(byte))
            {
                Value = MathHelper.ParseByte(String);
            }
            else if (NumericType == typeof(short))
            {
                Value = MathHelper.ParseShort(String);
            }
            else if (NumericType == typeof(ushort))
            {
                Value = MathHelper.ParseUShort(String);
            }
            else if (NumericType == typeof(int))
            {
                Value = MathHelper.ParseInt(String);
            }
            else if (NumericType == typeof(uint))
            {
                Value = MathHelper.ParseUInt(String);
            }
            else if (NumericType == typeof(long))
            {
                Value = MathHelper.ParseLong(String);
            }
            else if (NumericType == typeof(ulong))
            {
                Value = MathHelper.ParseULong(String);
            }
            else if (NumericType == typeof(float))
            {
                Value = MathHelper.ParseFloat(String);
            }
            else if (NumericType == typeof(double))
            {
                Value = MathHelper.ParseDouble(String);
            }
            else if (NumericType == typeof(decimal))
            {
                Value = MathHelper.ParseDecimal(String);
            }
            return Value;
        }

        public static object ParseNumber(string String, out string ErrorMessage)
        {
            ErrorMessage = null;
            int TypeCount = 0;
            bool bHex = false;
            bool bOct = false;
            bool bUnsigned = false;
            bool bLong = false;
            bool bFloat = false;
            bool bDouble = false;
            bool bDecimal = false;
            int TokenStringLength = String.Length;
            if (TokenStringLength >= 2)
            {
                char Char1 = String[0];
                if (Char1 == '0')
                {
                    char Char2 = String[1];
                    if (Char2 == 'x' || Char2 == 'X')
                    {
                        bHex = true;
                        TypeCount++;
                    }
                    else if (CharHelper.IsDigit_Fast(Char2))
                    {
                        bOct = true;
                        TypeCount++;
                    }
                }
            }
            foreach (char Char in String)
            {
                if (Char == '.')
                {
                    bDouble = true;
                }
                else if (Char == 'e' || Char == 'E')
                {
                    bDouble = true;
                }
                else if (Char == '+' || Char == '-')
                {
                    bDouble = true;
                }
                else if (Char == 'u' || Char == 'U')
                {
                    bUnsigned = true;
                }
                else if (Char == 'l' || Char == 'L')
                {
                    bLong = true;
                }
                else if (Char == 'f' || Char == 'f')
                {
                    bFloat = true;
                    TypeCount++;
                }
                else if (Char == 'd' || Char == 'D')
                {
                    bDouble = true;
                    TypeCount++;
                }
                else if (Char == 'm' || Char == 'M')
                {
                    bDecimal = true;
                    TypeCount++;
                }
            }
            if (TypeCount > 1)
            {
                ErrorMessage = "Invalid number string.";
                return 0;
            }
            if (bHex || bOct)
            {
                if (bFloat || bDouble || bDecimal)
                {
                    ErrorMessage = "Invalid number string.";
                    return 0;
                }
            }
            if (bFloat || bDouble || bDecimal)
            {
                if (bUnsigned || bLong)
                {
                    ErrorMessage = "Invalid number string.";
                    return 0;
                }
            }
            if (bFloat)
            {
                return MathHelper.ParseFloat(String);
            }
            else if (bDecimal)
            {
                return MathHelper.ParseDecimal(String);
            }
            else if (bDouble)
            {
                return MathHelper.ParseDouble(String);
            }
            else if (bHex)
            {
                if (bLong)
                {
                    if (bUnsigned)
                    {
                        return MathHelper.ParseHexULong(String);
                    }
                    else
                    {
                        return MathHelper.ParseHexLong(String);
                    }
                }
                else
                {
                    if (bUnsigned)
                    {
                        return MathHelper.ParseHexUInt(String);
                    }
                    else
                    {
                        return MathHelper.ParseHexInt(String);
                    }
                }
            }
            else if (bOct)
            {
                if (bLong)
                {
                    if (bUnsigned)
                    {
                        return MathHelper.ParseOctULong(String);
                    }
                    else
                    {
                        return MathHelper.ParseOctLong(String);
                    }
                }
                else
                {
                    if (bUnsigned)
                    {
                        return MathHelper.ParseOctUInt(String);
                    }
                    else
                    {
                        return MathHelper.ParseOctInt(String);
                    }
                }
            }
            else
            {
                if (bLong)
                {
                    if (bUnsigned)
                    {
                        return MathHelper.ParseULong(String);
                    }
                    else
                    {
                        return MathHelper.ParseLong(String);
                    }
                }
                else
                {
                    if (bUnsigned)
                    {
                        return MathHelper.ParseUInt(String);
                    }
                    else
                    {
                        return MathHelper.ParseInt(String);
                    }
                }
            }
        }
    }
}
