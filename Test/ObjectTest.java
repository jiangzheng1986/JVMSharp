class ObjectTest
{
	public static int staticVar;
	public static long staticVar1;
	public static int staticVar2;
	public int instanceVar;
	public double instanceVar1;
	public int instanceVar2;

	public static void main()
	{
		ObjectTest test = new ObjectTest();
		int x = 32768;
		ObjectTest.staticVar = x;
		x = ObjectTest.staticVar;
		test.instanceVar = x;
		x = test.instanceVar;
		Object obj = test;
		if (obj instanceof ObjectTest)
		{
			test = (ObjectTest)obj;
		}
	}
}