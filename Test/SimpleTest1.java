class SimpleTest1
{
	static void test()
	{
		int a = 1;
		int b = 2;
		int c = 3;
		if(c > 1)
		{
			c = a + b;
		}
		else
		{
			c = a - b;
		}
	}

	static void main()
	{
		test();
	}
}