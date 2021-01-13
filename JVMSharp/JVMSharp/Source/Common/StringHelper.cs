using System;

namespace JVMSharp
{
    public static class StringHelper
    {
        public static char GetChar(string String, int Index)
        {
            if (Index >= String.Length)
            {
                return '\0';
            }
            else
            {
                return String[Index];
            }
        }

		public static bool IgnoreCaseEqual(string String1, string String2)
		{
			bool ignoreCase = true;
			return string.Compare(String1, String2, ignoreCase) == 0;
		}

		public static bool IgnoreCaseContains(string String, string Pattern)
		{
			return String.Contains(Pattern, StringComparison.OrdinalIgnoreCase);
		}

		public static bool StartsWith(string String, string Pattern)
		{
			bool bIgnoreCase = true;
			return String.StartsWith(Pattern, bIgnoreCase, null);
		}

		public static bool EndsWith(string String, string Pattern)
		{
			bool bIgnoreCase = true;
			return String.EndsWith(Pattern, bIgnoreCase, null);
		}

		public static int FindFirstChar(string String, char Char)
		{
			return String.IndexOf(Char);
		}

		public static int FindLastChar(string String, char Char)
		{
			return String.LastIndexOf(Char);
		}

		public static int FindFirstString(string String, string Pattern)
		{
			return String.IndexOf(Pattern, System.StringComparison.OrdinalIgnoreCase);
		}

		public static bool IsUnicodeString(string String)
		{
			foreach (char Ch in String)
			{
				if (CharHelper.IsUnicodeChar(Ch))
				{
					return true;
				}
			}
			return false;
		}		

		public static bool IsIdentifierString(string String)
		{
			foreach (char Ch in String)
			{
				if (CharHelper.IsIdentifier_Fast(Ch) == false)
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsIdentifierOrUnicodeString(string String)
        {
            foreach (char Ch in String)
            {
                if (CharHelper.IsIdentifierOrUnicodeChar(Ch) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public static int FindFirstNonBlank(string String)
        {
            int Length = String.Length;
	        for (int i = 0; i < Length; i++)
	        {
		        char Char = String[i];
		        if (!CharHelper.IsBlankChar(Char))
		        {
			        return i;
		        }
            }
	        return -1;
        }

		static TextFileLineEnding DefaultFileLineEnding = TextFileLineEnding.Windows;

		public static void SetDefaultFileLineEnding(TextFileLineEnding InDefaultFileLineEnding)
		{
			DefaultFileLineEnding = InDefaultFileLineEnding;
		}

		public static TextFileLineEnding DetectLineEnding(string String)
		{
			bool bCR = false;
			bool bLF = false;
			int Count = String.Length;
			for (int i = 0; i < Count; i++)
			{ 
				char Char = String[i];
				if (Char == '\0')
				{
					break;
				}
				if (Char == '\r')
				{
					bCR = true;
				}
				if (Char == '\n')
				{
					bLF = true;
				}
			}
			if (bCR && bLF)
			{
				return TextFileLineEnding.Windows;
			}
			else if (!bCR && bLF)
			{
				return TextFileLineEnding.Unix;
			}
			else
			{
				return DefaultFileLineEnding;
			}
		}

		public static string GetLineEndingString(TextFileLineEnding TextFileLineEnding)
		{
			switch (TextFileLineEnding)
			{
			case TextFileLineEnding.Windows:
				return "\r\n";
			case TextFileLineEnding.Unix:
				return "\n";
			default:
				DebugHelper.Assert(false);
				return "\n";
			}
		}
    }
}
