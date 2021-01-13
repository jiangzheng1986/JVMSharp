class LookupSwitchTest
{
	public static void main()
	{
		int a = 1022;
		switch (a)
		{
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
			a = 0;
			break;
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
			a = 10;
			break;
		case 1020:
		case 1021:
		case 1022:
		case 1023:
		case 1024:
		case 1025:
		case 1026:
		case 1027:
		case 1028:
		case 1029:
			a = 1020;
			break;
		}
		Console.println(a);
	}
}