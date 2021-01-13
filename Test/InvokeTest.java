class InvokeBase
{
	public void instanceMethod1()
	{
		System.out.println("InvokeBase.instanceMethod1()");
	}
}

class InvokeTest extends InvokeBase
{
	public static void staticMethod()
	{
	}

	public void instanceMethod2()
	{
		System.out.println("InvokeTest.instanceMethod2()");
	}

	public void test()
	{
		InvokeTest.staticMethod();
		InvokeTest invokeTest = new InvokeTest();
		invokeTest.instanceMethod1();
		invokeTest.instanceMethod2();
	}

	public static void main()
	{
		new InvokeTest().test();
	}
}