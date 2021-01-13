class FibonacciTest
{
	public static long fibonacci(long n)
	{
		if (n <= 1)
		{
			return n;
		}
		else
		{
			return fibonacci(n - 1) + fibonacci(n - 2);
		}
	}

	public static void main()
	{
		long x = fibonacci(30);
	}
}