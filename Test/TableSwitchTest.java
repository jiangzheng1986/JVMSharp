class TableSwitchTest
{
	public static void main()
	{
		int a = 21;
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
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
			a = 20;
			break;
		}
		Console.println(a);
	}
}