using System;

namespace JVMSharp
{
    public static class EnumHelper
    {
        public static TEnum Parse<TEnum>(string Value, TEnum Default) where TEnum : struct
        {
            TEnum Result;
            if (Enum.TryParse(Value, out Result) == false)
            {
                Result = Default;
            }
            return Result;
        }
    }
}
