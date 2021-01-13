class Console
{
	public native static void println(String s);
	public native static void println(int a);
}

class ConsoleTest
{
	public static void main()
	{
		Console.println("Hello Console!");
	}
}