namespace JVMSharp
{
	public class CharHelper
	{
		static char[] _CharPropertyTable = new char[256]
		{
			'\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0',
			'\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0',
			'\0','\0','\0','\0','a', '\0','\0','\0','\0','\0','\0','\0','\0','\0','\0','\0',
			'd', 'd', 'd', 'd', 'd', 'd', 'd', 'd', 'd', 'd', '\0','\0','\0','\0','\0','\0',
			'\0','a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
			'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', '\0','\0','\0','\0','_',
			'\0','a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
			'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', '\0','\0','\0','\0','\0',
			'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
			'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
			'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
			'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
			'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
			'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
			'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a',
			'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', '\0',
		};

		static bool[] _IsLuaCharTable = new bool[256]
		{
			false, false, false, false, false, false, false, false, false, true,  true,  true,  true,  true,  false, false,
			false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
			true,  false, true,  true,  false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,
			true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false,
			false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,
			true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false, true,  true,  true,
			false, true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,
			true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  false,
			false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
		};

		public static bool IsAlpha_Fast(char Char)
		{
			return _CharPropertyTable[(byte)Char] == 'a';
		}

		public static bool IsDigit_Fast(char Char)
		{
			return _CharPropertyTable[(byte)Char] == 'd';
		}

		public static bool IsIdentifier_Fast(char Char)
		{
			return _CharPropertyTable[(byte)Char] != '\0';
		}

		public static bool IsLuaChar_Fast(char Char) 
		{
			return _IsLuaCharTable[(byte)Char];
		}

		public static bool IsUnicodeChar(char Char)
		{
			return Char >= 128;
		}

		public static bool IsIdentifierOrUnicodeChar(char Char)
		{
			if (IsIdentifier_Fast(Char) || IsUnicodeChar(Char))
			{
				return true;
			}
			return false;
		}

		public static bool IsPunctuationChar(char Char)
		{
			if (Char == '_')
			{
				return false;
			}
			if (Char >= 128)
			{
				return false;
			}
			return char.IsPunctuation(Char);
		}

		public static bool IsBlankChar(char Char)
		{
			return (Char == ' ') || (Char == '\f') || (Char == '\t') || (Char == '\v') || (Char == '\r') || (Char == '\n');
		}

		public static bool IsHexDigit(char Char)
		{
			return IsDigit_Fast(Char) || (Char >= 'A' && Char <= 'F') || (Char >= 'a' && Char <= 'f');
		}

		public static bool IsOctDigit(char Char)
		{
			return (Char >= '0' && Char <= '7');
		}

        public static uint OctDigitToNumber(char Char)
        {
            return (uint)(Char - '0');
        }

        public static uint HexDigitToNumber(char Char)
		{
			if (Char >= '0' && Char <= '9')
			{
				return (uint)(Char - '0');
			}
			else if (Char >= 'A' && Char <= 'F')
			{
				return (uint)(Char - 'A' + 10);
			}
			else if (Char >= 'a' && Char <= 'f')
			{
				return (uint)(Char - 'a' + 10);
			}
			return 0;
		}
	}
}
