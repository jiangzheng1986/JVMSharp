class ExceptionTest
{
	public static void main()
	{
		try
		{
			Console.println("try 1");
			int a = 0;
			if( a <= 0)
			{
				throw new Exception();
			}
			Console.println("try 2");
		}
		catch(Exception e)
		{
			Console.println("catch");
		}
		finally
		{
			Console.println("finally");
		}
	}
}